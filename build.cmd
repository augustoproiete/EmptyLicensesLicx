@echo off
setlocal

cd %~dp0

set _version=%~1
if "%_version%"=="" set _version=0.0.1-local

set CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

if exist %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
if not exist %LocalAppData%\NuGet mkdir %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
if exist .nuget\NuGet.exe goto run
mkdir .nuget
copy %CACHED_NUGET% .nuget\NuGet.exe > nul
.nuget\NuGet.exe Update -Self

:run
if not exist out mkdir out
.nuget\NuGet.exe pack "src\EmptyLicensesLicx.nuspec" -Version %_version% -NoPackageAnalysis -NonInteractive -OutputDirectory out
goto :eof
