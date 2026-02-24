using ShrimplePawns;

namespace Repro;

partial class State
{
	/// <summary>
	/// Called after the client is network spawned, just before it's given a pawn.
	/// </summary>
	public virtual void OnClientJoined( Client cl, Connection cn ) { }

	/// <summary>
	/// Called before the client is destroyed, just after disconnecting.
	/// </summary>
	public virtual void OnClientDisconnected( Client cl, Connection cn ) { }

	/// <summary>
	/// Called after a client has had its pawn successfully assigned.
	/// </summary>
	public virtual void OnPawnAssigned( Client cl, Pawn pawn )
	{
		if ( pawn is Player pl && pl.IsValid() )
			pl.RpcHostRespawn();
	}

	/// <summary>
	/// Called when the host tells the player to (re)spawn.
	/// </summary>
	public virtual void OnPlayerRespawn( Player pl )
	{
	}
}
