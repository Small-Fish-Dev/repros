using ShrimplePawns;

namespace Repro;

partial class State
{
	/// <summary>
	/// Called before the client is network spawned, just after its connection is assigned.
	/// </summary>
	public virtual void OnClientCreated( Client cl, Connection cn ) { }

	/// <summary>
	/// Called before the client is destroyed, just after disconnecting.
	/// </summary>
	public virtual void OnClientDisconnected( Client cl, Connection cn ) { }

	/// <summary>
	/// Called after a client has had its pawn successfully assigned.
	/// </summary>
	public virtual void OnPawnAssigned( Client cl, Pawn pawn ) { }
}
