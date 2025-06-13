echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

Expressium.LivingDoc.exe --native .\TestExecution.json .\LivingDoc.html

start .\LivingDoc.html
