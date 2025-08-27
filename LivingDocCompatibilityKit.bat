echo off

cd .\Expressium.LivingDoc.Tests\bin\Debug\net8.0

del .\Samples\Pending.html
Expressium.LivingDoc.exe -input .\Samples\pending.feature.ndjson --output .\Samples\Pending.html --title "Compatibility Kit Pending"
start .\Samples\Pending.html

del .\Samples\Ambiguous.html
Expressium.LivingDoc.exe -input .\Samples\ambiguous.feature.ndjson --output .\Samples\Ambiguous.html --title "Compatibility Kit Ambiguous"
start .\Samples\Ambiguous.html

del .\Samples\Empty.html
Expressium.LivingDoc.exe -input .\Samples\empty.feature.ndjson --output .\Samples\Empty.html --title "Compatibility Kit Empty"
start .\Samples\Empty.html

del .\Samples\Background.html
Expressium.LivingDoc.exe -input .\Samples\background.feature.ndjson --output .\Samples\Background.html --title "Compatibility Kit Background"
start .\Samples\Background.html

del .\Samples\StackTraces.html
Expressium.LivingDoc.exe -input .\Samples\stack-traces.feature.ndjson --output .\Samples\StackTraces.html --title "Compatibility Kit Stack Traces"
start .\Samples\StackTraces.html

del .\Samples\Retry.html
Expressium.LivingDoc.exe -input .\Samples\retry.feature.ndjson --output .\Samples\Retry.html --title "Compatibility Kit Retry"
start .\Samples\Retry.html

del .\Samples\Attachments.html
Expressium.LivingDoc.exe -input .\Samples\attachments.feature.ndjson --output .\Samples\Attachments.html --title "Compatibility Kit Attachments"
start .\Samples\Attachments.html

del .\Samples\ExampleTables.html
Expressium.LivingDoc.exe -input .\Samples\examples-tables.feature.ndjson --output .\Samples\ExampleTables.html --title "Compatibility Kit Example Tables"
start .\Samples\ExampleTables.html

del .\Samples\Minimal.html
Expressium.LivingDoc.exe -input .\Samples\minimal.feature.ndjson --output .\Samples\Minimal.html --title "Compatibility Kit Minimal"
start .\Samples\Minimal.html

del .\Samples\DataTables.html
Expressium.LivingDoc.exe -input .\Samples\data-tables.feature.ndjson --output .\Samples\DataTables.html --title "Compatibility Kit Data Tables"
start .\Samples\DataTables.html

del .\Rules.html
Expressium.LivingDoc.exe -input .\Samples\rules.feature.ndjson --output .\Rules.html --title "Compatibility Kit Rules"
start .\Rules.html

del .\Samples\Skipped.html
Expressium.LivingDoc.exe -input .\Samples\skipped.feature.ndjson --output .\Samples\Skipped.html --title "Compatibility Kit Skipped"
start .\Samples\Skipped.html

del .\Samples\HooksErrors.html
Expressium.LivingDoc.exe -input .\Samples\hooks-errors.feature.ndjson --output .\Samples\HooksErrors.html --title "Compatibility Kit Hooks Errors"
start .\Samples\HooksErrors.html


