#!/usr/bin/env python3
"""Проверяет AI-контекст репозитория и синхронизацию bridge-файлов."""

from __future__ import annotations

import argparse
import json
import os
import re
import sys
from dataclasses import dataclass
from pathlib import Path


MAX_CONTEXT_CHARS = 20_000
MAX_BRIDGE_BODY_CHARS = 500
FRONTMATTER_RE = re.compile(r"\A---\r?\n(.*?)\r?\n---(?:\r?\n|\Z)", re.DOTALL)
SKILL_NAME_RE = re.compile(r"[a-z0-9]+(?:-[a-z0-9]+)*")


@dataclass(frozen=True)
class MarkdownData:
    frontmatter: str
    keys: frozenset[str]
    body: str


@dataclass(frozen=True)
class CheckSummary:
    context_chars: int
    rule_count: int
    skill_count: int


class AgentSetupChecker:
    def __init__(self, root: Path) -> None:
        self.root = root.resolve()
        self.errors: list[str] = []

    def relative(self, path: Path) -> str:
        try:
            return path.relative_to(self.root).as_posix()
        except ValueError:
            return str(path)

    def read_text(self, path: Path) -> str:
        try:
            return path.read_text(encoding="utf-8")
        except FileNotFoundError:
            self.errors.append(f"Missing file: {self.relative(path)}")
        except UnicodeDecodeError as exc:
            self.errors.append(
                f"File is not valid UTF-8: {self.relative(path)} ({exc})"
            )
        return ""

    def markdown_data(self, path: Path) -> MarkdownData | None:
        raw = self.read_text(path)
        if not raw:
            if path.exists():
                self.errors.append(f"File is empty: {self.relative(path)}")
            return None

        match = FRONTMATTER_RE.match(raw)
        if match is None:
            self.errors.append(
                f"Missing or malformed YAML frontmatter: {self.relative(path)}"
            )
            return None

        frontmatter_keys = self.validate_frontmatter(path, match.group(1))

        body = raw[match.end() :].strip()
        if not body:
            self.errors.append(f"Markdown body is empty: {self.relative(path)}")
        return MarkdownData(
            frontmatter=match.group(1),
            keys=frozenset(frontmatter_keys),
            body=body,
        )

    def validate_frontmatter(self, path: Path, frontmatter: str) -> set[str]:
        parent_key = ""
        seen_keys: set[str] = set()

        for line_number, line in enumerate(frontmatter.splitlines(), start=2):
            if not line or "\t" in line:
                self.errors.append(
                    f"Invalid YAML frontmatter structure: "
                    f"{self.relative(path)}:{line_number}"
                )
                continue

            if line.startswith("  "):
                if parent_key != "metadata" or line.startswith("   "):
                    self.errors.append(
                        f"Invalid YAML frontmatter indentation: "
                        f"{self.relative(path)}:{line_number}"
                    )
                    continue
                prefix = "metadata."
                content = line[2:]
            elif line.startswith(" "):
                self.errors.append(
                    f"Invalid YAML frontmatter indentation: "
                    f"{self.relative(path)}:{line_number}"
                )
                continue
            else:
                parent_key = ""
                prefix = ""
                content = line

            if ":" not in content:
                self.errors.append(
                    f"Invalid YAML frontmatter entry: "
                    f"{self.relative(path)}:{line_number}"
                )
                continue

            key, raw_value = content.split(":", 1)
            if not re.fullmatch(r"[A-Za-z_][A-Za-z0-9_]*", key):
                self.errors.append(
                    f"Invalid YAML frontmatter key: "
                    f"{self.relative(path)}:{line_number}"
                )
                continue

            qualified_key = prefix + key
            if qualified_key in seen_keys:
                self.errors.append(
                    f"Duplicate YAML frontmatter key: "
                    f"{self.relative(path)}:{line_number} ({qualified_key})"
                )
                continue
            seen_keys.add(qualified_key)

            value = raw_value.strip()
            if not value:
                if not prefix and key == "metadata":
                    parent_key = key
                    continue
                if not prefix and key == "globs":
                    continue
                self.errors.append(
                    f"Empty YAML frontmatter scalar: "
                    f"{self.relative(path)}:{line_number} ({qualified_key})"
                )
                continue

            if not prefix and key == "metadata":
                self.errors.append(
                    f"Invalid YAML frontmatter structure: "
                    f"{self.relative(path)}:{line_number}"
                )
                continue

            if value.startswith('"'):
                try:
                    decoded = json.loads(value)
                except json.JSONDecodeError:
                    decoded = None
                if not isinstance(decoded, str):
                    self.errors.append(
                        f"Invalid YAML frontmatter scalar: "
                        f"{self.relative(path)}:{line_number} ({qualified_key})"
                    )
            elif value.startswith("'"):
                inner = value[1:-1] if len(value) >= 2 and value.endswith("'") else ""
                quote_runs = re.findall(r"'+", inner)
                if (
                    len(value) < 2
                    or not value.endswith("'")
                    or any(len(run) % 2 for run in quote_runs)
                ):
                    self.errors.append(
                        f"Invalid YAML frontmatter scalar: "
                        f"{self.relative(path)}:{line_number} ({qualified_key})"
                    )
            elif not re.fullmatch(r"[A-Za-z0-9_./-]+", value):
                self.errors.append(
                    f"Invalid YAML frontmatter scalar: "
                    f"{self.relative(path)}:{line_number} ({qualified_key})"
                )

        return seen_keys

    def check_frontmatter_schema(
        self,
        path: Path,
        markdown: MarkdownData,
        expected_keys: set[str],
    ) -> None:
        if markdown.keys != expected_keys:
            self.errors.append(
                f"Invalid YAML frontmatter schema: {self.relative(path)} "
                f"(expected {sorted(expected_keys)}, got {sorted(markdown.keys)})"
            )

    @staticmethod
    def raw_scalar(frontmatter: str, key: str) -> str:
        if "." in key:
            parent, child = key.split(".", 1)
            if not re.search(rf"(?m)^{re.escape(parent)}:[ \t]*$", frontmatter):
                return ""
            pattern = rf"(?m)^  {re.escape(child)}:[ \t]*(.*?)[ \t]*$"
        else:
            pattern = rf"(?m)^{re.escape(key)}:[ \t]*(.*?)[ \t]*$"
        match = re.search(pattern, frontmatter)
        if match is None:
            return ""
        return match.group(1).strip()

    @staticmethod
    def scalar(frontmatter: str, key: str) -> str:
        value = AgentSetupChecker.raw_scalar(frontmatter, key)
        if len(value) >= 2 and value[0] == value[-1] == '"':
            try:
                decoded = json.loads(value)
            except json.JSONDecodeError:
                return value[1:-1]
            return decoded if isinstance(decoded, str) else ""
        if len(value) >= 2 and value[0] == value[-1] == "'":
            return value[1:-1].replace("''", "'")
        return value

    def require_quoted_yaml_string(
        self,
        path: Path,
        markdown: MarkdownData,
        key: str,
    ) -> None:
        value = self.raw_scalar(markdown.frontmatter, key)
        if len(value) < 2 or value[0] != value[-1] or value[0] != '"':
            self.errors.append(
                "YAML frontmatter field must be a quoted YAML string: "
                f"{self.relative(path)} ({key})"
            )

    @staticmethod
    def workflow_event_block(workflow_text: str, event_name: str) -> str:
        event_header = f"  {event_name}:"
        in_event = False
        block: list[str] = []
        for line in workflow_text.splitlines():
            if line == event_header:
                in_event = True
                continue
            if not in_event:
                continue
            if line and not line.startswith("    "):
                break
            block.append(line)
        return "\n".join(block)

    def check_root_context(self) -> int:
        required_root_files = (
            "AGENTS.md",
            "CLAUDE.md",
            ".cursorrules",
            ".github/copilot-instructions.md",
        )
        root_texts: dict[str, str] = {}
        for relative_path in required_root_files:
            path = self.root / relative_path
            text = self.read_text(path)
            root_texts[relative_path] = text
            if path.is_file() and not text.strip():
                self.errors.append(
                    f"Required context file is empty: {relative_path}"
                )

        agents_text = root_texts["AGENTS.md"]

        excluded_directories = {
            ".git",
            ".idea",
            ".vs",
            "RobustToolbox",
            "bin",
            "obj",
        }
        for current_root, directory_names, file_names in os.walk(self.root):
            directory_names[:] = [
                name for name in directory_names if name not in excluded_directories
            ]
            for shadow_name in (".hermes.md", "HERMES.md"):
                if shadow_name not in file_names:
                    continue
                shadow_path = Path(current_root) / shadow_name
                self.errors.append(
                    "Higher-priority Hermes context shadows AGENTS.md: "
                    f"{self.relative(shadow_path)}"
                )

        if agents_text:
            if len(agents_text) > MAX_CONTEXT_CHARS:
                self.errors.append(
                    f"AGENTS.md has {len(agents_text)} characters; Hermes caps context "
                    f"files at {MAX_CONTEXT_CHARS}"
                )

            required_facts = (
                "Fork-owned project folder: `_Scp`.",
                "Fast agent-context check: `python3 .agents/check_agent_setup.py`.",
                "RobustToolbox engine as a Git submodule",
                "SunrisePrivate/",
            )
            for fact in required_facts:
                if fact not in agents_text:
                    self.errors.append(
                        f"AGENTS.md is missing required contract fact: {fact}"
                    )

        root_bridges = {
            "CLAUDE.md": "./AGENTS.md",
            ".cursorrules": "./AGENTS.md",
            ".github/copilot-instructions.md": "../AGENTS.md",
        }
        for bridge_name, expected_reference in root_bridges.items():
            bridge_text = root_texts[bridge_name]
            if not bridge_text:
                continue
            if expected_reference not in bridge_text:
                self.errors.append(
                    f"Root bridge {bridge_name} does not reference {expected_reference}"
                )
            if bridge_name == "CLAUDE.md" and "@./AGENTS.md" not in bridge_text:
                self.errors.append("Claude root bridge does not import AGENTS.md")
            if "authoritative repository context" not in bridge_text:
                self.errors.append(
                    f"Root bridge {bridge_name} does not declare AGENTS.md authoritative"
                )
            if len(bridge_text) > 2_000:
                self.errors.append(
                    f"Root bridge is duplicating too much context: {bridge_name}"
                )

        required_entrypoints = (
            ".agents/rules/ss14-skill-preflight-and-refresh.md",
            ".agents/rules/ss14-testing-guidelines.md",
            ".agents/rules/ss14-codebase-prefix-detection.md",
            ".agents/rules/ss14-interaction-flow.md",
            ".agents/rules/AUTHORING_POLICY.md",
            ".agents/skills/AUTHORING_POLICY.md",
            ".agents/check_agent_setup.py",
            ".agents/tests/test_check_agent_setup.py",
            ".github/workflows/agent-context.yml",
        )
        for entrypoint in required_entrypoints:
            entrypoint_path = self.root / entrypoint
            if not entrypoint_path.is_file():
                self.errors.append(
                    f"Missing canonical context entrypoint: {entrypoint}"
                )
            elif not self.read_text(entrypoint_path).strip():
                self.errors.append(
                    f"Required context file is empty: {entrypoint}"
                )

        workflow_path = self.root / ".github/workflows/agent-context.yml"
        if workflow_path.is_file():
            workflow_text = self.read_text(workflow_path)
            if "python3 .agents/check_agent_setup.py" not in workflow_text:
                self.errors.append(
                    "Agent context workflow does not invoke the agent setup checker"
                )
            required_workflow_fragments = (
                "python3 -m unittest discover",
                "actionlint@",
                "merge_group:",
                "push:",
                "pull_request:",
            )
            for fragment in required_workflow_fragments:
                if fragment not in workflow_text:
                    self.errors.append(
                        "Agent context workflow is missing required coverage: "
                        f"{fragment}"
                    )

            dual_trigger_path_lines = (
                "      - .hermes.md",
                "      - HERMES.md",
                '      - "**/.hermes.md"',
                '      - "**/HERMES.md"',
                "      - .github/instructions/**",
            )
            event_blocks = {
                event_name: self.workflow_event_block(workflow_text, event_name)
                for event_name in ("push", "pull_request")
            }
            for path_line in dual_trigger_path_lines:
                if any(
                    path_line not in event_block
                    for event_block in event_blocks.values()
                ):
                    self.errors.append(
                        "Agent context workflow path must cover both push and "
                        f"pull_request: {path_line.strip()}"
                    )

        return len(agents_text)

    @staticmethod
    def rule_entries(root: Path, bridge_kind: str = "markdown") -> dict[str, Path]:
        if not root.is_dir():
            return {}
        if bridge_kind == "cursor":
            return {
                f"{path.stem}.md": path
                for path in root.glob("*.mdc")
                if path.is_file()
            }
        if bridge_kind == "github":
            suffix = ".instructions.md"
            return {
                f"{path.name.removesuffix(suffix)}.md": path
                for path in root.glob(f"*{suffix}")
                if path.is_file()
            }
        return {path.name: path for path in root.glob("*.md") if path.is_file()}

    def skill_entries(self, root: Path) -> dict[str, Path]:
        if not root.is_dir():
            return {}

        entries: dict[str, Path] = {}
        for path in sorted(root.iterdir(), key=lambda item: item.name):
            if not path.is_dir():
                continue
            if SKILL_NAME_RE.fullmatch(path.name) is None:
                self.errors.append(
                    f"Invalid portable skill directory name: {self.relative(path)}"
                )

            manifest = path / "SKILL.md"
            if not manifest.is_file():
                self.errors.append(
                    f"Skill directory missing SKILL.md: {self.relative(path)}"
                )
                continue
            entries[path.name] = manifest
        return entries

    def compare_entry_names(
        self,
        canonical_names: set[str],
        bridge_names: set[str],
        label: str,
    ) -> None:
        for name in sorted(canonical_names - bridge_names):
            self.errors.append(f"Missing {label} bridge for: {name}")
        for name in sorted(bridge_names - canonical_names):
            self.errors.append(
                f"Orphan {label} bridge without canonical source: {name}"
            )

    def check_reference_resolves(
        self,
        bridge_path: Path,
        reference: str,
        canonical_path: Path,
        label: str,
    ) -> None:
        resolved_reference = (bridge_path.parent / reference).resolve()
        if resolved_reference != canonical_path.resolve():
            self.errors.append(
                f"{label} does not resolve to canonical source: "
                f"{self.relative(bridge_path)} -> {reference}"
            )

    def check_bridge_body_size(self, bridge_path: Path, body: str) -> None:
        if len(body) > MAX_BRIDGE_BODY_CHARS:
            self.errors.append(
                "Bridge duplicates too much canonical content: "
                f"{self.relative(bridge_path)}"
            )

    @staticmethod
    def body_references(body: str) -> set[str]:
        return {
            reference.rstrip(".")
            for reference in re.findall(
                r"(?:\.\./)+\.agents/[A-Za-z0-9_./-]+",
                body,
            )
        }

    def check_canonical_skill_resources(
        self,
        canonical_path: Path,
        body: str,
    ) -> None:
        skill_root = canonical_path.parent.resolve()
        references = {
            reference.rstrip(".,;:")
            for reference in re.findall(
                r"(?:references|templates|scripts|assets)/[A-Za-z0-9_./@+-]+",
                body,
            )
        }
        for reference in sorted(references):
            resolved = (canonical_path.parent / reference).resolve()
            try:
                resolved.relative_to(skill_root)
            except ValueError:
                self.errors.append(
                    "Canonical skill resource escapes its skill directory: "
                    f"{self.relative(canonical_path)} -> {reference}"
                )
                continue
            if not resolved.is_file():
                self.errors.append(
                    "Canonical skill resource does not resolve: "
                    f"{self.relative(canonical_path)} -> {reference}"
                )

        for resource_directory in ("references", "templates", "scripts", "assets"):
            directory = canonical_path.parent / resource_directory
            if not directory.is_dir():
                continue
            resource_paths = sorted(
                path for path in directory.rglob("*") if path.is_file()
            )
            for resource_path in resource_paths:
                relative_resource = resource_path.relative_to(
                    canonical_path.parent
                ).as_posix()
                if relative_resource not in body:
                    self.errors.append(
                        "Canonical skill resource is not referenced: "
                        f"{self.relative(resource_path)}"
                    )

    def check_rule_bridges(self) -> int:
        canonical_root = self.root / ".agents/rules"
        canonical_rules = {
            name: path
            for name, path in self.rule_entries(canonical_root).items()
            if name != "AUTHORING_POLICY.md"
        }
        bridge_specs = {
            "Antigravity": (self.root / ".agent/rules", "markdown"),
            "Claude": (self.root / ".claude/rules", "markdown"),
            "Cursor": (self.root / ".cursor/rules", "cursor"),
            "GitHub Copilot": (
                self.root / ".github/instructions",
                "github",
            ),
        }
        bridges = {
            label: self.rule_entries(root, bridge_kind)
            for label, (root, bridge_kind) in bridge_specs.items()
        }

        for legacy_path in sorted((self.root / ".cursor/rules").glob("*.md")):
            self.errors.append(
                f"Non-native Cursor rule bridge must use .mdc: "
                f"{self.relative(legacy_path)}"
            )
        for legacy_path in sorted((self.root / ".github/rules").glob("*.md")):
            self.errors.append(
                f"Non-native GitHub Copilot rule bridge must use "
                f".github/instructions/*.instructions.md: "
                f"{self.relative(legacy_path)}"
            )

        canonical_names = set(canonical_rules)
        for label, entries in bridges.items():
            self.compare_entry_names(canonical_names, set(entries), label)

        for name, canonical_path in sorted(canonical_rules.items()):
            canonical = self.markdown_data(canonical_path)
            if canonical is None:
                continue

            self.check_frontmatter_schema(
                canonical_path,
                canonical,
                {"trigger"},
            )

            trigger = self.scalar(canonical.frontmatter, "trigger")
            if not trigger:
                self.errors.append(
                    f"Canonical rule has no trigger: {self.relative(canonical_path)}"
                )
            elif trigger != "always_on":
                self.errors.append(
                    "Canonical operational rule must use trigger: always_on: "
                    f"{self.relative(canonical_path)}"
                )

            expected_reference = f"../../.agents/rules/{name}"
            for label, entries in bridges.items():
                bridge_path = entries.get(name)
                if bridge_path is None:
                    continue
                bridge = self.markdown_data(bridge_path)
                if bridge is None:
                    continue
                self.check_bridge_body_size(bridge_path, bridge.body)

                if label == "Antigravity":
                    expected_keys = {
                        "trigger",
                        "metadata",
                        "metadata.source_rule",
                    }
                elif label == "Claude":
                    expected_keys = {"trigger"}
                elif label == "Cursor":
                    expected_keys = {"description", "globs", "alwaysApply"}
                else:
                    expected_keys = {"applyTo"}
                self.check_frontmatter_schema(
                    bridge_path,
                    bridge,
                    expected_keys,
                )

                if label == "Antigravity":
                    self.require_quoted_yaml_string(
                        bridge_path,
                        bridge,
                        "metadata.source_rule",
                    )
                elif label == "Cursor":
                    self.require_quoted_yaml_string(
                        bridge_path,
                        bridge,
                        "description",
                    )
                elif label == "GitHub Copilot":
                    self.require_quoted_yaml_string(
                        bridge_path,
                        bridge,
                        "applyTo",
                    )

                if label in {"Antigravity", "Claude"} and (
                    self.scalar(bridge.frontmatter, "trigger") != trigger
                ):
                    self.errors.append(f"{label} rule trigger mismatch: {name}")
                if label == "Cursor":
                    if self.scalar(bridge.frontmatter, "alwaysApply") != "true":
                        self.errors.append(
                            f"Cursor rule is not always applied: {name}"
                        )
                    if not self.scalar(bridge.frontmatter, "description"):
                        self.errors.append(
                            f"Cursor rule has no discovery description: {name}"
                        )
                if label == "GitHub Copilot" and (
                    self.scalar(bridge.frontmatter, "applyTo") != "**"
                ):
                    self.errors.append(
                        f"GitHub Copilot applyTo mismatch: {name}"
                    )
                body_references = self.body_references(bridge.body)
                if expected_reference not in body_references:
                    self.errors.append(f"{label} rule reference mismatch: {name}")
                if label == "Claude" and f"@{expected_reference}" not in bridge.body:
                    self.errors.append(
                        f"Claude rule bridge does not import canonical source: {name}"
                    )
                for reference in body_references:
                    self.check_reference_resolves(
                        bridge_path,
                        reference,
                        canonical_path,
                        f"{label} rule bridge reference",
                    )
                if label == "Antigravity":
                    source_rule = self.scalar(
                        bridge.frontmatter,
                        "metadata.source_rule",
                    )
                    if source_rule != expected_reference:
                        self.errors.append(f"{label} source_rule mismatch: {name}")
                    if source_rule:
                        self.check_reference_resolves(
                            bridge_path,
                            source_rule,
                            canonical_path,
                            f"{label} source_rule",
                        )

        return len(canonical_rules)

    def check_skill_bridges(self) -> int:
        canonical_skills = self.skill_entries(self.root / ".agents/skills")
        if not canonical_skills:
            self.errors.append("No canonical skills found under .agents/skills")

        bridge_roots = {
            "Antigravity": self.root / ".agent/skills",
            "Claude": self.root / ".claude/skills",
            "Cursor": self.root / ".cursor/skills",
            "GitHub Copilot": self.root / ".github/skills",
        }
        bridges = {
            label: self.skill_entries(root) for label, root in bridge_roots.items()
        }

        canonical_names = set(canonical_skills)
        for label, entries in bridges.items():
            self.compare_entry_names(canonical_names, set(entries), label)

        for directory_name, canonical_path in sorted(canonical_skills.items()):
            canonical = self.markdown_data(canonical_path)
            if canonical is None:
                continue

            self.check_frontmatter_schema(
                canonical_path,
                canonical,
                {"name", "description"},
            )
            self.require_quoted_yaml_string(canonical_path, canonical, "name")
            self.require_quoted_yaml_string(canonical_path, canonical, "description")

            canonical_name = self.scalar(canonical.frontmatter, "name")
            description = self.scalar(canonical.frontmatter, "description")
            if not canonical_name:
                self.errors.append(
                    f"Canonical skill has no name: {directory_name}"
                )
            elif canonical_name != directory_name:
                self.errors.append(
                    "Canonical skill name must match directory: "
                    f"{directory_name} != {canonical_name}"
                )
            if not description:
                self.errors.append(
                    f"Canonical skill has no description: {directory_name}"
                )
            elif not description.startswith(("Use when ", "Use for ")):
                self.errors.append(
                    "Canonical skill description must begin with 'Use when' or "
                    f"'Use for': {directory_name}"
                )
            elif len(description) > 57:
                self.errors.append(
                    "Canonical skill description must be at most 57 characters: "
                    f"{directory_name}"
                )

            self.check_canonical_skill_resources(canonical_path, canonical.body)

            expected_reference = (
                f"../../../.agents/skills/{directory_name}/SKILL.md"
            )
            for label, entries in bridges.items():
                bridge_path = entries.get(directory_name)
                if bridge_path is None:
                    continue
                bridge = self.markdown_data(bridge_path)
                if bridge is None:
                    continue
                self.check_bridge_body_size(bridge_path, bridge.body)

                expected_keys = {"name", "description"}
                if label in {"Antigravity", "GitHub Copilot"}:
                    expected_keys.update({"metadata", "metadata.source_skill"})
                self.check_frontmatter_schema(
                    bridge_path,
                    bridge,
                    expected_keys,
                )
                self.require_quoted_yaml_string(bridge_path, bridge, "name")
                self.require_quoted_yaml_string(bridge_path, bridge, "description")
                if label in {"Antigravity", "GitHub Copilot"}:
                    self.require_quoted_yaml_string(
                        bridge_path,
                        bridge,
                        "metadata.source_skill",
                    )

                if self.scalar(bridge.frontmatter, "name") != directory_name:
                    self.errors.append(
                        f"{label} skill name mismatch: {directory_name}"
                    )
                if self.scalar(bridge.frontmatter, "description") != description:
                    self.errors.append(
                        f"{label} skill description mismatch: {directory_name}"
                    )
                body_references = self.body_references(bridge.body)
                if expected_reference not in body_references:
                    self.errors.append(
                        f"{label} skill reference mismatch: {directory_name}"
                    )
                for reference in body_references:
                    self.check_reference_resolves(
                        bridge_path,
                        reference,
                        canonical_path,
                        f"{label} skill bridge reference",
                    )
                if label in {"Antigravity", "GitHub Copilot"}:
                    source_skill = self.scalar(
                        bridge.frontmatter,
                        "metadata.source_skill",
                    )
                    if source_skill != expected_reference:
                        self.errors.append(
                            f"{label} source_skill mismatch: {directory_name}"
                        )
                    if source_skill:
                        self.check_reference_resolves(
                            bridge_path,
                            source_skill,
                            canonical_path,
                            f"{label} source_skill",
                        )

        return len(canonical_skills)

    def run(self) -> CheckSummary:
        return CheckSummary(
            context_chars=self.check_root_context(),
            rule_count=self.check_rule_bridges(),
            skill_count=self.check_skill_bridges(),
        )


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Validate repository AI context and compatibility bridges."
    )
    parser.add_argument(
        "--repo-root",
        type=Path,
        default=Path(__file__).resolve().parent.parent,
        help="Repository root to validate (defaults to this script's repository).",
    )
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    checker = AgentSetupChecker(args.repo_root)
    summary = checker.run()

    if checker.errors:
        print(
            f"Agent setup check failed with {len(checker.errors)} issue(s):",
            file=sys.stderr,
        )
        for error in checker.errors:
            print(f"- {error}", file=sys.stderr)
        return 1

    print("Agent setup check passed:")
    print(
        f"- authoritative context: AGENTS.md ({summary.context_chars} characters)"
    )
    print("- root compatibility bridges: 3")
    print(f"- canonical rules: {summary.rule_count} (4 bridge trees each)")
    print(f"- canonical skills: {summary.skill_count} (4 bridge trees each)")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
