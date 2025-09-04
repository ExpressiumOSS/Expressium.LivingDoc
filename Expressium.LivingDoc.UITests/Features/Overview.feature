Feature: Overview
	As a LivingDoc User
	I want to filter the overview list
	So that I select validate specific test results

Rule: Filter by Keywords in Overview List

Scenario: Filter with Unknown Keywords in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords  |
		| Gibberish |
	Then I should have following number of visible objects in the Overview
		| Folders | Features | Scenarios |
		| 0       | 0        | 0         |

Scenario: Filter by Multiple Keywords in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords                          |
		| inquiry resubmitting registration |
	Then I should have following visible objects in the Overview
		| Folders  | Features     | Scenarios                                      |
		| Features | Registration | Successful Resubmitting a Registration Inquiry |

Scenario: Filter by Uppercased Keywords in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| USER LOGIN |
	Then I should have following visible objects in the Overview
		| Folders                  | Features | Scenarios                                        |
		| Features                 |          |                                                  |
		| Features\\Authentication | Login    | Successful User Login with Valid Credentials     |
		|                          |          | Unsuccessful User Login with Invalid Credentials |

Scenario: Filter by Lowercased Keywords in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| user login |
	Then I should have following visible objects in the Overview
		| Folders                  | Features | Scenarios                                        |
		| Features                 |          |                                                  |
		| Features\\Authentication | Login    | Successful User Login with Valid Credentials     |
		|                          |          | Unsuccessful User Login with Invalid Credentials |

Scenario: Filter by Feature Tag Keywords in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| @TA-1000 |
	Then I should have following visible objects in the Overview
		| Folders                  | Features | Scenarios                                        |
		| Features                 |          |                                                  |
		| Features\\Authentication | Login    | Successful User Login with Valid Credentials     |
		|                          |          | Unsuccessful User Login with Invalid Credentials |

Scenario: Filter by Scenario Tag Keywords in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| @TA-3001 |
	Then I should have following visible objects in the Overview
		| Folders             | Features   | Scenarios                                  |
		| Features            |            |                                            |
		| Features\\Inquiries | Contact Us | Successful Submitting a Contact Us Inquiry |

Rule: Filter by Status in Overview List

Scenario: Filter by Status Passed in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords |
		| Inquiry  |
	And I enable the status prefilter Passed in the Filter Bar
	Then I should have following visible objects in the Overview
		| Folders             | Features     | Scenarios                                      |
		| Features            |              |                                                |
		| Features\\Inquiries | Contact Us   | Successful Submitting a Contact Us Inquiry     |
		|                     | Registration | Successful Submitting a Registration Inquiry   |
		|                     |              | Successful Resubmitting a Registration Inquiry |

Scenario: Filter by Status Incomplete in Overview List
	Given I have navigated to the Overview List
	When I enable the status prefilter Incomplete in the Filter Bar
	Then I should have following visible objects in the Overview
		| Folders                  | Features   | Scenarios                                        |
		| Features                 |            |                                                  |
		| Features\\Authentication | Login      | Unsuccessful User Login with Invalid Credentials |
		| Features\\Inquiries      | Contact Us | Successful Resubmitting a Contact Us Inquiry     |

Scenario: Filter by Status Failed in Overview List
	Given I have navigated to the Overview List
	When I enable the status prefilter Failed in the Filter Bar
	Then I should have following visible objects in the Overview
		| Folders                  | Features     | Scenarios                                    |
		| Features                 |              |                                              |
		| Features\\Authentication | Login        | Successful User Login with Valid Credentials |
		|                          | Registration | Successful Canceling a Registration Inquiry  |

Scenario: Filter by Status Skipped in Overview List
	Given I have navigated to the Overview List
	When I enable the status prefilter Skipped in the Filter Bar
	Then I should have following number of visible objects in the Overview
		| Folders | Features | Scenarios |
		| 2       | 1        | 1         |

Rule: Filter by Levels in Overview List

Scenario: Filter Zero Level Scenarios in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords     |
		| Registration |
	Then I should have following visible objects in the Overview
		| Folders  | Features     | Scenarios                                      |
		| Features | Registration | Successful Submitting a Registration Inquiry   |
		|          |              | Successful Canceling a Registration Inquiry    |
		|          |              | Successful Resubmitting a Registration Inquiry |

Scenario: Filter One Level Scenarios in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords       |
		| Authentication |
	Then I should have following visible objects in the Overview
		| Folders                  | Features | Scenarios                                        |
		| Features                 |          |                                                  |
		| Features\\Authentication | Login    | Successful User Login with Valid Credentials     |
		|                          |          | Unsuccessful User Login with Invalid Credentials |

Scenario: Filter Two Level Scenarios in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| Contact Us |
	Then I should have following visible objects in the Overview
		| Folders             | Features   | Scenarios                                    |
		| Features            |            |                                              |
		| Features\\Inquiries | Contact Us | Successful Submitting a Contact Us Inquiry   |
		|                     |            | Unsuccessful Submitting a Contact Us Inquiry |
		|                     |            | Successful Resubmitting a Contact Us Inquiry |

Scenario: Filter Multi Level Scenarios in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords     |
		| confirmation |
	Then I should have following visible objects in the Overview
		| Folders                           | Features | Scenarios                                           |
		| Features                          |          |                                                     |
		| Features\\Products                |          |                                                     |
		| Features\\Products\\Notifications | Orders   | Ordering Coffee Confirmation Notification           |
		|                                   |          | Ordering Multiple Coffees Confirmation Notification |

Rule: Filter by Collapse & Expand Features in Overview List

Scenario: Filter by Collapse All Features in Overview List
	Given I have navigated to the Overview List
	When I select the collapse all features in the Overview
	Then I should have following number of visible objects in the Overview
		| Folders | Features | Scenarios |
		| 5       | 4        | 0         |

Scenario: Filter by Expand All Features in Overview List
	Given I have navigated to the Overview List
	When I select the collapse all features in the Overview
	And I select the expand all features in the Overview
	Then I should have following number of visible objects in the Overview
		| Folders | Features | Scenarios |
		| 5       | 4        | 10        |

Scenario: Filter by Collapse a Feature in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| Contact Us |
	And I select the collapse a feature in the Overview
	Then I should have following visible objects in the Overview
		| Folders             | Features   | Scenarios |
		| Features            |            |           |
		| Features\\Inquiries | Contact Us |           |

Scenario: Filter by Expand a Feature in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| Contact Us |
	And I select the collapse a feature in the Overview
	And I select the expand a feature in the Overview
	Then I should have following visible objects in the Overview
		| Folders             | Features   | Scenarios                                    |
		| Features            |            |                                              |
		| Features\\Inquiries | Contact Us | Successful Submitting a Contact Us Inquiry   |
		|                     |            | Unsuccessful Submitting a Contact Us Inquiry |
		|                     |            | Successful Resubmitting a Contact Us Inquiry |

Rule: Clear Keywords and Status Filters in Overview List

Scenario: Clear All PreFilters in Overview List
	Given I have navigated to the Overview List
	When I enable all status prefilters in the Filter Bar
	Then I should have all status prefilters enabled in the Filter Bar
	When I clear all filters in the Filter Bar
	Then I should have all status prefilters disabled in the Filter Bar
	And I should have following number of visible objects in the Overview
		| Folders | Features | Scenarios |
		| 5       | 4        | 10        |

Scenario: Clear Keyword Filter in Overview List
	Given I have navigated to the Overview List
	When I filter by following keywords in the Filter Bar
		| Keywords   |
		| Contact Us |
	Then I should have a predefined keyword filter in the Filter Bar
	When I clear all filters in the Filter Bar
	Then I should have an emtpy keyword filter in the Filter Bar
	And I should have following number of visible objects in the Overview
		| Folders | Features | Scenarios |
		| 5       | 4        | 10        |

