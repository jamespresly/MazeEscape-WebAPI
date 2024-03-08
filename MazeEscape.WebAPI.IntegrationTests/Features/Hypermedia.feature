Feature: Hypermedia

Test links and actions returned from endpoints


Scenario: Get mazes root
	Given the MazeEscape client is running
	When I make a GET request to:/mazes
	Then the status code is:OK
	And the response contains the following:links
	| description            | href           | method | body |
	| get-mazes-root         | /mazes         | GET    |      |
	| get-mazes-presets-list | /mazes/presets | GET    |      |
	And the response contains the following:actions
	| description             | href                     | method | body                                     |
	| create-maze-from-preset | /mazes?createMode=preset | POST   | {"preset":{"presetName":"{presetName}"}} |
	| create-maze-from-text   | /mazes?createMode=custom | POST   | {"custom":{"mazeText":"{mazeText}"}}     |
	| create-random-maze      | /mazes?createMode=random | POST   | {"random":{"width":null,"height":null}}  |

Scenario: Get mazes presets
	Given the MazeEscape client is running
	When I make a GET request to:/mazes/presets
	Then the status code is:OK
	And the response contains the following:links
	| description    | href   | method | body |
	| get-mazes-root | /mazes | GET    |      |
	And the response contains the following:actions
	| description             | href                     | method | body                                     |
	| create-maze-from-preset | /mazes?createMode=preset | POST   | {"preset":{"presetName":"{presetName}"}} |
	
Scenario: Post mazes
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "spiral"}}
	Then the status code is:Created
	And the response contains the following:links
	| description    | href          | method | body                         |
	| get-mazes-root | /mazes        | GET    |                              |
	And the response contains the following:actions
	| description | href          | method | body                        |
	| post-player | /mazes/player | POST   | {"mazeToken":"{mazeToken}"} |


Scenario: Post Player
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "spiral"}}
	And I save the mazeToken
	And I make a POST request to:/mazes/player with saved mazeToken and body:{"mazeToken":"{mazeToken}"}
	Then the status code is:OK
	And the response contains the following:links
	| description    | href   | method | body |
	| get-mazes-root | /mazes | GET    |      |
	And the response contains the following:actions
	| description         | href                               | method | body                        |
	| post-player         | /mazes/player                      | POST   | {"mazeToken":"{mazeToken}"} |
	| player-turn-left    | /mazes/player?playerMove=turnLeft  | POST   | {"mazeToken":"{mazeToken}"} |
	| player-turn-right   | /mazes/player?playerMove=turnRight | POST   | {"mazeToken":"{mazeToken}"} |
	| player-move-forward | /mazes/player?playerMove=forward   | POST   | {"mazeToken":"{mazeToken}"} |

Scenario: No actions available after maze has been escaped
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "minmaze"}}
	And I save the mazeToken
	And I make a POST request to:/mazes/player with saved mazeToken and body:{"mazeToken":"{mazeToken}"}
	And I save the mazeToken
	And I make a POST request to:/mazes/player?playerMove=forward with saved mazeToken and body:{"mazeToken":"{mazeToken}"}
	And I save the mazeToken
	And I make a POST request to:/mazes/player?playerMove=forward with saved mazeToken and body:{"mazeToken":"{mazeToken}"}
	Then the response message is:You escaped
	And the response contains the following:links
	| description    | href   | method | body |
	| get-mazes-root | /mazes | GET    |      |
	And the response contains the following:actions
	| description         | href                               | method | body                        |


