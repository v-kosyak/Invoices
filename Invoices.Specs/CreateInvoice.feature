Feature: Invoice Creation
	In order to create a new invoice
	As user
	I want to be able to specify invoice details, submit and book an invoice

Scenario: Select Customer
	Given I am authorized
	And I have selected customer with id=101
	Then customer with id=101 is selected
	And customer address is Gartnergade 12
