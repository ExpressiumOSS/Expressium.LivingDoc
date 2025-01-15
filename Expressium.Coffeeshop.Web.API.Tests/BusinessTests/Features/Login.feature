Feature: Login
	As a User
	I want to login on the web application
	So that I can administrate my product orders

@BusinessTests @Id:TC20001
Scenario: Successful User Login with Valid Credentials
	Given I have logged in with valid user credentials
	Then I should be redirected to the Home page

@BusinessTests @Id:TC20002
Scenario: Unsuccessful User Login with Invalid Credentials
	Given I have logged in with invalid user credentials
	Then I should have an error message on the Login page

