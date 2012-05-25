@echo off

call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86_amd64

set push=no

if "%1"=="pushpackage" (
	set push=yes
)

echo %push%

REM Kilo
msbuild packageBuild.csproj /property:ProjectName=Kilo,Configuration=Release,PushPackage=%push% /p:Platform="Any CPU"

REM Kilo.Data
msbuild packageBuild.csproj /property:ProjectName=Kilo.Data,Configuration=Release,PushPackage=%push% /p:Platform="Any CPU"

REM Kilo.Mvc
msbuild packageBuild.csproj /property:ProjectName=Kilo.Mvc,Configuration=Release,PushPackage=%push% /p:Platform="Any CPU"