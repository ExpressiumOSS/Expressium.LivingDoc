@TC-2000 @ContactUs @BusinessTests
Feature: ContactUs
	As a User
	I want to send to a user Contact Us inquiry
	So that I can recieve detailed product information

Rule: Creating a new account
    This rule covers behavior related to new account creation.

Background:
	Given I am logged in as a registered user

@TC-2001 @Review
Scenario: Successful Submitting a Contact Us Inquiry
	Given I have logged in with valid user credentials
	When I complete and submit a valid Contact Us formular
	Then I should have a confirmation message displayed on the Contact Us page
