Feature: Documents
	As a LivingDoc User
	I want to view the documents
	So that I validate test run test results

Rule: Loading Overview List Test Results in Document View

Scenario: Loading a feature from the Overview List in Document View
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

Scenario: Loading a scenario from the Overview List in Document View
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

Scenario: Loading a scenario with rule from the Overview List in Document View
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords       |
		| TA-4001 Orders |
	And I load the scenario document in the Overview List
	Then I should have following rule properties in the Document View
		| Tags     | Name                               |
		| @TR-4001 | Ordering Confirmation Notification |

Rule: Loading Features List Test Results in Document View

Scenario: Loading a feature from the Features List in Document View
	Given I have navigated to the Features List
	When I filter by following keywords in the Filter Bar
		| Keywords           |
		| TA-3000 Contact Us |
	And I load the feature document in the Feature List
	Then I should have following feature properties in the Document View
		| Tags                               | Name       | Description |
		| @TA-3000 @ContactUs @BusinessTests | Contact Us | As a User   |
	And I should have following scenario properties in the Document View
		| Tags             | Name                                         |
		| @TA-3001 @Done   | Successful Submitting a Contact Us Inquiry   |
		| @TA-3002 @Ignore | Unsuccessful Submitting a Contact Us Inquiry |
		| @TA-3003 @Review | Successful Resubmitting a Contact Us Inquiry |

Rule: Loading Scenarios List Test Results in Document View

Scenario: Loading a scenario from the Scenarios List in Document View
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords             |
		| TA-2002 Registration |
	And I load the scenario document in the Scenarios List
	Then I should have following feature properties in the Document View
		| Tags                                  | Name         | Description |
		| @TA-2000 @Registration @BusinessTests | Registration | As a User   |
	And I should have following scenario properties in the Document View
		| Tags                 | Name                                        |
		| @TA-2002 @InProgress | Successful Canceling a Registration Inquiry |
	And I should have following step properties in the Document View
		| Name                                            |
		| I complete and cancel the Registration formular |
		| I should be redirected to the Login page        |

Scenario: Loading a scenario with exmaples from the Scenarios List in Document View
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords       |
		| TA-4001 Orders |
	And I load the scenario document in the Scenarios List
	Then I should have following number of scenarios in the Document View
		| Numbers |
		|       2 |
	And I should have following scenario properties in the Document View
		| Tags             | Name                                      |
		| @TA-4001 @Review | Ordering Coffee Confirmation Notification |

Rule: Loading Steps List Test Results in Document View

Scenario: Loading a step from the Steps List in Document View
	Given I have navigated to the Steps List
	When I filter by following keywords in the Filter Bar
		| Keywords             |
		| TA-2003 Registration |
	And I load the step document in the Steps List
	Then I should have following feature properties in the Document View
		| Tags                                  | Name         | Description |
		| @TA-2000 @Registration @BusinessTests | Registration | As a User   |
	And I should have following scenario properties in the Document View
		| Tags                 | Name                                           |
		| @TA-2003 @InProgress | Successful Resubmitting a Registration Inquiry |
	And I should have following step properties in the Document View
		| Name                                            |
		| I complete and submit the Registration formular |
		| I should be redirected to the Login page        |