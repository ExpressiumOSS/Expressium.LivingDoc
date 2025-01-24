echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net6.0

ReqnRoll.TestExecutionReport.exe .\TestExecution.json .\TestReport

start .\TestReport\LivingDoc.html
