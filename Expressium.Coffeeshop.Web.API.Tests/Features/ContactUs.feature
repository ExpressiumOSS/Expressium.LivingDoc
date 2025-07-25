@TA-3000 @ContactUs @BusinessTests
Feature: ContactUs
	As a User
	I want to send to a Contact Us inquiry
	So that I can recieve detailed product information

Background:
	Given I have logged in with valid user credentials

@TA-3001 @Done
Scenario: Successful Submitting a Contact Us Inquiry
	When I complete and submit the Contact Us formular
	Then I should receive an inquiry confirmation message

@TA-3002 @Ignore
Scenario: Unsuccessful Submitting a Contact Us Inquiry
	When I complete and submit the Contact Us formular
	Then I should receive an inquiry confirmation message

@TA-3003 @Review
Scenario: Successful Resubmitting a Contact Us Inquiry
	When I complete and submit the Contact Us formular
	And I resubmit the Contact Us formular
	Then I should receive an inquiry confirmation message
