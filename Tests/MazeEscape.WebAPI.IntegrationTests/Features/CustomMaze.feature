Feature: CustomMaze

Enpoint usage for creating a custom maze

Scenario: Create maze from custom string
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"custom", "custom": {"mazeText": "+E+\n+ +\n+S+\n+++"}}
	Then the status code is:Created
	And the response data contains a non-null variable named:mazeToken
	And the response data contains an int named:width with value:3
	And the response data contains an int named:height with value:4

Scenario: Error Scenario: Create maze from custom string with empty mazeText
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"custom", "custom": {"mazeText": ""}}
	Then the status code is:BadRequest
	And the response contains error message:mazeText is required

Scenario: Error Scenario: Create maze from custom string with empty invalid mazeText
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"custom", "custom": {"mazeText": "abcd"}}
	Then the status code is:BadRequest
	And the response contains error message:mazeText format is incorrect