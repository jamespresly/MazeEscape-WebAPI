﻿Feature: Player

Enpoint usage for navigating a maze as a player

Scenario: Get Player Info without mazeToken
	Given the MazeEscape client is running		
	When I make a POST request to:/mazes/player with body:{"mazeToken": ""}
	Then the status code is:BadRequest
	And the response message is:mazeToken is required

@ignore
Scenario: Move player through the smallest maze
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "minmaze"}}
	Then the status code is:Created
	And the response data is an object which contains non-null value:mazeToken
	
	