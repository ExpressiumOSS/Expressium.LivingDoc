echo off

cd .\Expressium.LivingDoc.UnitTests\bin\Debug\net8.0

del .\Samples\NativeHistory.html
Expressium.LivingDoc.Cli.exe --native --input .\Samples\native-history.json --output .\Samples\NativeHistory.html
start .\Samples\NativeHistory.html
