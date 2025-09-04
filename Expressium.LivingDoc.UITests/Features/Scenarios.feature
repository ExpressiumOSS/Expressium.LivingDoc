Feature: Scenarios
	As a LivingDoc User
	I want to filter the scenarios list
	So that I select validate specific test results

Rule: Filter by Keywords in Scenarios List

Scenario: Filter with Unknown Keywords in the Scenarios List
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords  |
		| Gibberish |
	Then I should have following number of visible objects in the Scenarios List
		| Scenarios |
		| 0         |

Scenario: Filter by Multiple Keywords in the Scenarios List
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords                          |
		| inquiry resubmitting registration |
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                      |
		| Successful Resubmitting a Registration Inquiry |

Scenario: Filter by Uppercased Keywords in the Scenarios List
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| USER LOGIN |
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                        |
		| Successful User Login with Valid Credentials     |
		| Unsuccessful User Login with Invalid Credentials |

Scenario: Filter by Lowercased Keywords in the Scenarios List
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| user login |
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                        |
		| Successful User Login with Valid Credentials     |
		| Unsuccessful User Login with Invalid Credentials |

Scenario: Filter by Feature Tag Keywords in the Scenarios List
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| @TA-1000 |
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                        |
		| Successful User Login with Valid Credentials     |
		| Unsuccessful User Login with Invalid Credentials |

Scenario: Filter by Scenario Tag Keywords in the Scenarios List
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| @TA-3001 |
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                  |
		| Successful Submitting a Contact Us Inquiry |

Rule: Filter by Status in Scenarios List

Scenario: Filter by Status Passed in the Scenarios List
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| Inquiry  |
	And I enable the status prefilter Passed in the Filter Bar
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                      |
		| Successful Submitting a Contact Us Inquiry     |
		| Successful Submitting a Registration Inquiry   |
		| Successful Resubmitting a Registration Inquiry |

Scenario: Filter by Status Incomplete in the Scenarios List
	Given I have navigated to the Scenarios List
	When I enable the status prefilter Incomplete in the Filter Bar
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                        |
		| Successful Resubmitting a Contact Us Inquiry     |
		| Unsuccessful User Login with Invalid Credentials |

Scenario: Filter by Status Failed in the Scenarios List
	Given I have navigated to the Scenarios List
	When I enable the status prefilter Failed in the Filter Bar
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                    |
		| Successful User Login with Valid Credentials |
		| Successful Canceling a Registration Inquiry  |

Scenario: Filter by Status Skipped in the Scenarios List
	Given I have navigated to the Scenarios List
	When I enable the status prefilter Skipped in the Filter Bar
	Then I should have following number of visible objects in the Scenarios List
		| Scenarios |
		| 1         |

Rule: Clear Keywords and Status Filters in Scenarios List

Scenario: Clear All PreFilters in the Scenarios List
	Given I have navigated to the Scenarios List
	When I enable all status prefilters in the Filter Bar
	Then I should have all status prefilters enabled in the Filter Bar
	When I clear all filters in the Filter Bar
	Then I should have all status prefilters disabled in the Filter Bar
	And I should have following number of visible objects in the Scenarios List
		| Scenarios |
		| 10        |

Scenario: Clear Keyword Filter in the Scenarios List
	Given I have navigated to the Scenarios List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| Contact Us |
	Then I should have a predefined keyword filter in the Filter Bar
	When I clear all filters in the Filter Bar
	Then I should have an emtpy keyword filter in the Filter Bar
	And I should have following number of visible objects in the Scenarios List
		| Scenarios |
		| 10        |

Rule: Sort by Columns in Scenarios List

Scenario: Sort by Scenario Column in the Scenarios List
	Given I have navigated to the Scenarios List
	When I enable the status prefilter Passed in the Filter Bar
	And I sort the scenarios by Scenario column in the Scenarios List
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                           |
		| Ordering Coffee Confirmation Notification           |
		| Ordering Multiple Coffees Confirmation Notification |
		| Successful Resubmitting a Registration Inquiry      |
		| Successful Submitting a Contact Us Inquiry          |
		| Successful Submitting a Registration Inquiry        |

Scenario: Sort by Order Column in the Scenarios List
	Given I have navigated to the Scenarios List
	When I enable the status prefilter Failed in the Filter Bar
	And I enable the status prefilter Incomplete in the Filter Bar
	And I sort the scenarios by Order column in the Scenarios List
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                        |
		| Successful Canceling a Registration Inquiry      |
		| Successful User Login with Valid Credentials     |
		| Unsuccessful User Login with Invalid Credentials |
		| Successful Resubmitting a Contact Us Inquiry     |

Scenario: Sort by Duration Column in the Scenarios List
	Given I have navigated to the Scenarios List
	When I enable the status prefilter Failed in the Filter Bar
	And I enable the status prefilter Incomplete in the Filter Bar
	And I sort the scenarios by Duration column in the Scenarios List
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                        |
		| Successful Resubmitting a Contact Us Inquiry     |
		| Successful Canceling a Registration Inquiry      |
		| Successful User Login with Valid Credentials     |
		| Unsuccessful User Login with Invalid Credentials |

Scenario: Sort by Status Column in the Scenarios List
	Given I have navigated to the Scenarios List
	When I enable the status prefilter Failed in the Filter Bar
	And I enable the status prefilter Incomplete in the Filter Bar
	And I sort the scenarios by Status column in the Scenarios List
	Then I should have following visible objects in the Scenarios List
		| Scenarios                                        |
		| Successful User Login with Valid Credentials     |
		| Successful Canceling a Registration Inquiry      |
		| Successful Resubmitting a Contact Us Inquiry     |
		| Unsuccessful User Login with Invalid Credentials |