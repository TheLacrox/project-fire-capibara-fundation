---
trigger: always_on
---

# Rule: Testing and verification

Run verification at the end of code changes. Choose the narrowest command that proves the changed area is healthy.

## 1. SS14 content YAML, FTL, and prototypes

If the change touches SS14 prototypes, resource YAML, or FTL files loaded by the game, run the YAML linter:

```powershell
dotnet run --project Content.YAMLLinter/Content.YAMLLinter.csproj --configuration Release --no-build
```

If the linter project has not been built yet, build it first:

```powershell
dotnet build Content.YAMLLinter/Content.YAMLLinter.csproj --configuration Release --no-restore /p:WarningsAsErrors= /m
```

If restore has not been completed for this checkout, run `dotnet restore Content.YAMLLinter/Content.YAMLLinter.csproj` before the build. Keep the build and `--no-build` run in the same `Release` configuration; mixing a Release build with a default Debug run can execute stale output or fail to find the binary.

For non-content YAML such as GitHub Actions or tool configuration, use the owning schema/linter instead. Do not run `Content.YAMLLinter` solely for those files; it validates game resources, not arbitrary repository YAML.

## 2. C# changes

If the change touches C# code, build the changed project:

```powershell
dotnet build <relative/path/to/project.csproj> --configuration Debug
```

For stricter CI-style verification, use:

```powershell
dotnet build <relative/path/to/project.csproj> --configuration Release --no-restore /m
```

Run `dotnet restore <relative/path/to/project.csproj>` first when that exact project graph has not already been restored in the checkout. Do not infer restore coverage from an unrelated project.

## 3. Client changes

If the change touches client behavior, run the client to check runtime errors and IL verification:

```powershell
dotnet run --project Content.Client/Content.Client.csproj
```

or:

```powershell
dotnet run --project Content.Client/Content.Client.csproj --configuration Tools
```

Stop the client process before finishing the task.

## 4. Test commands

Use these commands by scope. On a clean checkout, restore the same solution/project first. Every `--no-build` test command below must follow the matching `DebugOpt` build; do not assume a `Debug` or `Release` build produced reusable test artifacts.

1. All solution tests. This is broader than the Fire PR gate because the solution also contains RobustToolbox tests; do not describe it as an exact reproduction of `.github/workflows/build-test-debug.yml`:

   ```powershell
   dotnet restore SpaceStation14.slnx
   dotnet build SpaceStation14.slnx --configuration DebugOpt --no-restore /m
   dotnet test SpaceStation14.slnx --configuration DebugOpt --no-build
   ```

2. Specific test project:

   ```powershell
   dotnet restore Content.Tests/Content.Tests.csproj
   dotnet build Content.Tests/Content.Tests.csproj --configuration DebugOpt --no-restore /m
   dotnet test Content.Tests/Content.Tests.csproj --configuration DebugOpt --no-build
   ```

   ```powershell
   dotnet restore Content.IntegrationTests/Content.IntegrationTests.csproj
   dotnet build Content.IntegrationTests/Content.IntegrationTests.csproj --configuration DebugOpt --no-restore /m
   dotnet test Content.IntegrationTests/Content.IntegrationTests.csproj --configuration DebugOpt --no-build
   ```

3. Specific test:

   ```powershell
   dotnet restore Content.IntegrationTests/Content.IntegrationTests.csproj
   dotnet build Content.IntegrationTests/Content.IntegrationTests.csproj --configuration DebugOpt --no-restore /m
   dotnet test Content.IntegrationTests/Content.IntegrationTests.csproj --configuration DebugOpt --no-build --filter "FullyQualifiedName~GravityGridTest"
   ```

4. Packaging artifacts when needed. Use the same packaging CLI as CI. Packaging is mutating: by default it wipes `release/`, and its internal game build can replace `bin/`; inspect these generated outputs and never treat this as a read-only check:

   ```powershell
   dotnet restore Content.Packaging/Content.Packaging.csproj
   dotnet build Content.Packaging/Content.Packaging.csproj --configuration Release --no-restore /m
   ```

   ```powershell
   dotnet run --project Content.Packaging/Content.Packaging.csproj --configuration Release --no-build -- client
   ```

   ```powershell
   dotnet run --project Content.Packaging/Content.Packaging.csproj --configuration Release --no-build -- server --platform win-x64
   ```

## 5. Local resource limits

Never run more than two test commands at the same time. Prefer one test command at a time to avoid slowing down the user's machine.

Always stop any long-running process started during verification before completing the task.

## 6. Agent infrastructure

If a change touches `AGENTS.md`, a root compatibility bridge, `.agents`, `.agent`, `.claude`, `.cursor`, `.github/instructions`, or `.github/skills`, run:

```powershell
python3 .agents/check_agent_setup.py
```

If the change touches `.agents/check_agent_setup.py` or its tests, also run:

```powershell
python3 -m unittest discover -s .agents/tests -p 'test_check_agent_setup.py' -v
```

Agent-infrastructure-only Markdown/script changes do not require a .NET build. Still run `git diff --check` and inspect the final diff. If the change also touches C#, YAML, FTL, XAML, packaging, or runtime behavior, run the corresponding checks above as well.
