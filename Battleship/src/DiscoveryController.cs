using System;
using SwinGameSDK;

///<summary>
///DiscoveryController handles the battle phase.
/// </summary>
public static class DiscoveryController
{

    ///<summary>
    ///HandleDiscoveryInput handles input during the discovery phase of the game.
    /// </summary>
    /// <remarks>
    /// Pressing the esc key will open the game menu. Clicking the mouse will attack a location.
    /// </remarks>
    public static void HandleDiscoveryInput()
    {
		if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
		{
			GameController.AddNewState(GameState.ViewingGameMenu);
		}
        if (SwinGame.MouseClicked(MouseButton.LeftButton))
            DoAttack();
    }

    ///<summary>
    ///DoAttack attacks the location that the mouse is over.
    /// </summary>
    private static void DoAttack()
    {
        Point2D mouse;

        mouse = SwinGame.MousePosition();

        // Calculate the row/col clicked
        int row, col;
        row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions.FIELD_TOP) / (double)(UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
        col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (double)(UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

        if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height)
        {
            if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width)
                GameController.Attack(row, col);
        }
    }

    ///<summary>
    ///DrawDiscovery draws the game during the attack phase.
    /// </summary>
    public static void DrawDiscovery()
    {
        const int SCORES_LEFT = 172;
        const int SHOTS_TOP = 157;
        const int HITS_TOP = 206;
        const int SPLASH_TOP = 256;
		const int SHIPS_LEFT = 306;

        if ((SwinGame.KeyDown(KeyCode.vk_LSHIFT) | SwinGame.KeyDown(KeyCode.vk_RSHIFT)) & SwinGame.KeyDown(KeyCode.vk_c))
            UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
        else
            UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);

        UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
        UtilityFunctions.DrawMessage();

        SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
        SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
        SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
		SwinGame.DrawText("AI Ships Left:     "+ GameController.aiShipsLeft.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT-50, SHIPS_LEFT);
    }
}
