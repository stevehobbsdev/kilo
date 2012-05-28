@echo off

call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86_amd64

set push=no

if "%1"=="pushpackage" (
	set push=yes
)

echo %push%

msbuild ..\src\kilo.sln /p:Platform="Any CPU",Configuration=Release