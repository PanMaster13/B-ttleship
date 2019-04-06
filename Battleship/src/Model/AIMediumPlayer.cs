using System;
using System.Collections.Generic;
/// <summary>
/// The AIMediumPlayer is a type of AIPlayer where it will try and destroy a ship
/// if it has found a ship.
/// </summary>
public class AIMediumPlayer : AIPlayer
{
	/// <summary>
	/// Private enumarator for AI states. currently there are two states,
	/// the AI can be searching for a ship, or if it has found a ship it will
	/// target the same ship.
	/// </summary>
	private enum AIStates
	{
		Searching,
		TargetingShip
	}
	
	private AIStates _CurrentState = AIStates.Searching;
	private Stack<Location> _Targets = new Stack<Location>();

	/// <summary>
	/// Initializes a new instance of the <see cref="T:AIMediumPlayer"/> class.
	/// </summary>
	/// <param name="controller">Controller.</param>
	public AIMediumPlayer(BattleShipsGame controller) : base(controller)
	{
	}
	
	/// <summary>
	/// GenerateCoordinates should generate random shooting coordinates
	/// only when it has not found a ship, or has destroyed a ship and
	/// needs new shooting coordinates.
	/// </summary>
	/// <param name="row">The generated row.</param>
	/// <param name="column">The generated column.</param>
	protected override void GenerateCoords(ref int row, ref int column)
	{
		do
		{
			// Check which state the AI is in and uppon that choose which coordinate generation
			// method will be used.
			switch (_CurrentState)
			{
				// AI is searching for ship.
				case AIStates.Searching:
					SearchCoords(ref row, ref column);
					break;
					// AI is targeting ship.
				case AIStates.TargetingShip:
					TargetCoords(ref row, ref column);
					break;
				default:
					throw (new ApplicationException("AI has gone in an imvalid state"));
			}
		} 
		// While inside the grid and not a sea tile do the search.
		while (row < 0 || column < 0 || row >= EnemyGrid.Height || column >= EnemyGrid.Width || EnemyGrid[row, column] != TileView.Sea); 
	}
	
	/// <summary>
	/// TargetCoords is used when a ship has been hit and it will try and destroy
	/// this ship.
	/// </summary>
	/// <param name="row">Row generated around the hit tile.</param>
	/// <param name="column">Column generated around the hit tile.</param>
	private void TargetCoords(ref int row, ref int column)
	{
		Location l = _Targets.Pop();
		
		if (_Targets.Count == 0)
		{
			_CurrentState = AIStates.Searching;
		}
		row = l.Row;
		column = l.Column;
	}
	
	/// <summary>
	/// SearchCoords will randomly generate shots within the grid as long as its not hit that tile already.
	/// </summary>
	/// <param name="row">The generated row.</param>
	/// <param name="column">The generated column.</param>
	private void SearchCoords(ref int row, ref int column)
	{
		row = _Random.Next(0, EnemyGrid.Height);
		column = _Random.Next(0, EnemyGrid.Width);
	}
	
	/// <summary>
	/// ProcessShot will be called uppon when a ship is found.
	/// It will create a stack with targets it will try to hit. These targets
	/// will be around the tile that has been hit.
	/// </summary>
	/// <param name="row">The row it needs to process.</param>
	/// <param name="col">The column it needs to process.</param>
	/// <param name="result">The result og the last shot (should be hit).</param>
	protected override void ProcessShot(int row, int col, AttackResult result)
	{
		// If the attack hits.
		if (result.Value == ResultOfAttack.Hit)
		{
			_CurrentState = AIStates.TargetingShip;
			AddTarget(row - 1, col);
			AddTarget(row, col - 1);
			AddTarget(row + 1, col);
			AddTarget(row, col + 1);
		}
		// If the attack is at a place that is already attacked.
		else if (result.Value == ResultOfAttack.ShotAlready)
		{
			throw (new ApplicationException("Error in AI"));
		}
	}
	
	/// <summary>
	/// AddTarget will add the targets it will shoot onto a stack
	/// </summary>
	/// <param name="row">The row of the targets location.</param>
	/// <param name="column">The column of the targets location.</param>
	private void AddTarget(int row, int column)
	{
		if (row >= 0 && column >= 0 && row < EnemyGrid.Height && column < EnemyGrid.Width && EnemyGrid[row, column] == TileView.Sea)
		{
			
			_Targets.Push(new Location(row, column));
		}
	}
}