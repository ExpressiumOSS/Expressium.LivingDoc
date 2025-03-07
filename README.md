# Expressium LivingDoc Test Report

SpecFlow has reached its end-of-life and a replacement
for the SpecFlow LivingDoc functionality is required in ReqnRoll.
This demo solution is intended to serve as input and inspiration for a potential solution.

The ongoing implementation of the Cucumber Messages in ReqnRoll
will provide a more flexible and powerful solution for generating LivingDoc test reports.

The current solution will generate a custom JSON file during test execution
using the FeatureContext and ScenarioContext classes.
The LivingDocGenerator will then create a self-contained HTML report based on the test execution output.

<br />
<img src="ExpressiumLivingDoc.png"
     alt="Expressium LivingDoc"
     style="display: block; margin-left: auto; margin-right: auto; width: 80%;" />

## How-To-Use
* Execute the ReqnRoll BDD business tests in the solution...
* Run the LivingDoc.bat file from the project's root folder...

## Demo Test Report
Web: https://expressium.dev/reqnroll/LivingDoc.html


