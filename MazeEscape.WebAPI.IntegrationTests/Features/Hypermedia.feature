Feature: Hypermedia

Test links and actions returned from endpoints


Scenario: Get mazes root
	Given the MazeEscape client is running
	When I make a GET request to:/mazes
	Then the status code is:OK
	And the response contains the following hypermedia array:actions with values:
	| description             | href   | method | body                                                                      |
	| create-maze-from-preset | /mazes | POST   | {"createMode":"preset","preset":{"presetName":"{presetName}"}}           |
	| create-maze-from-text   | /mazes | POST   | {"createMode":"custom","custom":{"mazeText":"{mazeText}"}}               |
	| create-random-maze      | /mazes | POST   | {"createMode":"random","random":{"width":"{width}","height":"{height}"}} |
	And the response contains the following hypermedia array:links with values:
	| description            | href           | method | body |
	| get-mazes-root         | /mazes         | GET    |      |
	| get-mazes-presets-list | /mazes/presets | GET    |      |

Scenario: Get mazes presets
	Given the MazeEscape client is running
	When I make a GET request to:/mazes/presets
	Then the status code is:OK
	And the response contains the following hypermedia array:actions with values:
	| description             | href   | method | body                                                            |
	| create-maze-from-preset | /mazes | POST   | {"createMode":"preset","preset":{"presetName":"{presetName}"}} |
	And the response contains the following hypermedia array:links with values:
	| description    | href   | method | body |
	| get-mazes-root | /mazes | GET    |      |

Scenario: Error Scenario: Create maze from empty preset - Returns root hypermedia
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": ""}}
	Then the status code is:BadRequest
	And the response contains the following hypermedia array:actions with values:
	| description             | href   | method | body                                                                      |
	| create-maze-from-preset | /mazes | POST   | {"createMode":"preset","preset":{"presetName":"{presetName}"}}           |
	| create-maze-from-text   | /mazes | POST   | {"createMode":"custom","custom":{"mazeText":"{mazeText}"}}               |
	| create-random-maze      | /mazes | POST   | {"createMode":"random","random":{"width":"{width}","height":"{height}"}} |
	And the response contains the following hypermedia array:links with values:
	| description            | href           | method | body |
	| get-mazes-root         | /mazes         | GET    |      |
	| get-mazes-presets-list | /mazes/presets | GET    |      |

Scenario: Error Scenario: Create maze from a non-existent preset - Returns root hypermedia
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "doesntExist"}}
	Then the status code is:NotFound
	And the response contains the following hypermedia array:actions with values:
	| description             | href   | method | body                                                                      |
	| create-maze-from-preset | /mazes | POST   | {"createMode":"preset","preset":{"presetName":"{presetName}"}}           |
	| create-maze-from-text   | /mazes | POST   | {"createMode":"custom","custom":{"mazeText":"{mazeText}"}}               |
	| create-random-maze      | /mazes | POST   | {"createMode":"random","random":{"width":"{width}","height":"{height}"}} |
	And the response contains the following hypermedia array:links with values:
	| description            | href           | method | body |
	| get-mazes-root         | /mazes         | GET    |      |
	| get-mazes-presets-list | /mazes/presets | GET    |      |
	
Scenario: Post mazes
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "spiral"}}
	Then the status code is:Created
	And the response contains the following hypermedia array:actions with values:
	| description | href          | method | body                        |
	| post-player | /mazes/player | POST   | {"mazeToken":"{mazeToken}"} |
	And the response contains the following hypermedia array:links with values:
	| description    | href          | method | body                         |
	| get-mazes-root | /mazes        | GET    |                              |

Scenario: Error Scenario: Get Player Info without mazeToken  - Still returns hypermedia
	Given the MazeEscape client is running		
	When I make a POST request to:/mazes/player with body:{"mazeToken": ""}
	Then the status code is:BadRequest
	And the response contains the following hypermedia array:actions with values:
	| description         | href          | method | body                                                 |
	| post-player         | /mazes/player | POST   | {"mazeToken":"{mazeToken}"}                          |
	| player-turn-left    | /mazes/player | POST   | {"mazeToken":"{mazeToken}","playerMove":"turnLeft"}  |
	| player-turn-right   | /mazes/player | POST   | {"mazeToken":"{mazeToken}","playerMove":"turnRight"} |
	| player-move-forward | /mazes/player | POST   | {"mazeToken":"{mazeToken}","playerMove":"forward"}   |
	And the response contains the following hypermedia array:links with values:
	| description    | href          | method | body                         |
	| get-mazes-root | /mazes        | GET    |                              |

Scenario: Post Player
	Given the MazeEscape client is running
	When I make a POST request to:/mazes?createMode=preset with body:{"preset": {"presetName": "spiral"}}
	And I save the mazeToken
	And I make a POST request to:/mazes/player with saved mazeToken and body:{"mazeToken":"{mazeToken}"}
	Then the status code is:OK
	And the response contains the following hypermedia array:actions with values:
	| description         | href          | method | body                                                 |
	| post-player         | /mazes/player | POST   | {"mazeToken":"{mazeToken}"}  |
	| player-turn-left    | /mazes/player | POST   | {"mazeToken":"{mazeToken}","playerMove":"turnLeft"}  |
	| player-turn-right   | /mazes/player | POST   | {"mazeToken":"{mazeToken}","playerMove":"turnRight"} |
	| player-move-forward | /mazes/player | POST   | {"mazeToken":"{mazeToken}","playerMove":"forward"}   |
	And the response contains the following hypermedia array:links with values:
	| description    | href   | method | body |
	| get-mazes-root | /mazes | GET    |      |

Scenario: No actions available after maze has been escaped
	Given the MazeEscape client is running
	When I make a POST request to:/mazes with body:{"createMode":"preset", "preset": {"presetName": "minmaze"}}
	And I save the mazeToken
	And I make a POST request to:/mazes/player with saved mazeToken and body:{"mazeToken":"{mazeToken}"}
	And I save the mazeToken
	And I make a POST request to:/mazes/player with saved mazeToken and body:{"mazeToken":"{mazeToken}","playerMove":"forward"}
	And I save the mazeToken
	And I make a POST request to:/mazes/player with saved mazeToken and body:{"mazeToken":"{mazeToken}","playerMove":"forward"}
	Then the response message contains:You escaped
	Then the response message contains:You escaped
	And the response contains the following hypermedia array:actions with values:
	| description | href | method | body |
	And the response contains the following hypermedia array:links with values:
	| description    | href   | method | body |
	| get-mazes-root | /mazes | GET    |      |



