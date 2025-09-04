Feature: Steps
	As a LivingDoc User
	I want to filter the steps list
	So that I select validate specific test results

Rule: Filter by Keywords in Steps List

Scenario: Filter with Unknown Keywords in the Steps List
	Given I have navigated to the Steps List
	When I filter by following keywords in the Filter Bar
		| Keywords  |
		| Gibberish |
	Then I should have following number of visible objects in the Steps List
		| Steps |
		| 0     |

Scenario: Filter by Multiple Keywords in the Steps List
	Given I have navigated to the Steps List
	When I filter by following keywords in the Filter Bar
		| Keywords                  |
		| message have notification |
	Then I should have following visible objects in the Steps List
		| Steps                                                               |
		| Then I should have <Price> in the confirmation notification message |
		| Then I should have a confirmation notification message              |

Scenario: Filter by Uppercased Keywords in the Steps List
	Given I have navigated to the Steps List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| USER LOGIN |
	Then I should have following visible objects in the Steps List
		| Steps                                                |
		| Given I have logged in with invalid user credentials |

Scenario: Filter by Lowercased Keywords in the Steps List
	Given I have navigated to the Steps List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| user login |
	Then I should have following visible objects in the Steps List
		| Steps                                                |
		| Given I have logged in with invalid user credentials |

Scenario: Filter by Feature Tag Keywords in the Steps List
	Given I have navigated to the Steps List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| @TA-1000 |
	Then I should have following visible objects in the Steps List
		| Steps                                                 |
		| Then I should be redirected to the Home page          |
		| Then I should have an error message on the Login page |
		| Given I have logged in with invalid user credentials  |

Scenario: Filter by Scenario Tag Keywords in the Steps List
	Given I have navigated to the Steps List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| @TA-3003 |
	Then I should have following visible objects in the Steps List
		| Steps                                  |
		| And I resubmit the Contact Us formular |

Rule: Filter by Status in Steps List

Scenario: Filter by Status Passed in the Steps List
	Given I have navigated to the Steps List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| formular |
	And I enable the status prefilter Passed in the Filter Bar
	Then I should have following visible objects in the Steps List
		| Steps                                                |
		| When I complete and submit the Registration formular |
		| And I complete and submit the Registration formular |

Scenario: Filter by Status Incomplete in the Steps List
	Given I have navigated to the Steps List
	When I enable the status prefilter Incomplete in the Filter Bar
	Then I should have following visible objects in the Steps List
		| Steps                                                 |
		| And I resubmit the Contact Us formular                |
		| Then I should have an error message on the Login page |

Scenario: Filter by Status Failed in the Steps List
	Given I have navigated to the Steps List
	When I enable the status prefilter Failed in the Filter Bar
	Then I should have following visible objects in the Steps List
		| Steps                                                |
		| Then I should be redirected to the Home page         |
		| When I complete and cancel the Registration formular |

Scenario: Filter by Status Skipped in the Steps List
	Given I have navigated to the Steps List
	When I enable the status prefilter Skipped in the Filter Bar
	Then I should have following number of visible objects in the Steps List
		| Steps |
		| 4     |

Rule: Clear Keywords and Status Filters in Steps List

Scenario: Clear All PreFilters in the Steps List
	Given I have navigated to the Steps List
	When I enable all status prefilters in the Filter Bar
	Then I should have all status prefilters enabled in the Filter Bar
	When I clear all filters in the Filter Bar
	Then I should have all status prefilters disabled in the Filter Bar
	And I should have following number of visible objects in the Steps List
		| Steps |
		| 15    |

Scenario: Clear Keyword Filter in the Steps List
	Given I have navigated to the Steps List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| Contact Us |
	Then I should have a predefined keyword filter in the Filter Bar
	When I clear all filters in the Filter Bar
	Then I should have an emtpy keyword filter in the Filter Bar
	And I should have following number of visible objects in the Steps List
		| Steps |
		| 15    |

Rule: Sort by Columns in Steps List

Scenario: Sort by Step Column in the Steps List
	Given I have navigated to the Steps List
	When I enable the status prefilter Failed in the Filter Bar
	And I enable the status prefilter Incomplete in the Filter Bar
	And I sort the steps by Step column in the Steps List
	Then I should have following visible objects in the Steps List
		| Steps                                                 |
		| And I resubmit the Contact Us formular                |
		| Then I should be redirected to the Home page          |
		| Then I should have an error message on the Login page |
		| When I complete and cancel the Registration formular  |

Scenario: Sort by Status Column in the Steps List
	Given I have navigated to the Steps List
	When I enable the status prefilter Failed in the Filter Bar
	And I enable the status prefilter Incomplete in the Filter Bar
	And I sort the steps by Status column in the Steps List
	Then I should have following visible objects in the Steps List
		| Steps                                                 |
		| Then I should be redirected to the Home page          |
		| When I complete and cancel the Registration formular  |
		| And I resubmit the Contact Us formular                |
		| Then I should have an error message on the Login page |