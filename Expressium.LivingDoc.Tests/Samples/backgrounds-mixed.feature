Feature: Backgrounds-Examples

Background:
	Given I have an initial background step running

Scenario: Scenario without Examples and with Background Step
	Given I have a Passed step running
	Then The scenario should be marked as Passed

Scenario: Scenario with Examples both Passing and Failing Background Steps
	Given I have a <status> step running
	Then The scenario should be marked as <result>
Examples:
	| status | level | result     |
	| Passed | 1     | Failed     |
	| Passed | 2     | Incomplete |
	| Passed | 3     | Passed     |
	| Passed | 4     | Failed     |
