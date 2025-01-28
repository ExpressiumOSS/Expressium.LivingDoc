echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

ReqnRoll.TestExecutionReport.exe .\TestExecution.json .\TestReport

start .\TestReport\LivingDoc.html
