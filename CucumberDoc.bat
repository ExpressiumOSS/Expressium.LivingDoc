echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

rem Expressium.LivingDocReport.exe --cucumber C:\Company\Source\ExpressiumLivingDocTestReport\Expressium.CucumberMessages.Tests\Samples\examples-tables.feature.ndjson .\CucumberReport.html
Expressium.LivingDocReport.exe --cucumber C:\Company\Source\ExpressiumLivingDocTestReport\Expressium.CucumberMessages.Tests\Samples\minimal.feature.ndjson .\CucumberReport.html
rem Expressium.LivingDocReport.exe --cucumber C:\Company\Source\ExpressiumLivingDocTestReport\Expressium.CucumberMessages.Tests\Samples\data-tables.feature.ndjson .\CucumberReport.html
rem Expressium.LivingDocReport.exe --cucumber C:\Company\Source\ExpressiumLivingDocTestReport\Expressium.CucumberMessages.Tests\Samples\rules.feature.ndjson .\CucumberReport.html
rem Expressium.LivingDocReport.exe --cucumber C:\Company\Source\ExpressiumLivingDocTestReport\Expressium.CucumberMessages.Tests\Samples\skipped.feature.ndjson .\CucumberReport.html

start .\CucumberReport.html
