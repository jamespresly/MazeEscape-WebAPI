Feature: Player

Enpoint usage for navigating a maze as a player

@ignore
Scenario: Get Player Info without mazeToken
	Given the MazeEscape client is running		
	When I make a POST request to:/mazes/player with body:{"mazeToken": ""}
	Then the status code is:BadRequest
	And the response message is:mazeToken is required