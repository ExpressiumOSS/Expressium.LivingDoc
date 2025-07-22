echo off

cd .\Expressium.LivingDoc.Tests\bin\Debug\net8.0

del ".\Samples\Compatibility Kit Pending.html"
Expressium.LivingDoc.exe .\Samples\pending.feature.ndjson ".\Samples\Compatibility Kit Pending.html"
start "" ".\Samples\Compatibility Kit Pending.html"

del ".\Samples\Compatibility Kit Ambiguous.html"
Expressium.LivingDoc.exe .\Samples\ambiguous.feature.ndjson ".\Samples\Compatibility Kit Ambiguous.html"
start "" ".\Samples\Compatibility Kit Ambiguous.html"

del ".\Samples\Compatibility Kit Empty.html"
Expressium.LivingDoc.exe .\Samples\empty.feature.ndjson ".\Samples\Compatibility Kit Empty.html"
start "" ".\Samples\Compatibility Kit Empty.html"

del ".\Samples\Compatibility Kit Background.html"
Expressium.LivingDoc.exe .\Samples\background.feature.ndjson ".\Samples\Compatibility Kit Background.html"
start "" ".\Samples\Compatibility Kit Background.html"

del ".\Samples\Compatibility Kit Stack Traces.html"
Expressium.LivingDoc.exe .\Samples\stack-traces.feature.ndjson ".\Samples\Compatibility Kit Stack Traces.html"
start "" ".\Samples\Compatibility Kit Stack Traces.html"

del ".\Samples\Compatibility Kit Retry.html"
Expressium.LivingDoc.exe .\Samples\retry.feature.ndjson ".\Samples\Compatibility Kit Retry.html"
start "" ".\Samples\Compatibility Kit Retry.html"

del ".\Samples\Compatibility Kit Attachments.html"
Expressium.LivingDoc.exe .\Samples\attachments.feature.ndjson ".\Samples\Compatibility Kit Attachments.html"
start "" ".\Samples\Compatibility Kit Attachments.html"

del ".\Samples\Compatibility Kit Example Tables.html"
Expressium.LivingDoc.exe .\Samples\examples-tables.feature.ndjson ".\Samples\Compatibility Kit Example Tables.html"
start "" ".\Samples\Compatibility Kit Example Tables.html"

del ".\Samples\Compatibility Kit Minimal.html"
Expressium.LivingDoc.exe .\Samples\minimal.feature.ndjson ".\Samples\Compatibility Kit Minimal.html"
start "" ".\Samples\Compatibility Kit Minimal.html"

del ".\Samples\Compatibility Kit Data Tables.html"
Expressium.LivingDoc.exe .\Samples\data-tables.feature.ndjson ".\Samples\Compatibility Kit Data Tables.html"
start "" ".\Samples\Compatibility Kit Data Tables.html"

del ".\Compatibility Kit Rules.html"
Expressium.LivingDoc.exe .\Samples\rules.feature.ndjson ".\Compatibility Kit Rules.html"
start "" ".\Compatibility Kit Rules.html"

rem del ".\Samples\Compatibility Kit Skipped.html"
rem Expressium.LivingDoc.exe .\Samples\skipped.feature.ndjson ".\Samples\Compatibility Kit Skipped.html"
rem start "" ".\Samples\Compatibility Kit Skipped.html"
