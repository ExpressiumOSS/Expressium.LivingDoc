echo off

cd .\Expressium.LivingDoc.Tests\bin\Debug\net8.0

Expressium.LivingDoc.exe .\Samples\background.feature.ndjson ".\Samples\Compatibility Kit Background.html"
start "" ".\Samples\Compatibility Kit Background.html"

Expressium.LivingDoc.exe .\Samples\stack-traces.feature.ndjson ".\Samples\Compatibility Kit Stack Traces.html"
start "" ".\Samples\Compatibility Kit Stack Traces.html"

Expressium.LivingDoc.exe .\Samples\retry.feature.ndjson ".\Samples\Compatibility Kit Retry.html"
start "" ".\Samples\Compatibility Kit Retry.html"

Expressium.LivingDoc.exe .\Samples\attachments.feature.ndjson ".\Samples\Compatibility Kit Attachments.html"
start "" ".\Samples\Compatibility Kit Attachments.html"

Expressium.LivingDoc.exe .\Samples\examples-tables.feature.ndjson ".\Samples\Compatibility Kit Example Tables.html"
start "" ".\Samples\Compatibility Kit Example Tables.html"

Expressium.LivingDoc.exe .\Samples\minimal.feature.ndjson ".\Samples\Compatibility Kit Minimal.html"
start "" ".\Samples\Compatibility Kit Minimal.html"

Expressium.LivingDoc.exe .\Samples\data-tables.feature.ndjson ".\Samples\Compatibility Kit Data Tables.html"
start "" ".\Samples\Compatibility Kit Data Tables.html"

Expressium.LivingDoc.exe .\Samples\rules.feature.ndjson ".\Compatibility Kit Rules.html"
start "" ".\Compatibility Kit Rules.html"

rem Expressium.LivingDoc.exe .\Samples\skipped.feature.ndjson ".\Samples\Compatibility Kit Skipped.html"
rem start "" ".\Samples\Compatibility Kit Skipped.html"
