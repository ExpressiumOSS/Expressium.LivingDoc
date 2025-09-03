echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

del .\LivingDocPerago.html
Expressium.LivingDoc.exe --native c:\Temp\TestExecutionEx.json .\LivingDocPerago.html

start .\LivingDocPerago.html
