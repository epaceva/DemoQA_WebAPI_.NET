Feature: Create User API

  @API
  Scenario: Create a new user successfully
    When I send POST Request to create user
    Then I get response with status code 201
    And I verify "job" and "name" in the response
    And I verify "id" and "createdAt" are generated