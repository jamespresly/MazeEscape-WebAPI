Feature: Player

Enpoint usage for navigating a maze as a player

Scenario: Error Scenario: Get Player Info without mazeToken
	Given the MazeEscape client is running		
	When I make a POST request to:/mazes/player with body:{"mazeToken": ""}
	Then the status code is:BadRequest
	And the response contains error message:mazeToken is required

Scenario: Move player through the smallest maze
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "minmaze"}}
	And I save the mazeToken
	And I make a POST request to:/mazes/player with saved mazeToken and body:{"mazeToken":"{mazeToken}"}
	And I save the mazeToken
	And I make a POST request to:/mazes/player?playerMove=forward with saved mazeToken and body:{"mazeToken":"{mazeToken}"}
	And I save the mazeToken
	And I make a POST request to:/mazes/player?playerMove=forward with saved mazeToken and body:{"mazeToken":"{mazeToken}"}
	Then the response message contains:You escaped
	
	