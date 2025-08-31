Feature: Backgrounds-Examples

Background:
	Given I have an initial background step running

Scenario: Scenario with Examples both Passing and Failing Background Steps
	Given I have a <status> step running
Examples:
	| status | Level |
	| Passed | 1     |
	| Passed | 2     |