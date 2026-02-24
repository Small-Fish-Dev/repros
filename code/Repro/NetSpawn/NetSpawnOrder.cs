namespace Repro;

[DefaultValue( None )]
public enum NetSpawnOrder
{
	/// <summary>
	/// No automatic spawning.
	/// </summary>
	None,

	/// <summary>
	/// Attach BEFORE network spawning.
	/// </summary>
	Before,

	/// <summary>
	/// Attach AFTER network spawning.
	/// </summary>
	After,
}
