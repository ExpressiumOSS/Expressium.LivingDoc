echo off

cd .\Expressium.LivingDoc.UnitTests\bin\Debug\net8.0

del .\CCK\Samples\Pending\Pending.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\pending\pending.ndjson --output .\CCK\Samples\Pending\Pending.html --title "Compatibility Kit Pending"
start .\CCK\Samples\Pending\Pending.html

del .\CCK\Samples\Ambiguous\Ambiguous.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\ambiguous\ambiguous.ndjson --output .\CCK\Samples\Ambiguous\Ambiguous.html --title "Compatibility Kit Ambiguous"
start .\CCK\Samples\Ambiguous\Ambiguous.html

del .\CCK\Samples\Empty\Empty.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\empty\empty.ndjson --output .\CCK\Samples\Empty\Empty.html --title "Compatibility Kit Empty"
start .\CCK\Samples\Empty\Empty.html

del .\CCK\Samples\Backgrounds\Backgrounds.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\backgrounds\backgrounds.ndjson --output .\CCK\Samples\Backgrounds\Backgrounds.html --title "Compatibility Kit Backgrounds"
start .\CCK\Samples\Backgrounds\Backgrounds.html

del .\CCK\Samples\Stack-Traces\StackTraces.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\stack-traces\stack-traces.ndjson --output .\CCK\Samples\Stack-Traces\StackTraces.html --title "Compatibility Kit Stack Traces"
start .\CCK\Samples\Stack-Traces\StackTraces.html

del .\CCK\Samples\Retry\Retry.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\retry\retry.ndjson --output .\CCK\Samples\Retry\Retry.html --title "Compatibility Kit Retry"
start .\CCK\Samples\Retry\Retry.html

del .\CCK\Samples\Attachments\Attachments.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\attachments\attachments.ndjson --output .\CCK\Samples\Attachments\Attachments.html --title "Compatibility Kit Attachments"
start .\CCK\Samples\Attachments\Attachments.html

del .\CCK\Samples\Examples-Tables\ExampleTables.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\examples-tables\examples-tables.ndjson --output .\CCK\Samples\Examples-Tables\ExampleTables.html --title "Compatibility Kit Example Tables"
start .\CCK\Samples\Examples-Tables\ExampleTables.html

del .\CCK\Samples\Minimal\Minimal.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\minimal\minimal.ndjson --output .\CCK\Samples\Minimal\Minimal.html --title "Compatibility Kit Minimal"
start .\CCK\Samples\Minimal\Minimal.html

del .\CCK\Samples\Data-Tables\DataTables.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\data-tables\data-tables.ndjson --output .\CCK\Samples\Data-Tables\DataTables.html --title "Compatibility Kit Data Tables"
start .\CCK\Samples\Data-Tables\DataTables.html

del .\CCK\Samples\Rules\Rules.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\rules\rules.ndjson --output .\CCK\Samples\Rules\Rules.html --title "Compatibility Kit Rules"
start .\CCK\Samples\Rules\Rules.html

del .\CCK\Samples\Rules-Backgrounds\RulesBackgrounds.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\rules-backgrounds\rules-backgrounds.ndjson --output .\CCK\Samples\Rules-Backgrounds\RulesBackgrounds.html --title "Compatibility Kit Rules Backgrounds"
start .\CCK\Samples\Rules-Backgrounds\RulesBackgrounds.html

del .\CCK\Samples\Skipped\Skipped.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\skipped\skipped.ndjson --output .\CCK\Samples\Skipped\Skipped.html --title "Compatibility Kit Skipped"
start .\CCK\Samples\Skipped\Skipped.html

del .\CCK\Samples\Hooks\HooksErrors.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\hooks\hooks.ndjson --output .\CCK\Samples\Hooks\HooksErrors.html --title "Compatibility Kit Hooks Errors"
start .\CCK\Samples\Hooks\HooksErrors.html
