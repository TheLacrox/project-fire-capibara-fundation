# Skill Authoring Policy

## Scope

This repository uses five skill trees:

- `.agents/skills` is the source of truth and stores full skill content and resources.
- `.agent/skills` is the Antigravity compatibility layer.
- `.claude/skills` is the Claude Code compatibility layer.
- `.cursor/skills` is the Cursor compatibility layer.
- `.github/skills` is the GitHub Copilot compatibility layer.

## Required Rule

For every skill in `.agents/skills/<skill-name>`, keep matching bridge files:

- `.agent/skills/<skill-name>/SKILL.md`
- `.claude/skills/<skill-name>/SKILL.md`
- `.cursor/skills/<skill-name>/SKILL.md`
- `.github/skills/<skill-name>/SKILL.md`

When creating, renaming, or deleting a skill, or when changing its `name` or `description`, update `.agent/skills`, `.claude/skills`, `.cursor/skills`, and `.github/skills` in the same pull request. A body/resource-only canonical change does not require rewriting bridge bodies because they dereference the canonical directory, but the unified bridge check must still pass.

## Canonical Frontmatter Contract

Every canonical skill must begin with this YAML shape:

```yaml
---
name: "<skill-name>"
description: "Use when <short, discriminating trigger>."
---
```

`name` must exactly match the containing directory. Keep `name`, `description`, and bridge path values double-quoted so every consumer parses valid YAML even when descriptions contain colons or other punctuation. Begin each description with `Use when` or `Use for`, keep the selection trigger complete within the first 57 characters, and move explanatory detail into the skill body because some agent indexes truncate descriptions.

Frontmatter intentionally uses a strict, portable YAML subset: only the exact keys documented here, top-level scalars, and the two-space `metadata` mapping are accepted. Do not use aliases, tags, multiline scalars, flow collections, or extra nesting.

Every file under a skill's `references/`, `templates/`, `scripts/`, or `assets/` directory must be referenced by its canonical `SKILL.md`, and every declared local resource path must resolve inside that skill directory. State a useful reading order when a skill has several supporting files.

## Bridge Contract

Bridge bodies must remain at most 500 characters and contain only the canonical reference plus minimal compatibility guidance; skill procedures and resources belong only in `.agents/skills`.

Each Antigravity bridge SKILL file must contain:

- `name`: exact `<skill-name>` folder name in hyphen-case.
- `description`: synchronized copy of canonical description from `.agents/skills/<skill-name>/SKILL.md`.
- `metadata.source_skill`: `../../../.agents/skills/<skill-name>/SKILL.md`.
- A reference in the markdown body to `../../../.agents/skills/<skill-name>/SKILL.md`.

Each Claude bridge SKILL file must contain:

- `name`: exact `<skill-name>` folder name in hyphen-case.
- `description`: synchronized copy of canonical description from `.agents/skills/<skill-name>/SKILL.md`.
- A reference in the markdown body to `../../../.agents/skills/<skill-name>/SKILL.md`.

Each Cursor bridge SKILL file must contain:

- `name`: exact `<skill-name>` folder name in hyphen-case.
- `description`: synchronized copy of canonical description from `.agents/skills/<skill-name>/SKILL.md`.
- A reference in the markdown body to `../../../.agents/skills/<skill-name>/SKILL.md`.

Each GitHub Copilot bridge SKILL file must contain:

- `name`: exact `<skill-name>` folder name in hyphen-case.
- `description`: synchronized copy of canonical description from `.agents/skills/<skill-name>/SKILL.md`.
- `metadata.source_skill`: `../../../.agents/skills/<skill-name>/SKILL.md`.
- A reference in the markdown body to `../../../.agents/skills/<skill-name>/SKILL.md`.

Bridge `name`, `description`, and metadata path values follow the same double-quoted scalar rule as canonical frontmatter.

## PR Checklist Gate

A PR is incomplete if any skill exists in `.agents/skills` without matching bridges in
`.agent/skills`, `.claude/skills`, `.cursor/skills`, and `.github/skills`.

Run the unified agent setup check before pushing:

`python3 ./.agents/check_agent_setup.py`

For a focused skill-only diagnostic, `pwsh ./.agents/skills/check-skill-bridges.ps1` remains available.
