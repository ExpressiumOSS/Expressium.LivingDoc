echo Runnning Regression Test...

set PROFILE=Development

rmdir /q /s .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net6.0\TestResults

dotnet nuget
dotnet build

dotnet test .\Expressium.Coffeeshop.Web.API.Tests\Expressium.Coffeeshop.Web.API.Tests.csproj --filter TestCategory="BusinessTests"
