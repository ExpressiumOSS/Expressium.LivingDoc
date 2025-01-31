echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

Expressium.TestExecutionReport.exe .\TestExecution.json .\TestExecutionReport

start .\TestExecutionReport\LivingDoc.html
