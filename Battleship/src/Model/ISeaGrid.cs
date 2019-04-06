using System;
/// <summary>
/// The ISeaGrid defines the read only interface of a Grid. This
/// allows each player to see and attack their opponents grid.
/// </summary>
public interface ISeaGrid
{
	/// <summary>
	/// Gets the width.
	/// </summary>
	/// <value>The width.</value>
	int Width {get;}

	/// <summary>
	/// Gets the height.
	/// </summary>
	/// <value>The height.</value>
	int Height {get;}
	
	/// <summary>
	/// Indicates that the grid has changed.
	/// </summary>
	// Event Changed As EventHandler VBConversions Warning: events in interfaces not supported in C#.
	
	/// <summary>
	/// Provides access to the given row/column.
	/// </summary>
	/// <param name="row">The row to access.</param>
	/// <param name="column">The column to access.</param>
	/// <value>What the player can see at that location.</value>
	/// <returns>What the player can see at that location.</returns>
	TileView this[int row, int column]{ get; }
	
	/// <summary>
	/// Mark the indicated tile as shot.
	/// </summary>
	/// <param name="row">The row of the tile.</param>
	/// <param name="col">The column of the tile.</param>
	/// <returns>The result of the attack.</returns>
	AttackResult HitTile(int row, int col);
}