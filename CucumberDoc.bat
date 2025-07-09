echo off

cd .\Expressium.LivingDoc.Tests\bin\Debug\net8.0

Expressium.LivingDoc.exe .\Samples\stack-traces.feature.ndjson ".\Compatibility Kit Stack Traces.html"
start "" ".\Stack Traces.html"

Expressium.LivingDoc.exe .\Samples\retry.feature.ndjson ".\Compatibility Kit Retry.html"
start "" ".\Compatibility Kit Retry.html"

Expressium.LivingDoc.exe .\Samples\attachments.feature.ndjson ".\Compatibility Kit Attachments.html"
start "" ".\Compatibility Kit Attachments.html"

Expressium.LivingDoc.exe .\Samples\examples-tables.feature.ndjson ".\Compatibility Kit Example Tables.html"
start "" ".\Compatibility Kit Example Tables.html"

Expressium.LivingDoc.exe .\Samples\minimal.feature.ndjson ".\Compatibility Kit Minimal.html"
start "" ".\Compatibility Kit Minimal.html"

Expressium.LivingDoc.exe .\Samples\data-tables.feature.ndjson ".\Compatibility Kit Data Tables.html"
start "" ".\Compatibility Kit Data Tables.html"

Expressium.LivingDoc.exe .\Samples\rules.feature.ndjson ".\Compatibility Kit Rules.html"
start "" ".\Compatibility Kit Rules.html"

rem Expressium.LivingDoc.exe .\Samples\skipped.feature.ndjson ".\Compatibility Kit Skipped.html"
rem start "" ".\Compatibility Kit Skipped.html"
