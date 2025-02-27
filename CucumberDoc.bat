echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

rem Expressium.TestExecutionReport.exe --cucumber .\Small.json .\CucumberReport
Expressium.TestExecutionReport.exe --cucumber .\Example.json .\CucumberReport

start .\CucumberReport\LivingDoc.html
