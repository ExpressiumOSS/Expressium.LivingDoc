Feature: Backgrounds

Background:
	Given I have an initial step running

@passed
Scenario: Scenario without Examples and Passing Background Step
	Given I have a Passed step running

@failed
Scenario: Scenario without Examples and Failing Background Step
	Given I have a Passed step running

#@passed
#Scenario: Scenario with Examples and Passing Background Step
#	Given I have a <status> step running
#Examples:
#	| status | Level |
#	| Passed | 1     |
#
#@failed
#Scenario: Scenario with Examples and Failing Background Step
#	Given I have a <status> step running
#Examples:
#	| status | Level |
#	| Passed | 2     |