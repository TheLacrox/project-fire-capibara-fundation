from __future__ import annotations

import shutil
import subprocess
import sys
import tempfile
import unittest
from pathlib import Path


REPO_ROOT = Path(__file__).resolve().parents[2]
CHECKER = REPO_ROOT / ".agents/check_agent_setup.py"
RULE_NAMES = (
    "ss14-skill-preflight-and-refresh.md",
    "ss14-testing-guidelines.md",
    "ss14-codebase-prefix-detection.md",
    "ss14-interaction-flow.md",
)


def write(root: Path, relative_path: str, content: str) -> None:
    path = root / relative_path
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(content, encoding="utf-8")


def create_valid_fixture(root: Path) -> None:
    write(
        root,
        "AGENTS.md",
        "\n".join(
            (
                "# Test contract",
                "Fork-owned project folder: `_Scp`.",
                "Fast agent-context check: `python3 .agents/check_agent_setup.py`.",
                "RobustToolbox engine as a Git submodule",
                "Optional boundary: SunrisePrivate/",
            )
        ),
    )
    write(
        root,
        "CLAUDE.md",
        "@./AGENTS.md\n\nAGENTS.md is the authoritative repository context.\n",
    )
    write(
        root,
        ".cursorrules",
        "./AGENTS.md is the authoritative repository context.\n",
    )
    write(
        root,
        ".github/copilot-instructions.md",
        "../AGENTS.md is the authoritative repository context.\n",
    )
    write(root, ".agents/rules/AUTHORING_POLICY.md", "# Policy\n")
    write(root, ".agents/skills/AUTHORING_POLICY.md", "# Policy\n")
    write(root, ".agents/check_agent_setup.py", "# Checker\n")
    write(root, ".agents/tests/test_check_agent_setup.py", "# Tests\n")
    write(
        root,
        ".github/workflows/agent-context.yml",
        'name: Test\non:\n  merge_group:\n  push:\n    paths:\n'
        "      - .hermes.md\n      - HERMES.md\n"
        '      - "**/.hermes.md"\n      - "**/HERMES.md"\n'
        "      - .github/instructions/**\n"
        "  pull_request:\n    paths:\n"
        "      - .hermes.md\n      - HERMES.md\n"
        '      - "**/.hermes.md"\n      - "**/HERMES.md"\n'
        "      - .github/instructions/**\n"
        "jobs:\n  validate:\n    steps:\n"
        "      - run: go run example/actionlint@v1\n"
        "      - run: python3 -m unittest discover\n"
        "      - run: python3 .agents/check_agent_setup.py\n",
    )

    for rule_name in RULE_NAMES:
        write(
            root,
            f".agents/rules/{rule_name}",
            "---\ntrigger: always_on\n---\n\n# Canonical rule\n",
        )
        reference = f"../../.agents/rules/{rule_name}"
        write(
            root,
            f".agent/rules/{rule_name}",
            "---\ntrigger: always_on\nmetadata:\n"
            f'  source_rule: "{reference}"\n---\n\n'
            f"# Antigravity bridge\n\n{reference}\n",
        )
        write(
            root,
            f".claude/rules/{rule_name}",
            "---\ntrigger: always_on\n---\n\n"
            f"# Claude bridge\n\n@{reference}\n",
        )
        cursor_name = rule_name.removesuffix(".md") + ".mdc"
        write(
            root,
            f".cursor/rules/{cursor_name}",
            '---\ndescription: "Always-on SS14 rule bridge."\n'
            "globs:\nalwaysApply: true\n---\n\n"
            f"# Cursor bridge\n\n{reference}\n",
        )
        github_name = rule_name.removesuffix(".md") + ".instructions.md"
        write(
            root,
            f".github/instructions/{github_name}",
            '---\napplyTo: "**"\n---\n\n'
            f"# GitHub Copilot bridge\n\n{reference}\n",
        )

    description = "Use for validating the test fixture."
    write(
        root,
        ".agents/skills/demo/SKILL.md",
        f'---\nname: "demo"\ndescription: "{description}"\n---\n\n# Demo\n',
    )
    skill_reference = "../../../.agents/skills/demo/SKILL.md"
    for bridge_root, title, needs_metadata in (
        (".agent/skills", "Antigravity", True),
        (".claude/skills", "Claude", False),
        (".cursor/skills", "Cursor", False),
        (".github/skills", "GitHub Copilot", True),
    ):
        metadata = (
            f'metadata:\n  source_skill: "{skill_reference}"\n'
            if needs_metadata
            else ""
        )
        write(
            root,
            f"{bridge_root}/demo/SKILL.md",
            f'---\nname: "demo"\ndescription: "{description}"\n{metadata}---\n\n'
            f"# {title} bridge\n\n{skill_reference}\n",
        )


def run_checker(root: Path) -> subprocess.CompletedProcess[str]:
    return subprocess.run(
        [sys.executable, str(CHECKER), "--repo-root", str(root)],
        cwd=REPO_ROOT,
        capture_output=True,
        text=True,
        check=False,
    )


class AgentSetupCheckerTests(unittest.TestCase):
    def test_valid_fixture_passes(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)

            result = run_checker(root)

            self.assertEqual(0, result.returncode, result.stderr)
            self.assertIn("Agent setup check passed", result.stdout)

    def test_empty_required_context_files_fail(self) -> None:
        for relative_path in (
            "AGENTS.md",
            "CLAUDE.md",
            ".cursorrules",
            ".github/copilot-instructions.md",
            ".github/workflows/agent-context.yml",
        ):
            with self.subTest(relative_path=relative_path):
                with tempfile.TemporaryDirectory() as directory:
                    root = Path(directory)
                    create_valid_fixture(root)
                    (root / relative_path).write_text("", encoding="utf-8")

                    result = run_checker(root)

                    self.assertEqual(1, result.returncode)
                    self.assertIn("Required context file is empty", result.stderr)

    def test_higher_priority_hermes_context_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            write(root, ".hermes.md", "# Shadow\n")

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("shadows AGENTS.md", result.stderr)

    def test_nested_higher_priority_hermes_context_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            write(root, "Content.Shared/.hermes.md", "# Nested shadow\n")

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("Content.Shared/.hermes.md", result.stderr)

    def test_missing_skill_bridge_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            (root / ".cursor/skills/demo/SKILL.md").unlink()

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("Missing Cursor bridge for: demo", result.stderr)

    def test_bridge_body_must_stay_minimal(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            bridge_path = root / ".claude/skills/demo/SKILL.md"
            bridge_path.write_text(
                bridge_path.read_text(encoding="utf-8") + ("duplicate " * 80),
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("duplicates too much canonical content", result.stderr)

    def test_skill_directory_without_manifest_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            write(root, ".agents/skills/incomplete/README.md", "# Incomplete\n")

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("Skill directory missing SKILL.md", result.stderr)

    def test_unreferenced_canonical_skill_resource_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            write(
                root,
                ".agents/skills/demo/references/guide.md",
                "# Guide\n",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("Canonical skill resource is not referenced", result.stderr)

    def test_broken_canonical_skill_resource_reference_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            skill_path = root / ".agents/skills/demo/SKILL.md"
            skill_path.write_text(
                skill_path.read_text(encoding="utf-8")
                + "\n[Missing](references/missing.md)\n",
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("Canonical skill resource does not resolve", result.stderr)

    def test_invalid_skill_yaml_frontmatter_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            for relative_path in (
                ".agents/skills/demo/SKILL.md",
                ".agent/skills/demo/SKILL.md",
                ".claude/skills/demo/SKILL.md",
                ".cursor/skills/demo/SKILL.md",
                ".github/skills/demo/SKILL.md",
            ):
                skill_path = root / relative_path
                skill_text = skill_path.read_text(encoding="utf-8")
                skill_path.write_text(
                    skill_text.replace(
                        'description: "Use for validating the test fixture."',
                        "description: Use when: frontmatter is invalid.",
                    ),
                    encoding="utf-8",
                )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("Invalid YAML frontmatter scalar", result.stderr)

    def test_invalid_single_quoted_yaml_scalar_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            for relative_path in (
                ".agents/skills/demo/SKILL.md",
                ".agent/skills/demo/SKILL.md",
                ".claude/skills/demo/SKILL.md",
                ".cursor/skills/demo/SKILL.md",
                ".github/skills/demo/SKILL.md",
            ):
                skill_path = root / relative_path
                skill_text = skill_path.read_text(encoding="utf-8")
                skill_path.write_text(
                    skill_text.replace(
                        'description: "Use for validating the test fixture."',
                        "description: 'Use for Bob's fixture.'",
                    ),
                    encoding="utf-8",
                )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("Invalid YAML frontmatter scalar", result.stderr)

    def test_skill_string_contract_requires_double_quotes(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            for relative_path in (
                ".agents/skills/demo/SKILL.md",
                ".agent/skills/demo/SKILL.md",
                ".claude/skills/demo/SKILL.md",
                ".cursor/skills/demo/SKILL.md",
                ".github/skills/demo/SKILL.md",
            ):
                skill_path = root / relative_path
                skill_text = skill_path.read_text(encoding="utf-8")
                skill_path.write_text(
                    skill_text.replace(
                        'description: "Use for validating the test fixture."',
                        "description: 'Use for validating the test fixture.'",
                    ),
                    encoding="utf-8",
                )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("must be a quoted YAML string", result.stderr)

    def test_skill_description_with_boolean_type_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            for relative_path in (
                ".agents/skills/demo/SKILL.md",
                ".agent/skills/demo/SKILL.md",
                ".claude/skills/demo/SKILL.md",
                ".cursor/skills/demo/SKILL.md",
                ".github/skills/demo/SKILL.md",
            ):
                skill_path = root / relative_path
                skill_text = skill_path.read_text(encoding="utf-8")
                skill_path.write_text(
                    skill_text.replace(
                        'description: "Use for validating the test fixture."',
                        "description: true",
                    ),
                    encoding="utf-8",
                )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("must be a quoted YAML string", result.stderr)

    def test_non_string_skill_description_types_fail(self) -> None:
        for invalid_value in ("123", "null", "[one, two]", "{kind: demo}"):
            with self.subTest(invalid_value=invalid_value):
                with tempfile.TemporaryDirectory() as directory:
                    root = Path(directory)
                    create_valid_fixture(root)
                    for relative_path in (
                        ".agents/skills/demo/SKILL.md",
                        ".agent/skills/demo/SKILL.md",
                        ".claude/skills/demo/SKILL.md",
                        ".cursor/skills/demo/SKILL.md",
                        ".github/skills/demo/SKILL.md",
                    ):
                        skill_path = root / relative_path
                        skill_text = skill_path.read_text(encoding="utf-8")
                        skill_path.write_text(
                            skill_text.replace(
                                'description: "Use for validating the test fixture."',
                                f"description: {invalid_value}",
                            ),
                            encoding="utf-8",
                        )

                    result = run_checker(root)

                    self.assertEqual(1, result.returncode)
                    self.assertIn("must be a quoted YAML string", result.stderr)

    def test_skill_description_must_start_with_a_short_trigger(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            replacement = (
                'description: "An explanatory description whose useful trigger '
                'appears too late for skill discovery."'
            )
            for relative_path in (
                ".agents/skills/demo/SKILL.md",
                ".agent/skills/demo/SKILL.md",
                ".claude/skills/demo/SKILL.md",
                ".cursor/skills/demo/SKILL.md",
                ".github/skills/demo/SKILL.md",
            ):
                skill_path = root / relative_path
                skill_text = skill_path.read_text(encoding="utf-8")
                skill_path.write_text(
                    skill_text.replace(
                        'description: "Use for validating the test fixture."',
                        replacement,
                    ),
                    encoding="utf-8",
                )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("must begin with 'Use when' or 'Use for'", result.stderr)

    def test_skill_description_must_fit_discovery_index(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            replacement = (
                'description: "Use when validating a deliberately overlong skill '
                'description that would be truncated."'
            )
            for relative_path in (
                ".agents/skills/demo/SKILL.md",
                ".agent/skills/demo/SKILL.md",
                ".claude/skills/demo/SKILL.md",
                ".cursor/skills/demo/SKILL.md",
                ".github/skills/demo/SKILL.md",
            ):
                skill_path = root / relative_path
                skill_text = skill_path.read_text(encoding="utf-8")
                skill_path.write_text(
                    skill_text.replace(
                        'description: "Use for validating the test fixture."',
                        replacement,
                    ),
                    encoding="utf-8",
                )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("must be at most 57 characters", result.stderr)

    def test_skill_metadata_at_wrong_nesting_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            bridge_path = root / ".agent/skills/demo/SKILL.md"
            bridge_text = bridge_path.read_text(encoding="utf-8")
            bridge_path.write_text(
                bridge_text.replace(
                    'metadata:\n  source_skill: "',
                    'source_skill: "',
                ),
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("frontmatter schema", result.stderr)

    def test_canonical_skill_name_must_match_directory(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            skill_path = root / ".agents/skills/demo/SKILL.md"
            skill_text = skill_path.read_text(encoding="utf-8")
            skill_path.write_text(
                skill_text.replace('name: "demo"', 'name: "Demo Skill"'),
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("Canonical skill name must match directory", result.stderr)

    def test_rule_bridge_reference_must_resolve_to_canonical_source(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            bridge_path = root / ".claude/rules/ss14-testing-guidelines.md"
            bridge_text = bridge_path.read_text(encoding="utf-8")
            bridge_path.write_text(
                bridge_text.replace(
                    "../../.agents/rules/ss14-testing-guidelines.md",
                    "../../../.agents/rules/ss14-testing-guidelines.md",
                ),
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("does not resolve to canonical source", result.stderr)

    def test_operational_rules_must_remain_always_on(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            rule_name = "ss14-testing-guidelines.md"
            for relative_path in (
                f".agents/rules/{rule_name}",
                f".agent/rules/{rule_name}",
                f".claude/rules/{rule_name}",
            ):
                rule_path = root / relative_path
                rule_path.write_text(
                    rule_path.read_text(encoding="utf-8").replace(
                        "trigger: always_on",
                        "trigger: manual",
                    ),
                    encoding="utf-8",
                )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("must use trigger: always_on", result.stderr)

    def test_skill_bridge_reference_must_resolve_to_canonical_source(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            bridge_path = root / ".cursor/skills/demo/SKILL.md"
            bridge_text = bridge_path.read_text(encoding="utf-8")
            bridge_path.write_text(
                bridge_text.replace(
                    "../../../.agents/skills/demo/SKILL.md",
                    "../../../../.agents/skills/demo/SKILL.md",
                ),
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("does not resolve to canonical source", result.stderr)

    def test_claude_rule_bridge_must_import_canonical_source(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            bridge_path = root / ".claude/rules/ss14-testing-guidelines.md"
            bridge_text = bridge_path.read_text(encoding="utf-8")
            bridge_path.write_text(
                bridge_text.replace(
                    "@../../.agents/rules/ss14-testing-guidelines.md",
                    "../../.agents/rules/ss14-testing-guidelines.md",
                ),
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("does not import canonical source", result.stderr)

    def test_claude_root_bridge_must_import_agents(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            bridge_path = root / "CLAUDE.md"
            bridge_text = bridge_path.read_text(encoding="utf-8")
            bridge_path.write_text(
                bridge_text.replace("@./AGENTS.md", "./AGENTS.md"),
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("does not import AGENTS.md", result.stderr)

    def test_nonnative_cursor_and_github_rule_bridges_fail(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)

            cursor_native = root / ".cursor/rules/ss14-testing-guidelines.mdc"
            cursor_legacy = root / ".cursor/rules/ss14-testing-guidelines.md"
            if cursor_native.exists():
                cursor_native.rename(cursor_legacy)

            github_native = (
                root
                / ".github/instructions/ss14-testing-guidelines.instructions.md"
            )
            github_legacy = root / ".github/rules/ss14-testing-guidelines.md"
            if github_native.exists():
                github_legacy.parent.mkdir(parents=True, exist_ok=True)
                github_native.rename(github_legacy)

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("Missing Cursor bridge", result.stderr)
            self.assertIn("Missing GitHub Copilot bridge", result.stderr)

    def test_missing_agent_context_workflow_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            (root / ".github/workflows/agent-context.yml").unlink()

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn(
                "Missing canonical context entrypoint: "
                ".github/workflows/agent-context.yml",
                result.stderr,
            )

    def test_agent_context_workflow_must_invoke_checker(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            workflow_path = root / ".github/workflows/agent-context.yml"
            workflow_text = workflow_path.read_text(encoding="utf-8")
            workflow_path.write_text(
                workflow_text.replace(
                    "python3 .agents/check_agent_setup.py",
                    "echo checker-skipped",
                ),
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("does not invoke the agent setup checker", result.stderr)

    def test_workflow_shadow_paths_cover_push_and_pull_request(self) -> None:
        for fragment in (
            "      - .hermes.md\n",
            "      - HERMES.md\n",
            '      - "**/.hermes.md"\n',
            '      - "**/HERMES.md"\n',
            "      - .github/instructions/**\n",
        ):
            with self.subTest(fragment=fragment.strip()):
                with tempfile.TemporaryDirectory() as directory:
                    root = Path(directory)
                    create_valid_fixture(root)
                    workflow_path = root / ".github/workflows/agent-context.yml"
                    workflow_text = workflow_path.read_text(encoding="utf-8")
                    workflow_path.write_text(
                        workflow_text.replace(fragment, "", 1),
                        encoding="utf-8",
                    )

                    result = run_checker(root)

                    self.assertEqual(1, result.returncode)
                    self.assertIn(
                        "must cover both push and pull_request",
                        result.stderr,
                    )

    def test_workflow_shadow_path_cannot_be_duplicated_in_one_event(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            workflow_path = root / ".github/workflows/agent-context.yml"
            workflow_text = workflow_path.read_text(encoding="utf-8")
            push_block, pull_request_block = workflow_text.split(
                "  pull_request:\n",
                1,
            )
            push_block = push_block.replace(
                "      - .hermes.md\n",
                "      - .hermes.md\n      - .hermes.md\n",
                1,
            )
            pull_request_block = pull_request_block.replace(
                "      - .hermes.md\n",
                "",
                1,
            )
            workflow_path.write_text(
                push_block + "  pull_request:\n" + pull_request_block,
                encoding="utf-8",
            )

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn(
                "must cover both push and pull_request",
                result.stderr,
            )

    def test_no_canonical_skills_fails(self) -> None:
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            create_valid_fixture(root)
            for relative_path in (
                ".agents/skills/demo",
                ".agent/skills/demo",
                ".claude/skills/demo",
                ".cursor/skills/demo",
                ".github/skills/demo",
            ):
                shutil.rmtree(root / relative_path)

            result = run_checker(root)

            self.assertEqual(1, result.returncode)
            self.assertIn("No canonical skills found", result.stderr)


if __name__ == "__main__":
    unittest.main()
