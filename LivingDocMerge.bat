echo off

cd .\Expressium.LivingDoc.UnitTests\bin\Debug\net8.0

del .\Samples\Merge.html
Expressium.LivingDoc.Cli.exe --merge --input .\CCK\Samples\minimal\minimal.ndjson .\CCK\Samples\examples-tables\examples-tables.ndjson --output .\Samples\Merge.html --title "Merged Documentation"
start .\Samples\Merge.html
