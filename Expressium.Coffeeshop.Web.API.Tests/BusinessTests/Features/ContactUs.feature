@TA-2000 @ContactUs @BusinessTests
Feature: ContactUs
	As a User
	I want to send to a user Contact Us inquiry
	So that I can recieve detailed product information

Background:
	Given I have logged in with valid user credentials

@TA-2001 @Done
Scenario: Successful Submitting a Contact Us Inquiry
	When I complete and submit the Contact Us formular
	Then I should receive an inquiry confirmation message

@TA-2002 @Ignore
Scenario: Unsuccessful Submitting a Contact Us Inquiry
	When I complete and submit the Contact Us formular
	Then I should receive an inquiry confirmation message

@TA-2003 @Review
Scenario: Successful Resubmitting a Contact Us Inquiry
	When I complete and submit the Contact Us formular
	And I resubmit the Contact Us formular
	Then I should receive an inquiry confirmation message
