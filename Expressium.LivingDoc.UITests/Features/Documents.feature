Feature: Documents
	As a LivingDoc User
	I want to view the documents
	So that I validate test run test results

Rule: Loading Test Results in Documents

Scenario: Loading a Feature from the Overview List in Documents
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords      |
		| TA-1000 Login |
	And I load the feature document in the Overview List
	Then I should have following feature properties in the Document View
		| Tags                           | Name  | Description |
		| @TA-1000 @Login @BusinessTests | Login | As a User   |
	And I should have following scenario properties in the Document View
		| Tags             | Name                                             |
		| @TA-1001 @Done   | Successful User Login with Valid Credentials     |
		| @TA-1002 @Review | Unsuccessful User Login with Invalid Credentials |

Scenario: Loading a Scenario from the Overview List in Documents
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords      |
		| TA-1001 Login |
	And I load the scenario document in the Overview List
	Then I should have following feature properties in the Document View
		| Tags                           | Name  | Description |
		| @TA-1000 @Login @BusinessTests | Login | As a User   |
	And I should have following scenario properties in the Document View
		| Tags           | Name                                         |
		| @TA-1001 @Done | Successful User Login with Valid Credentials |
