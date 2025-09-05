echo off

cd .\Expressium.LivingDoc.UnitTests\bin\Debug\net8.0

del .\Samples\Native.html
Expressium.LivingDoc.exe --native .\Samples\native.json .\Samples\Native.html
start .\Samples\Native.html
