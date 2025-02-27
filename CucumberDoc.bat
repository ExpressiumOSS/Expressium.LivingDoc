echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

Expressium.LivingDocReport.exe --cucumber .\Small.json .\CucumberReport
rem Expressium.LivingDocReport.exe --cucumber .\Example.json .\CucumberReport
rem Expressium.LivingDocReport.exe --cucumber .\DataTable.json .\CucumberReport
rem Expressium.LivingDocReport.exe --cucumber .\Minimal.json .\CucumberReport

start .\CucumberReport\LivingDoc.html
