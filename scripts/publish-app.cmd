@echo off
dotnet publish --output ..\output ..\app\src\Bittn.Api.csproj
xcopy ..\app\mocks ..\output\mocks /Y /I /E