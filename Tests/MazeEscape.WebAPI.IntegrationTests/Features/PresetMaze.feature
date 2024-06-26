﻿Feature: PresetMaze

Enpoint usage for creating a maze from presets

Scenario: Get Maze preset list
	Given the MazeEscape client is running
	When I make a GET request to:/mazes/presets
	Then the status code is:OK
	And the response data is an array which contains value:spiral

Scenario: Create maze from preset
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"preset", "preset": {"presetName": "spiral"}}
	Then the status code is:Created
	And the response data contains a non-null variable named:mazeToken
	And the response data contains a non-null variable named:width
	And the response data contains a non-null variable named:height

Scenario: Error Scenario: Create maze from empty preset
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"preset", "preset": {"presetName": ""}}
	Then the status code is:BadRequest
	And the response contains error message:presetName is required

Scenario: Error Scenario: Create maze from a non-existent preset
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"preset", "preset": {"presetName": "doesntExist"}}
	Then the status code is:NotFound
	And the response contains error message:Preset:doesntExist not found