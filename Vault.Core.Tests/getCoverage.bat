dotnet build
coverlet %~dp0/bin/Debug/netcoreapp2.1/Vault.Core.Tests.dll -t dotnet -a "test Vault.Core.Tests.csproj --no-build" -o coverage.xml -f opencover --exclude [xunit.*]*
reportgenerator -reports:coverage.xml -targetdir:CoverageReport
pause