Feature: Orders

Scenario Outline: Ordering Coffee Confirmation Notification
	When I add <Brand> coffee to the shopping cart
Examples:
	| Brand                      |
	| Santa Rita Cerrado Mineiro |
	| La Soledad Antioquia       |
