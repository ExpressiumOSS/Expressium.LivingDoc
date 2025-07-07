echo off

cd .\Expressium.LivingDoc.Tests\bin\Debug\net8.0

Expressium.LivingDoc.exe .\Samples\stack-traces.feature.ndjson .\CucumberReportStackTraces.html
start .\CucumberReportStackTraces.html

Expressium.LivingDoc.exe .\Samples\retry.feature.ndjson .\CucumberReportRetry.html
start .\CucumberReportRetry.html

Expressium.LivingDoc.exe .\Samples\attachments.feature.ndjson .\CucumberReportAttachments.html
start .\CucumberReportAttachments.html

Expressium.LivingDoc.exe .\Samples\examples-tables.feature.ndjson .\CucumberReportExampleTables.html
start .\CucumberReportExampleTables.html

Expressium.LivingDoc.exe .\Samples\minimal.feature.ndjson .\CucumberReportMinimal.html
start .\CucumberReportMinimal.html

Expressium.LivingDoc.exe .\Samples\data-tables.feature.ndjson .\CucumberReportDataTables.html
start .\CucumberReportDataTables.html

Expressium.LivingDoc.exe .\Samples\rules.feature.ndjson .\CucumberReportRules.html
start .\CucumberReportRules.html

rem Expressium.LivingDoc.exe .\Samples\skipped.feature.ndjson .\CucumberReportSkipped.html
rem start .\CucumberReportSkipped.html
