Feature: Properties
	As a LivingDoc User
	I want to view the project properties
	So that I know the context of the test run

Rule: Viewing Project Properties

Scenario: Viewing the sections in Properties
	Given I have navigated to the Properties
	Then I should have following sections in Properties
		| Name        |
		| Environment |
		| Test Run    |

Scenario: Viewing the Environment properties in Properties
	Given I have navigated to the Properties
	Then I should have following properties in the "Environment" section of Properties
		| Name                | Value                        |
		| Environment         | Staging                      |
		| Application Version | 2.5.0                        |
		| Build ID            | build-458                    |
		| Operating System    | Microsoft Windows 10.0.26200 |

Scenario: Viewing the Test Run properties in Properties
	Given I have navigated to the Properties
	Then I should have following properties in the "Test Run" section of Properties
		| Name         | Value              |
		| Run ID       | RUN-1042           |
		| Run Name     | Nightly Regression |
		| Triggered By | CI/CD Pipeline     |
		| Git Commit   | a3f92b1            |
