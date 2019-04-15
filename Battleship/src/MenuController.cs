using SwinGameSDK;


/// <summary>
/// The menu controller handles the drawing and user interactions
/// from the menus in the game. These include the main menu, game
/// menu and the settings menu.
/// </summary>

public static class MenuController
{
	
	/// <summary>
	/// The menu structure for the game.
	/// </summary>
	/// <remarks>
	/// These are the text captions for the menu items.
	/// </remarks>
	private readonly static string[][] _menuStructure = {new string[] {"PLAY", "DIFFICULTY", "SCORES", "QUIT"}, 
		new string[] {"RETURN", "SURRENDER", "QUIT"}, 
		new string[] {"EASY", "MEDIUM", "HARD"}};
	
	private const int MENU_TOP = 550;
	private const int MENU_LEFT = 30;
	private const int MENU_GAP = 0;
	private const int BUTTON_WIDTH = 75;
	private const int BUTTON_HEIGHT = 15;
	private const int BUTTON_SEP = BUTTON_WIDTH + MENU_GAP;
	private const int TEXT_OFFSET = 0;
	
	private const int MAIN_MENU = 0;
	private const int GAME_MENU = 1;
	private const int DIFFICULTY_MENU = 2;
	
	private const int MAIN_MENU_PLAY_BUTTON = 0;
	private const int MAIN_MENU_SETUP_BUTTON = 1;
	private const int MAIN_MENU_TOP_SCORES_BUTTON = 2;
	private const int MAIN_MENU_QUIT_BUTTON = 3;
	
	private const int DIFFICULTY_MENU_EASY_BUTTON = 0;
	private const int DIFFICULTY_MENU_MEDIUM_BUTTON = 1;
	private const int DIFFICULTY_MENU_HARD_BUTTON = 2;
	private const int DIFFICULTY_MENU_EXIT_BUTTON = 3;
	
	private const int GAME_MENU_RETURN_BUTTON = 0;
	private const int GAME_MENU_SURRENDER_BUTTON = 1;
	private const int GAME_MENU_QUIT_BUTTON = 2;
	
	private readonly static Color MENU_COLOR = SwinGame.RGBAColor(2, 167, 252, 255);
	private readonly static Color HIGHLIGHT_COLOR = SwinGame.RGBAColor(1, 57, 86, 255);
	
	/// <summary>
	/// Handles the processing of user input when the main menu is showing.
	/// </summary>
	public static void HandleMainMenuInput()
	{
		HandleMenuInput(MAIN_MENU, 0, 0);
	}
	
	/// <summary>
	/// Handles the processing of user input when the main menu is showing.
	/// </summary>
	public static void HandleDifficultyMenuInput()
	{
		bool handled = false;
		handled = HandleMenuInput(DIFFICULTY_MENU, 1, 1);
		
		if (!handled)
		{
			HandleMenuInput(MAIN_MENU, 0, 0);
		}
	}
	
	/// <summary>
	/// Handle input in the game menu.
	/// </summary>
	/// <remarks>
	/// Player can return to the game, surrender, or quit entirely.
	/// </remarks>
	public static void HandleGameMenuInput()
	{
		HandleMenuInput(GAME_MENU, 0, 0);
	}
	
	/// <summary>
	/// Handles input for the specified menu.
	/// </summary>
	/// <param name="menu">The identifier of the menu being processed.</param>
	/// <param name="level">The vertical level of the menu.</param>
	/// <param name="xOffset">The xoffset of the menu.</param>
	/// <returns>False if a clicked missed the buttons. This can be used to check prior menus.</returns>
	private static bool HandleMenuInput(int menu, int level, int xOffset)
	{
		// If Escape key is pressed.
		if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE)) 
		{
			GameController.EndCurrentState();
			return true;
		}

		// If Left Mouse Button is clicked.
		if (SwinGame.MouseClicked(MouseButton.LeftButton)) 
		{
			int i = 0;
			for (i = 0; i <= _menuStructure[menu].Length - 1; i++)
			{
				// IsMouseOver the i'th button of the menu.
				if (IsMouseOverMenu(i, level, xOffset))
				{
					PerformMenuAction(menu, i);
					return true;
				}
			}
			
			if (level > 0)
			{
				// None clicked - so end this sub menu.
				GameController.EndCurrentState();
			}
		}
		
		return false;
	}
	
	/// <summary>
	/// Draws the main menu to the screen.
	/// </summary>
	public static void DrawMainMenu()
	{
		// Clears the Screen to Black.
		// SwinGame.DrawText("Main Menu", Color.White, GameFont("ArialLarge"), 50, 50).
		
		DrawButtons(MAIN_MENU);
	}
	
	/// <summary>
	/// Draws the Game menu to the screen.
	/// </summary>
	public static void DrawGameMenu()
	{
		// Clears the Screen to Black.
		// SwinGame.DrawText("Paused", Color.White, GameFont("ArialLarge"), 50, 50).

		DrawButtons(GAME_MENU);
	}
	
	/// <summary>
	/// Draws the settings menu to the screen.
	/// </summary>
	/// <remarks>
	/// Also shows the main menu.
	/// </remarks>
	public static void DrawSettings()
	{
		// Clears the Screen to Black.
		// SwinGame.DrawText("Settings", Color.White, GameFont("ArialLarge"), 50, 50).
		
		DrawButtons(MAIN_MENU);
		DrawButtons(DIFFICULTY_MENU, 1, 1);
	}
	
	/// <summary>
	/// Draw the buttons associated with a top level menu.
	/// </summary>
	/// <param name="menu">The index of the menu to draw.</param>
	private static void DrawButtons(int menu)
	{
		DrawButtons(menu, 0, 0);
	}
	
	/// <summary>
	/// Draws the menu at the indicated level.
	/// </summary>
	/// <param name="menu">The menu to draw.</param>
	/// <param name="level">The level (height) of the menu.</param>
	/// <param name="xOffset">The offset of the menu.</param>
	/// <remarks>
	/// The menu text comes from the _menuStructure field. The level indicates the height
	/// of the menu, to enable sub menus. The xOffset repositions the menu horizontally
	/// to allow the submenus to be positioned correctly.
	/// </remarks>
	private static void DrawButtons(int menu, int level, int xOffset)
	{
		int btnTop = 0;
		
		btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
		int i = 0;
		for (i = 0; i <= _menuStructure[menu].Length - 1; i++)
		{
			int btnLeft = 0;
			btnLeft = MENU_LEFT + BUTTON_SEP * (i + xOffset);
			// SwinGame.FillRectangle(Color.White, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT).
			SwinGame.DrawTextLines(_menuStructure[menu][i], MENU_COLOR, Color.Black, GameResources.GameFont("Menu"), FontAlignment.AlignCenter, btnLeft + TEXT_OFFSET, btnTop + TEXT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT);
			
			if (SwinGame.MouseDown(MouseButton.LeftButton) && IsMouseOverMenu(i, level, xOffset))
			{
				SwinGame.DrawRectangle(HIGHLIGHT_COLOR, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
			}
		}
	}
	
	/// <summary>
	/// Determined if the mouse is over one of the button in the main menu.
	/// </summary>
	/// <param name="button">The index of the button to check.</param>
	/// <returns>True if the mouse is over that button.</returns>
	private static bool IsMouseOverButton(int button)
	{
		return IsMouseOverMenu(button, 0, 0);
	}
	
	/// <summary>
	/// Checks if the mouse is over one of the buttons in a menu.
	/// </summary>
	/// <param name="button">The index of the button to check.</param>
	/// <param name="level">The level of the menu.</param>
	/// <param name="xOffset">The xOffset of the menu.</param>
	/// <returns>True if the mouse is over the button.</returns>
	private static bool IsMouseOverMenu(int button, int level, int xOffset)
	{
		int btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
		int btnLeft = MENU_LEFT + BUTTON_SEP * (button + xOffset);
		
		return UtilityFunctions.IsMouseInRectangle(btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
	}
	
	/// <summary>
	/// A button has been clicked, perform the associated action.
	/// </summary>
	/// <param name="menu">The menu that has been clicked.</param>
	/// <param name="button">The index of the button that was clicked.</param>
	private static void PerformMenuAction(int menu, int button)
	{
		// A switch case involving which button is pressed.
		switch (menu)
		{
			// If main menu button is pressed.
			case MAIN_MENU: 
				PerformMainMenuAction(button);
				break;
				// If difficulty menu button is pressed.
			case DIFFICULTY_MENU: 
				PerformDifficultyMenuAction(button);
				break;
				// If game menu button is pressed.
			case GAME_MENU: 
				PerformGameMenuAction(button);
				break;
		}
	}
	
	/// <summary>
	/// The main menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">The button pressed.</param>
	private static void PerformMainMenuAction(int button)
	{
		// A switch case involving buttons in the main menu.
		switch (button)
		{
			// Play button is pressed.
			case MAIN_MENU_PLAY_BUTTON: 
				GameController.StartGame();
				break;
				// Setup button is pressed.
			case MAIN_MENU_SETUP_BUTTON: 
				GameController.AddNewState(GameState.AlteringSettings);
				break;
				// Scores button is pressed.
			case MAIN_MENU_TOP_SCORES_BUTTON: 
				GameController.AddNewState(GameState.ViewingHighScores);
				break;
				// Quit button is pressed.
			case MAIN_MENU_QUIT_BUTTON: 
				GameController.EndCurrentState();
				break;
		}
	}
	
	/// <summary>
	/// The difficulty menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">The button pressed.</param>
	private static void PerformDifficultyMenuAction(int button)
	{
		// A switch case involving buttons in the setup menu.
		switch (button)
		{
			// Sets difficulty to easy.
			case DIFFICULTY_MENU_EASY_BUTTON: 
				GameController.SetDifficulty(AIOption.Hard);
                Audio.PlaySoundEffect(GameResources.GameSound("Easy"));
				break;
				// Sets difficulty to medium.
			case DIFFICULTY_MENU_MEDIUM_BUTTON: 
				GameController.SetDifficulty(AIOption.Hard);
                Audio.PlaySoundEffect(GameResources.GameSound("Medium"));
                break;
				// Sets difficulty to hard.
			case DIFFICULTY_MENU_HARD_BUTTON: 
				GameController.SetDifficulty(AIOption.Hard);
                Audio.PlaySoundEffect(GameResources.GameSound("Hard"));
                break;
		}
		// Always end state - handles exit button as well.
		GameController.EndCurrentState();
	}
	
	/// <summary>
	/// The game menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">The button pressed.</param>
	private static void PerformGameMenuAction(int button)
	{
		// A switch case involving buttons in the game menu.
		switch (button)
		{
			// Return button is pressed.
			case GAME_MENU_RETURN_BUTTON: 
				GameController.EndCurrentState();
				break;
				// Surrender button is pressed.
			case GAME_MENU_SURRENDER_BUTTON: 
				GameController.EndCurrentState(); // End game menu.
				GameController.EndCurrentState(); // End game.
				break;
				// Quit button is pressed.
			case GAME_MENU_QUIT_BUTTON: 
				GameController.AddNewState(GameState.Quitting);
				break;
		}
	}
}