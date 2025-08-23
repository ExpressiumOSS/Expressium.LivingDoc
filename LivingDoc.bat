echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

del .\LivingDocNative.html
Expressium.LivingDoc.exe --native .\TestExecution.json .\LivingDocNative.html
start .\LivingDocNative.html

start .\ReqnRoll.html

del .\LivingDoc.html
Expressium.LivingDoc.exe -input .\TestExecution.ndjson --output .\LivingDoc.html --title "Expressium.Coffeeshop.Web.API.Tests"
start .\LivingDoc.html
