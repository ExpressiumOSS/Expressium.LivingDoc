echo off

cd .\Expressium.LivingDoc.UnitTests\bin\Debug\net8.0

del .\Samples\Merge.html
Expressium.LivingDoc.Cli.exe --merge --input .\Samples\minimal.feature.ndjson .\Samples\examples-tables.feature.ndjson --output .\Samples\Merge.html --title "Merged Documentation"
start .\Samples\Merge.html
