echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

start .\ReqnRoll.html

del .\LivingDoc.html
Expressium.LivingDoc.exe -input .\ReqnRoll.ndjson --output .\LivingDoc.html --title "Expressium.Coffeeshop.Web.API.Tests"
start .\LivingDoc.html
