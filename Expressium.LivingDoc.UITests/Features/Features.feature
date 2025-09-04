Feature: Features
	As a LivingDoc User
	I want to filter the features list
	So that I select validate specific test results

Rule: Filter by Keywords in Features List

Scenario: Filter with Unknown Keywords in the Features List
	Given I have navigated to the Features List
	When I filter by following keywords in the Filter Bar
		| Keywords  |
		| Gibberish |
	Then I should have following number of visible objects in the Features List
		| Features |
		| 0        |

Scenario: Filter by Multiple Keywords in the Features List
	Given I have navigated to the Features List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| contact us |
	Then I should have following visible objects in the Features List
		| Features   |
		| Contact Us |

Scenario: Filter by Uppercased Keywords in the Features List
	Given I have navigated to the Features List
	When I filter by following keywords in the Filter Bar
		| Keywords     |
		| REGISTRATION |
	Then I should have following visible objects in the Features List
		| Features     |
		| Registration |

Scenario: Filter by Lowercased Keywords in the Features List
	Given I have navigated to the Features List
	When I filter by following keywords in the Filter Bar
		| Keywords     |
		| registration |
	Then I should have following visible objects in the Features List
		| Features     |
		| Registration |

Scenario: Filter by Feature Tag Keywords in the Features List
	Given I have navigated to the Features List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| @TA-1000 |
	Then I should have following visible objects in the Features List
		| Features |
		| Login    |

#Scenario: Filter by Scenario Tag Keywords in the Features List
#	Given I have navigated to the Features List
#	When I filter by following keywords in the Filter Bar
#		| Keywords |
#		| @TA-3001 |
#	Then I should have following visible objects in the Features List
#		| Features   |
#		| Contact Us |

Rule: Filter by Status in Features List

Scenario: Filter by Status Passed in the Features List
	Given I have navigated to the Features List
	When I enable the status prefilter Passed in the Filter Bar
	Then I should have following visible objects in the Features List
		| Features |
		| Orders   |

Scenario: Filter by Status Incomplete in the Features List
	Given I have navigated to the Features List
	When I enable the status prefilter Incomplete in the Filter Bar
	Then I should have following number of visible objects in the Features List
		| Features |
		| 1        |

Scenario: Filter by Status Failed in the Features List
	Given I have navigated to the Features List
	When I enable the status prefilter Failed in the Filter Bar
	Then I should have following visible objects in the Features List
		| Features     |
		| Login        |
		| Registration |

Scenario: Filter by Status Skipped in the Features List
	Given I have navigated to the Features List
	When I enable the status prefilter Skipped in the Filter Bar
	Then I should have following number of visible objects in the Features List
		| Features |
		| 0        |

Rule: Clear Keywords and Status Filters in Features List

Scenario: Clear All PreFilters in the Features List
	Given I have navigated to the Features List
	When I enable all status prefilters in the Filter Bar
	Then I should have all status prefilters enabled in the Filter Bar
	When I clear all filters in the Filter Bar
	Then I should have all status prefilters disabled in the Filter Bar
	And I should have following number of visible objects in the Features List
		| Features |
		| 4        |

Scenario: Clear Keyword Filter in the Features List
	Given I have navigated to the Features List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| Contact Us |
	Then I should have a predefined keyword filter in the Filter Bar
	When I clear all filters in the Filter Bar
	Then I should have an emtpy keyword filter in the Filter Bar
	And I should have following number of visible objects in the Features List
		| Features |
		| 4        |

Rule: Sort by Columns in Features List

Scenario: Sort by Feature Column in the Features List
	Given I have navigated to the Features List
	When I sort the features by Feature column in the Features List
	Then I should have following visible objects in the Features List
		| Features     |
		| Registration |
		| Orders       |
		| Login        |
		| Contact Us   |

Scenario: Sort by Completion Column in the Features List
	Given I have navigated to the Features List
	When I sort the features by Completion column in the Features List
	Then I should have following visible objects in the Features List
		| Features     |
		| Login        |
		| Contact Us   |
		| Registration |
		| Orders       |

Scenario: Sort by Duration Column in the Features List
	Given I have navigated to the Features List
	When I sort the features by Duration column in the Features List
	Then I should have following visible objects in the Features List
		| Features     |
		| Orders       |
		| Login        |
		| Contact Us   |
		| Registration |

Scenario: Sort by Status Column in the Features List
	Given I have navigated to the Features List
	When I sort the features by Status column in the Features List
	Then I should have following visible objects in the Features List
		| Features     |
		| Login        |
		| Registration |
		| Contact Us   |
		| Orders       |