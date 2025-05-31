echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

Expressium.LivingDocReport.exe .\TestExecution.json .\LivingDoc.html

start .\LivingDoc.html
