echo off

cd .\Expressium.Coffeeshop.Web.API.Tests\bin\Debug\net8.0

del .\LivingDoc.html
Expressium.LivingDoc.exe --native c:\Temp\TestExecutionEx.json .\LivingDoc.html

start .\LivingDoc.html
