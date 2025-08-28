echo off

cd .\Expressium.LivingDoc.Tests\bin\Debug\net8.0

del .\Samples\Coffeeshop.html
Expressium.LivingDoc.exe --native .\Samples\coffeeshop.json .\Samples\Coffeeshop.html
start .\Samples\Coffeeshop.html

del .\Samples\Native.html
Expressium.LivingDoc.exe --native .\Samples\native.json .\Samples\Native.html
start .\Samples\Native.html
