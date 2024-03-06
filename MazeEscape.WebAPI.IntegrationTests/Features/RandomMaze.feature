Feature: RandomMaze

Enpoint usage for creating a random maze

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