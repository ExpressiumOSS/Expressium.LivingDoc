Feature: Analytics
	As a LivingDoc User
	I want to view the analytics
	So that I gain an overview of the test results

Rule: Evaluating Test Results in Analytics

Scenario: Evaluating Features Test Results in Analytics
	Given I have navigated to the Features Analytics
	Then I should have the passed features displayed in Analytics
	And I should have the duration displayed in Analytics

Scenario: Evaluating Scenarios Test Results in Analytics
	Given I have navigated to the Scenarios Analytics
	Then I should have the passed scenarios displayed in Analytics
	And I should have the duration displayed in Analytics

Scenario: Evaluating Steps Test Results in Analytics
	Given I have navigated to the Steps Analytics
	Then I should have the passed steps displayed in Analytics
	And I should have the duration displayed in Analytics

