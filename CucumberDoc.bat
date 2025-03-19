echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

rem Expressium.LivingDocReport.exe --cucumber .\Samples\examples-tables.feature.ndjson .\CucumberReport
rem Expressium.LivingDocReport.exe --cucumber .\Samples\minimal.feature.ndjson .\CucumberReport
rem Expressium.LivingDocReport.exe --cucumber .\Samples\data-tables.feature.ndjson .\CucumberReport
Expressium.LivingDocReport.exe --cucumber .\Samples\rules.feature.ndjson .\CucumberReport
rem Expressium.LivingDocReport.exe --cucumber .\Samples\skipped.feature.ndjson .\CucumberReport

start .\CucumberReport\LivingDoc.html
