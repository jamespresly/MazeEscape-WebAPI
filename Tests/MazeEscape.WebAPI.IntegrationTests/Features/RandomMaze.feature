Feature: RandomMaze

Enpoint usage for creating a random maze

Scenario: Create a random maze 
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"random", "random": {"width": 10, "height": 10}}
	Then the status code is:Created
	And the response data contains a non-null variable named:mazeToken
	And the response data contains an int named:width with value:10
	And the response data contains an int named:height with value:10	

Scenario: Error Scenario: Create a random maze with missing height
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"random", "random": {"width": 10 }}
	Then the status code is:BadRequest
	And the response contains error message:width and height are required parameters

Scenario: Error Scenario: Create a random maze with height and width out of range
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"random", "random": {"width": 51, "height": 51 }}
	Then the status code is:BadRequest
	And the response contains error message:width and height must be between 10 and 50