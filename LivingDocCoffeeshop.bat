echo off

cd .\Expressium.LivingDoc.UnitTests\bin\Debug\net10.0

del .\Samples\Coffeeshop.html
Expressium.LivingDoc.Cli.exe --generate --input .\Samples\coffeeshop.ndjson --output .\Samples\Coffeeshop.html --title "Expressium.Coffeeshop.Web.API.Tests" .\Samples\properties.json
start .\Samples\Coffeeshop.html

