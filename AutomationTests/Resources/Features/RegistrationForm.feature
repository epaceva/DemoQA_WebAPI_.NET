@UI @Smoke
Feature: UI form for registration

  Scenario: Fill in form with predefined data
    Given I navigate to the practice form page
    When I fill the registration form with "default" data
    And I submit the form
    Then I should see the confirmation modal
    And I verify the "default" data in the modal
    When I close the modal
    Then the modal should disappear

  Scenario: Fill in form with Random User
    Given I navigate to the practice form page
    When I fill the registration form with "random" data
    And I submit the form
    Then I should see the confirmation modal
    And I verify the "random" data in the modal
    When I close the modal
    Then the modal should disappear

  Scenario: Attempt to submit with empty required fields
    Given I navigate to the practice form page
    When I submit the empty form
    Then I should see validation errors for required fields

  Scenario Outline: Successfully submit the form with multiple full data sets
    Given I navigate to the practice form page
    When I fill the complete form with data: <FName>, <LName>, <Email>, <Gender>, <Mobile>, <Day>, <Month>, <Year>, <Subject>, <Hobby>, <Address>, <State>, <City>
    And I submit the form
    Then I should see the confirmation modal
    And I verify the complete modal data matches: <FName>, <LName>, <Email>, <Gender>, <Mobile>, <Day>, <Month>, <Year>, <Subject>, <Hobby>, <Address>, <State>, <City>
    When I close the modal
    Then the modal should disappear

    Examples:
      | FName   | LName       | Email               | Gender   | Mobile       | Day | Month    | Year  |  Subject  | Hobby     | Address         | State       | City    |
      | "Peter" | "Petrov"    | "peter.p@test.com"  | "Male"   | "0888111222" | "01"| "January"| "1990"| "Maths"   | "Sports"  | "1 St"          | "NCR"       | "Delhi" |
      | "Maria" | "Georgieva" | "maria.g@test.com"  | "Female" | "0888333444" | "15"| "May"    | "1995"| "Physics" | "Reading" | "123 Main St"   | "Rajasthan" | "Jaipur"|