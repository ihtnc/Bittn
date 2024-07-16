@echo off
xcopy ..\app\mocks ..\app\src\bin\Debug\netcoreapp8.0\mocks /Y /I /E
dotnet run --project ..\app\src\Bittn.Api.csproj