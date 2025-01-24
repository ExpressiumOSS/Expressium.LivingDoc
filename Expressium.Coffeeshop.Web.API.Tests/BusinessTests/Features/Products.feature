@BusinessTests @Products
Feature: Product Management
	As a User
	I want to order products from the web application
	So that I can enjoy my favorite coffee

@TA-3001 @Review
Scenario Outline: Ordering Coffee Confirmation Notification
	Given I have logged in with valid user credentials
	When I add <Brand> coffee to the shopping cart
	Then I should have <Price> in the confirmation notification message
Examples:
	| Brand                      | Price |
	| Santa Rita Cerrado Mineiro | $99   |
	| La Soledad Antioquia       | $77   |
