echo off

cd .\Expressium.LivingDoc.UnitTests\bin\Debug\net8.0

del .\Samples\Merge.html
Expressium.LivingDoc.exe --merge .\Samples\minimal.feature.ndjson .\Samples\examples-tables.feature.ndjson .\Samples\Merge.html
start .\Samples\Merge.html
