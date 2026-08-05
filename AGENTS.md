# Repository Agent Instructions

## Project identity

This is the Capibara Foundation fork of the Russian SCP: Project Fire fork of Space Station 14. The inherited Fire layout remains authoritative until maintainers explicitly approve a migration:

- Codebase/edit prefix: `Fire`.
- Fork-owned project folder: `_Scp`.
- Minimal edits to inherited files use the nearest existing `Fire edit` / `Fire added` marker style.
- Do not invent a `Capibara` code prefix or move existing `_Scp` content merely because the GitHub origin changed.

The code targets C# 14 on .NET 10 and uses the RobustToolbox engine as a Git submodule.

## Context strategy

`AGENTS.md` is the portable startup contract for Hermes, Codex/GPT, Claude Code, Cursor, Copilot, and other coding agents. Start Hermes from the repository root because portable `AGENTS.md` startup discovery does not walk parent directories. Do not add `.hermes.md` or `HERMES.md`: Hermes gives those files higher priority and they would shadow this contract.

Detailed sources of truth:

- Rules: `.agents/rules`.
- Skills: `.agents/skills`.
- Compatibility bridges only: `.agent`, `.claude`, `.cursor`, `.github/instructions`, `.github/skills`, and `.github/copilot-instructions.md`. Other `.github` content, such as workflows, remains ordinary repository configuration.
- Rule authoring policy: `.agents/rules/AUTHORING_POLICY.md`.
- Skill authoring policy: `.agents/skills/AUTHORING_POLICY.md`.

At the start of a new dialogue, after context compaction, or when the task changes subsystem/file type:

1. Read every rule in `.agents/rules` whose frontmatter says `trigger: always_on`.
2. Review the skill names/descriptions in `.agents/skills`.
3. Load only the skills required by the touched extensions and concrete subsystem, following `.agents/rules/ss14-skill-preflight-and-refresh.md`.
4. Treat loaded rules and skills as mandatory constraints. If one is stale, update its canonical source and keep its bridges synchronized.

Hermes does not index repository-local skills unless the active profile opts into `.agents/skills` through `skills.external_dirs`. When no `ss14-*` entries appear in the Hermes skill index, read the selected canonical `SKILL.md` files directly; do not copy them into a second source or silently alter the user's global profile configuration.

Fast agent-context check: `python3 .agents/check_agent_setup.py`. It requires Python 3.10 or newer; on native Windows without a `python3` command, use `py -3 .agents/check_agent_setup.py`.

## First checkout and architecture landmarks

Initialize engine dependencies before restore/build:

```bash
git submodule update --init --recursive
dotnet restore SpaceStation14.slnx
```

Work in the narrowest owning area:

- `Content.Shared`: networked components, shared systems, events, prediction-facing behavior.
- `Content.Server`: authoritative server-only systems and database integration.
- `Content.Client`: rendering, UI, overlays, and client-only presentation.
- `Resources/Prototypes/_Scp`: Fire-owned prototypes; corresponding code lives under the nearest `Content.*/_Scp` tree.
- `Resources/Locale`: Fluent localization; preserve locale and directory conventions.
- `Content.Tests` and `Content.IntegrationTests`: unit/content tests and engine-backed integration tests.
- `RobustToolbox`: pinned engine submodule. Do not edit or advance it unless the task explicitly targets the engine revision.

Inherited `_Sunrise` and `_Starlight` trees coexist with `_Scp`. Preserve them in place, but do not use their names to infer the active fork or as the destination for new Capibara/Fire-owned work.

`Content.Client`, `Content.Server`, `Content.Shared`, and `Content.Tests` conditionally compile files from the optional private directory `SunrisePrivate/` at the repository root (`../SunrisePrivate` relative to each `Content.*` project). Never create, inspect, or modify that private tree unless the user explicitly provides access and asks for it. State whether it was absent when that affects a result.

## Change discipline

- Gather context before editing: trace definitions and usages, inspect neighboring Fire code, and read the owning project/workflow.
- Put new Fire-owned gameplay code/assets in `_Scp`; keep inherited/vanilla hooks minimal. Follow `ss14-codebase-prefix-detection` and `ss14-upstream-maintenance`.
- Prefer Rider MCP for navigation, diagnostics, refactors, and formatting when its allowed tools are available. Run Rider file diagnostics on every changed code/config file and address its findings unless a suggestion contradicts an intentional public contract. Do not use Rider's terminal/run-configuration/project-module/project-dependency commands. If Rider MCP is unavailable, use repository file tools and the shell only where needed.
- Do not edit generated output or dependency trees. Never blanket-reset the repository or overwrite unrelated user changes.
- Do not read or print secrets. Do not commit, stage, push, merge, or update submodule pointers unless explicitly asked.
- This is a one-way downstream fork. Treat `space-sunrise/project-fire` and any `upstream` remote as read-only: fetch and merge changes from upstream, but never push to it or open a pull request whose base repository is upstream. Repository pull requests may target only this fork's `origin`; verify the base repository before creating one.
- For a large investigation where repository edits are allowed, keep a temporary task-notes file and delete it before completion. For read-only work, use the session task list or scratch space outside the repository.

## Generated and mutating boundaries

- `bin/`, `obj/`, `artifacts/`, `.integration-filters/`, `Resources/MapImages/`, and DocFX output are generated and ignored; never treat them as source deliverables.
- A full solution build in `Debug` runs `BuildChecker`, which can replace `.git/hooks` and update submodules. Prefer narrow project builds and do not use the full Debug build as a read-only check without approval.
- EF Core migration designer files and model snapshots under `Content.Server.Database/Migrations` are generator-owned. Use the migration workflow/skill rather than hand-editing generated companions.
- `Content.MigrationHideSpawnMenu` can rewrite tracked files under `Resources/Prototypes` from `Resources/migration.yml`. Prefer non-mutating `check` mode; use `sync` only when the task requires it, then inspect every generated diff.
- `Content.Packaging` wipes `release/` by default and may replace `bin/`; run it only when packaging behavior is in scope and account for every generated artifact afterward.
- Some GitHub workflows generate or push locale/prototype/credit data. Never reproduce those side effects locally unless the requested task requires generated updates.

## Verification contract

Read `.agents/rules/ss14-testing-guidelines.md` and run the narrowest command that proves the final tree:

- Agent-infrastructure-only changes: `python3 .agents/check_agent_setup.py`.
- C# changes: build the owning `.csproj` in `Debug`; use `Release --no-restore /m` for CI-strength verification.
- SS14 resource YAML/FTL/prototype changes: build and run `Content.YAMLLinter` in `Release`; use the owning validator for CI/tool YAML.
- Client behavior: launch `Content.Client` (or `Tools`) long enough to check startup/runtime/IL errors, then stop it.
- Focused tests first; build the same test project in `DebugOpt` before using `Content.Tests` or `Content.IntegrationTests` with `DebugOpt --no-build`.
- Packaging changes: restore and build `Content.Packaging` in `Release`, then exercise the CLI used by `.github/workflows/test-packaging.yml`.

Run at most two test commands concurrently; prefer one. Stop every process started for runtime verification. Before finishing, run the relevant checks again on the final source state, `git diff --check`, and account for every working-tree change.

## Language and terminology

- Communicate with maintainers in the language of their request.
- Keep identifiers, type/API names, loc keys, canonical marker tokens, and C# XML documentation in English.
- Write explanatory inline comments, `using` comments, and edit-marker reason phrases in Russian, matching the inherited repository convention.
- In prose near `Sponsor*` code, use **«спонсор»**. Do not introduce `donor` or `donator` in new comments, marker reasons, or explanatory prose.
