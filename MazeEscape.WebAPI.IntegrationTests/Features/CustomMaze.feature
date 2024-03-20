Feature: CustomMaze

Enpoint usage for creating a custom maze


Scenario: Create maze from custom string
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=custom with body:{"custom": {"mazeText": "+E+\n+ +\n+S+\n+++"}}
	Then the status code is:Created
	And the response data is an object which contains non-null value:mazeToken

Scenario: Error Scenario: Create maze from custom string with empty mazeText
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=custom with body:{"custom": {"mazeText": ""}}
	Then the status code is:BadRequest
	And the response message contains:mazeText is required

Scenario: Error Scenario: Create maze from custom string with empty invalid mazeText
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=custom with body:{"custom": {"mazeText": "abcd"}}
	Then the status code is:BadRequest
	And the response message contains:mazeText format is incorrect