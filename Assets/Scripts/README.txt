------------------------------------------------
THE FASTEST WAY TO UNDERSTAND WHAT'S GOING ON
------------------------------------------------

Go to default methods* which are placed straight after variables section. 
If you hover over each of the methods that are used inside of the default methods you will find out what each of them does 
(all methods are documented within the program, so the hints will show up).

* default methods - Start(), Update() and all other default Unity methods


------------------------------------------------
FILE MANAGEMENT & SCRIPTS OVERVIEW
------------------------------------------------

Scripts are sorted according to functions they serve.

Hierarchy of files (which is presented below) is structured the following way - folders are at the top, then scripts in given folder and then general overview of functionality of each script:

	----------------------------------------------------------------
	| (f) - folder										         
	| (s) - script										           
	| (m) - general overview of the script functionality (methods) 
	----------------------------------------------------------------

-(f) Collectables 
	-(s) Coins
		-(m) add score when coin is picked up; play coin sound
	-(s) Powerups 
		-(m) create random powerups; activate picked up powerup	

-(f) Controllers
	-(s) CameraController
		-(m) move camera when player moves
	-(s) ParallaxBackground
		-(m) move layer at specified speed; create infinite background
	-(s) PlayerController
		-(m) speed, jump, animation and sound management of the player

-(f) Generators
	-(s) CoinGenerator
		-(m) generate coins; spawn three coins
	-(s) PlatformGenerator
		-(m) create random platform; randomly spawn coins, spikes and powerups on the platforms; move platform generator to avoid overlapping platforms

-(f) Managers
	-(s) GameManager
		-(m) restart the game by resetting player stats and position to initial values; activate death screen
	-(s) PowerupManager
		-(m) choose which powerups to activate and when to deactivate them
	-(s) ScoreManager
		-(m) add and update score while player is alive; update high score if necessary

-(f) Menus
	-(s) DeathMenu
		-(m) restart game by pressing UI button or 'enter'; quit to main menu
	-(f) MainMenu
		-(s) MainMenu
			-(m) start game; quit game
		-(s) MainMenuBackground
			-(m) move invisible player (implemented to move the background, because background follows player)
		-(s) MainMenuCamera
			-(m) move camera with background
	-(s) PauseMenu
		-(m) pause game; resume game; restart game; quit to main menu
	-(s) SettingsMenu
		-(m) change, save and load resolution, quality, sound settings

-(f) Object Management
	-(s) ObjectDestroyer
		-(m) destroy objects when they destruction point passes them
	-(s) ObjectPooler
		-(m) get inactive object from the pool of object or create a new one if there no any


------------------------------------------------
CODE STYLE
------------------------------------------------

-usage of underscores
	-underscores are used for private variables with Unity or Game specific types (e.g. ScoreManager _scoreManager, AudioSource _coinSound etc.)

-comments
	-most of the code is self descriptive but I added comments just in case if someone will need them
	-if there is a tooltip added to the variable, there is no comment for it to reduce redundant documentation 

-methods
	-I tried to separate different functionality into different methods in order to build few levels of abstraction in order to make program more readable
	-methods that are not default are documented using summary feature

-usage of #regions
	-there are three main region names throughout the program which are:
		-variables (all variables used in the script)
		-default methods (unity methods)
		-custom methods (methods that were created specifically for this program)
	-regions are used only in big scripts which have either many variables, default methods or custom methods


------------------------------------------------
NOTES
------------------------------------------------

-FindObjectOfType() method is used for getting objects (e.g. _scoreManager) throghout the program only in cases where there is only one instance of this object type in the program.
