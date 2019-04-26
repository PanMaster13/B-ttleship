using System;
using SwinGameSDK;
using System.Collections.Generic;

///<summary>
///GameController is responsible for controlling the game,
///managing user input, and displaing the current state of the game.
///</summary>
public static class GameController
{
    private static BattleShipsGame _theGame;
    private static Player _human;
	private static AIPlayer _ai;
    private static Stack<GameState> _state = new Stack<GameState>();
	private static int _humanShips, _aiShips;

    private static AIOption _aiSetting;

    ///<summary>
    ///CurrentState returns the current state of the game, indicating which screen is currently being used.
    /// </summary>
    ///<value>The current state</value>
    ///<returns>The current state</returns>
    public static GameState CurrentState
    {
        get
        {
            return _state.Peek();
        }

    }


    ///<summary>
    ///HumanPlayer returns the human player.
    ///</summary>
    ///<value>The human player</value>
    ///<returns>The humna player</returns>
    public static Player HumanPlayer
    {
        get
        {
            return _human;
        }
    }

    ///<summary>
    ///ComputerPlayer returns the computer player.
    ///</summary>
    ///<value>The computer player</value>
    ///<returns>The computer player</returns>
    public static Player ComputerPlayer
    {
        get
        {
            return _ai;
        }
    }

	public static int humanShipsLeft
	{
		get
		{
			return _humanShips;
		}
		set {
			_humanShips = value;
		}
	}

	public static int aiShipsLeft
	{
		get
		{
			return _aiShips;
		}
		set
		{
			_aiShips = value;
		}
	}

    static GameController()
    {
        // bottom state will be quitting. If player exits main menu then the game is over
        _state.Push(GameState.Quitting);

        // at the start the player is viewing the main menu
        _state.Push(GameState.ViewingMainMenu);
    }

    ///<summary>
    ///StartGame starts a new game.
    ///</summary>
    ///<remarks>
    ///Creates an AI player based upon the _aiSetting.
    /// </remarks>
    public static void StartGame()
    {
        if (_theGame != null)
            EndGame();

        // Create the game
        _theGame = new BattleShipsGame();

        // create the players
        switch (_aiSetting)
        {
            case AIOption.Easy:
                {
                    _ai = new AIEasyPlayer(_theGame);
                    break;
                }
            case AIOption.Medium:
                {
                    _ai = new AIMediumPlayer(_theGame);
                    break;
                }

            case AIOption.Hard:
                {
                    _ai = new AIHardPlayer(_theGame);
                    break;
                }
			case AIOption.Insane:
				{
					_ai = new AIInsanePlayer(_theGame);
					break;
				}

            default:
                {
                    _ai = new AIEasyPlayer(_theGame);
                    break;
                }
        }

        _human = new Player(_theGame);
		_aiShips = 5;
		_humanShips = 5;

        // AddHandler _human.PlayerGrid.Changed, AddressOf GridChanged
        _ai.PlayerGrid.Changed += GridChanged;
        _theGame.AttackCompleted += AttackCompleted;

        AddNewState(GameState.Deploying);
    }

    ///<summary>
    ///EndGame stops listening to the old game once a new game is started.
    ///</summary>
    private static void EndGame()
    {
        // RemoveHandler _human.PlayerGrid.Changed, AddressOf GridChanged
        _ai.PlayerGrid.Changed -= GridChanged;
        _theGame.AttackCompleted -= AttackCompleted;
    }


    ///<summary>
    ///GridChanged listens to the game grids for any changes and redraws the screen
    ///when the grid change.
    /// </summary>
    /// <param name="sender">The grid that changed</param>
    /// <param name="args">Not used</param>
    private static void GridChanged(object sender, EventArgs args)
    {
        DrawScreen();
        SwinGame.RefreshScreen();
    }

    /// <summary>
    /// PlayHitSequence plays a small animation at the location of impact
    /// if the player hits an enemy ship.
    /// </summary>
    /// <param name="row">The row of the hit enemy ship is located in.</param>
    /// <param name="column">The column of the hit enemy ship is located in.</param>
    /// <param name="showAnimation">A boolean value to determine if the enemy ship is hit.</param>
    private static void PlayHitSequence(int row, int column, bool showAnimation)
    {
        if (showAnimation)
        {
            UtilityFunctions.AddExplosion(row, column);
        }
        Audio.PlaySoundEffect(GameResources.GameSound("Hit"));

        UtilityFunctions.DrawAnimationSequence();
    }

    /// <summary>
    /// PlayMissSequence plays a small animation at the location of miss
    /// if the player misses the enemy ship.
    /// </summary>
    /// <param name="row">The row of the miss hit.</param>
    /// <param name="column">The column of the miss hit.</param>
    /// <param name="showAnimation">A boolean value to determine if the enemy ship is missed.</param>
    private static void PlayMissSequence(int row, int column, bool showAnimation)
    {
        if (showAnimation)
        { 
            UtilityFunctions.AddSplash(row, column);
        }
        Audio.PlaySoundEffect(GameResources.GameSound("Miss"));

        UtilityFunctions.DrawAnimationSequence();
    }

    ///<summary>
    ///AttackCompleted listens for attacks to be completed.
    ///It will display a message, play a sound and redraw the screen.
    ///</summary>
    ///<param name="sender">The current game Object.</param>
    ///<param name="result">The result of the attack.</param>
    private static void AttackCompleted(object sender, AttackResult result)
    {
        bool isHuman;
        isHuman = _theGame.Player==HumanPlayer;

        if (isHuman)
        {
            UtilityFunctions.Message = "You " + result.ToString();
        }
        else
        {
            UtilityFunctions.Message = "The AI " + result.ToString();
        }

        switch (result.Value)
        {
            case ResultOfAttack.Destroyed:
                {
                    PlayHitSequence(result.Row, result.Column, isHuman);
                    Audio.PlaySoundEffect(GameResources.GameSound("Sink"));
					if (isHuman)
					{
						aiShipsLeft--;
					}
					else
					{
						humanShipsLeft--;
					}
                    break;
                }

            case ResultOfAttack.GameOver:
                {
                    PlayHitSequence(result.Row, result.Column, isHuman);
                    Audio.PlaySoundEffect(GameResources.GameSound("Sink"));

                    while (Audio.SoundEffectPlaying(GameResources.GameSound("Sink")))
                    {
                        SwinGame.RefreshScreen();
                    }

                    if (_human.IsDestroyed)
                    {
                        Audio.PlaySoundEffect(GameResources.GameSound("Lose"));
                    }
                    else
                    {
                        Audio.PlaySoundEffect(GameResources.GameSound("Winner"));
                    }
                        break;
                }

            case ResultOfAttack.Hit:
                {
                    PlayHitSequence(result.Row, result.Column, isHuman);
                    break;
                }

            case ResultOfAttack.Miss:
                {
                    PlayMissSequence(result.Row, result.Column, isHuman);
                    break;
                }

            case ResultOfAttack.ShotAlready:
                {
                    Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                    break;
                }
        }
    }

    ///<summary>
    ///EndDeployment completes the deployment phase of the game and
    ///switches to the battle mode (discovery state).
    ///</summary>
    ///<remarks>
    ///Adds the player objects to the game before switching states.
    ///</remarks>
    public static void EndDeployment()
    {
        // deploy the players
        _theGame.AddDeployedPlayer(_human);
        _theGame.AddDeployedPlayer(_ai);

        SwitchState(GameState.Discovering);
    }

    ///<summary>
    ///Attack allow the human Player to attack the indicated row and column.
    ///</summary>
    ///<param name="row">The row to attack.</param>
    ///<param name="col">The column to attack.</param>
    ///<remarks>Checks the results of the attack once it is complete.</remarks>
    public static void Attack(int row, int col)
    {
        AttackResult result;
        result = _theGame.Shoot(row, col);
        CheckAttackResult(result);
    }

    ///<summary>
    ///AIAttack gets the ai Player to attack.
    /// </summary>
    /// <remarks>
    /// Checks the attack result once the attack is complete.
    /// </remarks>
    private static void AIAttack()
    {
        AttackResult result;
        result = _theGame.Player.Attack();
        CheckAttackResult(result);
    }

    ///<summary>
    ///CheckAttackResult checks the results of the attack and switches to
    ///ending the game if the result is game over.
    /// </summary>
    /// <param name="result">The result of the attack.</param>
    /// <remarks>Gets the AI to attack if the result switched to the AI player.</remarks>
    private static void CheckAttackResult(AttackResult result)
    {
        switch (result.Value)
        {
            case ResultOfAttack.Miss:
                {
                    if (_theGame.Player==ComputerPlayer)
                    {
                        AIAttack();
                    }
                        break;
                }

            case ResultOfAttack.GameOver:
                {
                    SwitchState(GameState.EndingGame);
                    break;
                }
        }
    }

    ///<summary>
    ///HandleUserInput handles the user SwinGame.
    /// </summary>
    /// <remarks>
    /// Reads the key and mouse input and converts these into
    /// actions for the game to perform. The actions
    /// performed depend upon the state of the game.
    /// </remarks>
    public static void HandleUserInput()
    {
        // Read incoming input events
        SwinGame.ProcessEvents();

        switch (CurrentState)
        {
            case GameState.ViewingMainMenu:
                {
                    MenuController.HandleMainMenuInput();
                    break;
                }

            case GameState.ViewingGameMenu:
                {
                    MenuController.HandleGameMenuInput();
                    break;
                }

            case GameState.AlteringSettings:
                {
                    MenuController.HandleDifficultyMenuInput();
                    break;
                }

            case GameState.Deploying:
                {
                    DeploymentController.HandleDeploymentInput();
                    break;
                }

            case GameState.Discovering:
                {
                    DiscoveryController.HandleDiscoveryInput();
                    break;
                }

            case GameState.EndingGame:
                {
                    EndingGameController.HandleEndOfGameInput();
                    break;
                }

            case GameState.ViewingHighScores:
                {
                    HighScoreController.HandleHighScoreInput();
                    break;
                }
        }

        UtilityFunctions.UpdateAnimations();
    }

    ///<summary>
    ///DrawScreen draws the current state of the game to the screen.
    /// </summary>
    /// <remarks>
    /// Draws the game based on its state.
    /// </remarks>
    public static void DrawScreen()
    {
        UtilityFunctions.DrawBackground();

        switch (CurrentState)
        {
            case GameState.ViewingMainMenu:
                {
                    MenuController.DrawMainMenu();
                    break;
                }

            case GameState.ViewingGameMenu:
                {
                    MenuController.DrawGameMenu();
                    break;
                }

            case GameState.AlteringSettings:
                {
                    MenuController.DrawSettings();
                    break;
                }

            case GameState.Deploying:
                {
                    DeploymentController.DrawDeployment();
                    break;
                }

            case GameState.Discovering:
                {
                    DiscoveryController.DrawDiscovery();
                    break;
                }

            case GameState.EndingGame:
                {
                    EndingGameController.DrawEndOfGame();
                    break;
                }

            case GameState.ViewingHighScores:
                {
                    HighScoreController.DrawHighScores();
                    break;
                }
        }

        UtilityFunctions.DrawAnimations();

        SwinGame.RefreshScreen();
    }

    ///<summary>
    ///AddNewState moves the game to a new state. The current state is maintained
    ///so that it can be returned to.
    /// </summary>
    /// <param name="state">The new game state</param>
    public static void AddNewState(GameState state)
    {
        _state.Push(state);
        UtilityFunctions.Message = "";
    }

    ///<summary>
    ///SwitchState ends the current state and adds in the new state.
    /// </summary>
    /// <param name="newState">The new state of the game.</param>
    public static void SwitchState(GameState newState)
    {
        EndCurrentState();
        AddNewState(newState);
    }

    ///<summary>
    ///EndCurrentState ends the current game state, returns to the prior game state.
    /// </summary>
    public static void EndCurrentState()
    {
        _state.Pop();
    }

    ///<summary>
    ///SetDifficulty sets the difficulty for the next level of the game.
    ///</summary>
    ///<param name="setting">The new difficulty level.</param>
    public static void SetDifficulty(AIOption setting)
    {
        _aiSetting = setting;
    }
}
