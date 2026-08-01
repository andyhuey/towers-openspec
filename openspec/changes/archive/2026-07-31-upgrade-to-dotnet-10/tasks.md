## 1. Retarget projects

- [x] 1.1 Change `<TargetFramework>` from `net8.0` to `net10.0` in `src/TowersOfHanoi.Core/TowersOfHanoi.Core.csproj`
- [x] 1.2 Change `<TargetFramework>` from `net8.0` to `net10.0` in `src/TowersOfHanoi.Console/TowersOfHanoi.Console.csproj`
- [x] 1.3 Change `<TargetFramework>` from `net8.0` to `net10.0` in `tests/TowersOfHanoi.Core.Tests/TowersOfHanoi.Core.Tests.csproj`

## 2. Update test dependencies

- [x] 2.1 Run `dotnet restore` and `dotnet build` on the test project; if `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`, or `coverlet.collector` fail to restore/build cleanly on `net10.0`, bump them to the latest stable versions compatible with .NET 10

## 3. Verify

- [x] 3.1 Run `dotnet build` on the solution and confirm it succeeds with no warnings/errors introduced by the retarget
- [x] 3.2 Run `dotnet test` and confirm all existing tests still pass
- [x] 3.3 Run `dotnet run --project src/TowersOfHanoi.Console` and manually play a short game to confirm the console UI still behaves correctly under .NET 10

## 4. Update documentation

- [x] 4.1 Update `CLAUDE.md` to reference .NET 10 instead of .NET 8 (project description and any other mentions)
