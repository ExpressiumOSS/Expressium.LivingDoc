Feature: ContactUs
	As a User
	I want to send to a user Contact Us inquiry
	So that I can recieve detailed product information

@BusinessTests @Id:TC30001
Scenario: Successful Submitting a Contact Us Inquiry
	Given I have logged in with valid user credentials
	When I complete and submit the Contact Us formular
	Then I should receive an inquiry confirmation message
