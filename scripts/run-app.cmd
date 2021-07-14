@echo off
xcopy ..\app\mocks ..\app\src\bin\Debug\netcoreapp5.0\mocks /Y /I /E
dotnet run -p ..\app\src\Bittn.Api.csproj