echo off

cd .\Expressium.LivingDoc.UnitTests\bin\Debug\net8.0

del .\Samples\Coffeeshop.html
Expressium.LivingDoc.Cli.exe --history --input .\Samples\coffeeshop.feature.ndjson .\Samples\History\Coffee*.ndjson --output .\Samples\Coffeeshop.html --title "Expressium.Coffeeshop.Web.API.Tests"
start .\Samples\Coffeeshop.html
