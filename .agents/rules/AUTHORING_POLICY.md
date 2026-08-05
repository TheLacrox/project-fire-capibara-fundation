# Rule Authoring Policy

## Scope

This repository uses five rule trees:

- `.agents/rules` is the source of truth and stores full rule content.
- `.agent/rules` is the Antigravity compatibility layer.
- `.claude/rules` is the Claude Code compatibility layer.
- `.cursor/rules` contains native Cursor project rules (`.mdc`).
- `.github/instructions` contains native GitHub Copilot path instructions.

## Required Rule

For every operational rule file in `.agents/rules/<rule-name>.md`, keep matching bridge files. `AUTHORING_POLICY.md` is repository-maintenance documentation, not a loadable rule, and is intentionally excluded:

- `.agent/rules/<rule-name>.md`
- `.claude/rules/<rule-name>.md`
- `.cursor/rules/<rule-name>.mdc`
- `.github/instructions/<rule-name>.instructions.md`

When creating, renaming, or deleting a rule, or when changing its `trigger`, update all bridge trees in the same pull request. A body-only canonical change does not require rewriting bridge bodies because they dereference the canonical file, but the unified bridge check must still pass.

## Bridge Contract

Frontmatter intentionally uses a strict, portable YAML subset: only the exact keys documented below, top-level scalars, and the two-space `metadata` mapping are accepted. Do not use aliases, tags, multiline scalars, flow collections, or extra nesting. Quote every string value shown as quoted in this policy.

Bridge bodies must remain at most 500 characters and contain only the native import/reference plus minimal compatibility guidance; canonical rule content belongs only in `.agents/rules`.

Each Antigravity bridge rule file must contain:

- `trigger`: synchronized copy of canonical trigger from `.agents/rules/<rule-name>.md`.
- `metadata.source_rule`: `../../.agents/rules/<rule-name>.md`.
- A reference in the markdown body to `../../.agents/rules/<rule-name>.md`.

Each Claude bridge rule file must contain:

- `trigger`: synchronized copy of canonical trigger from `.agents/rules/<rule-name>.md`.
- A native Claude import in the markdown body: `@../../.agents/rules/<rule-name>.md`.

Each Cursor bridge rule file must contain:

- `description`: a concise discovery description.
- `globs`: empty, because the rule is repository-wide.
- `alwaysApply`: `true`.
- A reference in the markdown body to `../../.agents/rules/<rule-name>.md`.

Each GitHub Copilot bridge rule file must contain:

- `applyTo`: `"**"`.
- A reference in the markdown body to `../../.agents/rules/<rule-name>.md`.

## PR Checklist Gate

A PR is incomplete if any rule exists in `.agents/rules` without matching bridges in
`.agent/rules`, `.claude/rules`, `.cursor/rules`, and `.github/instructions`.

Run the unified agent setup check before pushing:

`python3 ./.agents/check_agent_setup.py`

For a focused rule-only diagnostic, `pwsh ./.agents/rules/check-rule-bridges.ps1` remains available.
