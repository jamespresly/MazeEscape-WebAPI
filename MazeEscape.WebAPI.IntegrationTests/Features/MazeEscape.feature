
Feature: MazeEscape

MazeEscape endpoints for creating the maze and moving the player around the maze

	#preset maze

Scenario: Get Maze preset list
	Given the MazeEscape client is running
	When I make a GET request to:/mazes/presets
	Then the status code is:OK
	And the response data is an array which contains value:spiral

Scenario: Create maze from preset
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "spiral"}}
	Then the status code is:Created
	And the response data is an object which contains non-null value:mazeToken

Scenario: Create maze from empty preset
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": ""}}
	Then the status code is:BadRequest
	And the response message is:presetName is required

Scenario: Create maze from a non-existent preset
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "doesntExist"}}
	Then the status code is:NotFound
	And the response message is:Preset:doesntExist not found

	#custom maze
@ignore
Scenario: Create maze from custom string
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=custom with body:{"custom": {"mazeText": "+E+\n+ +\n+S+\n+++"}}
	Then the status code is:Created
	And the response data is an object which contains non-null value:mazeToken
@ignore
Scenario: Create maze from custom string with empty mazeText
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=custom with body:{"custom": {"mazeText": ""}}
	Then the status code is:BadRequest
	And the response message is:mazeText is required
@ignore
Scenario: Create maze from custom string with empty invalid mazeText
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=custom with body:{"custom": {"mazeText": "abcd"}}
	Then the status code is:BadRequest
	And the response message is:mazeText format is incorrect

	#random maze
@ignore
Scenario: Create a random maze 
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=random with body:{"random": {"width": 10, "height": 10}}
	Then the status code is:Created
	And the response data is an object which contains non-null value:mazeToken
@ignore
Scenario: Create a random maze with missing height
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=random with body:{"random": {"width": 10 }}
	Then the status code is:BadRequest
	And the response message is:"width and height are required"
@ignore
Scenario: Create a random maze with height and width out of range
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=random with body:{"random": {"width": 101, "height": 101 }}
	Then the status code is:BadRequest
	And the response message is:"width and height must be between 3 and 100"

	#player
@ignore
Scenario: Get Player Info without mazeToken
	Given the MazeEscape client is running		
	When I make a POST request to:/mazes/player with body:{"mazeToken": ""}
	Then the status code is:BadRequest
	And the response message is:mazeToken is required



