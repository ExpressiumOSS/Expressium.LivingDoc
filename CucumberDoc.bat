echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

Expressium.TestExecutionReport.exe --cucumber .\Cucumber.json .\CucumberReport

start .\CucumberReport\LivingDoc.html
