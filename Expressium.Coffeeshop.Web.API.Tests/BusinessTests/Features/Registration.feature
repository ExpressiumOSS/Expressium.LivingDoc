@TA-4000 @Registration @BusinessTests
Feature: Registration
	As a User
	I want to send to a user Registration inquiry
	So that I can order my favorite coffee

@TA-4001 @InProgress
Scenario: Successful Submitting a Registration Inquiry
	When I complete and submit the Registration formular
	Then I should be redirected to the Login page

@TA-4002 @InProgress
Scenario: Successful Canceling a Registration Inquiry
	When I complete and cancel the Registration formular
	Then I should be redirected to the Login page

@TA-4003 @InProgress
Scenario: Successful Resubmitting a Registration Inquiry
	When I complete and submit the Registration formular
	And I complete and submit the Registration formular
	Then I should be redirected to the Login page