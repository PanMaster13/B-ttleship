/// <summary>
/// The different AI levels.
/// </summary>
public enum AIOption
{
	/// <summary>
	/// Easy, total random shooting.
	/// </summary>
	Easy,
	
	/// <summary>
	/// Medium, marks squares around hits.
	/// </summary>
	Medium,
	
	/// <summary>
	/// As medium, but removes shots once it misses.
	/// </summary>
	Hard,
	/// <summary>
	/// As hard, but AI needs to miss twice before changing turns
	/// </summary>
	Insane,
}