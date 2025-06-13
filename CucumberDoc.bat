echo off

cd .\Expressium.LivingDoc.Tests\bin\Debug\net8.0

Expressium.LivingDoc.exe --cucumber .\Samples\examples-tables.feature.ndjson .\CucumberReportExampleTables.html
start .\CucumberReportExampleTables.html

Expressium.LivingDoc.exe --cucumber .\Samples\minimal.feature.ndjson .\CucumberReportMinimal.html
start .\CucumberReportMinimal.html

Expressium.LivingDoc.exe --cucumber .\Samples\data-tables.feature.ndjson .\CucumberReportDataTables.html
start .\CucumberReportDataTables.html

Expressium.LivingDoc.exe --cucumber .\Samples\rules.feature.ndjson .\CucumberReportRules.html
start .\CucumberReportRules.html

rem Expressium.LivingDoc.exe --cucumber .\Samples\skipped.feature.ndjson .\CucumberReportSkipped.html
rem start .\CucumberReportSkipped.html
