using ShrimplePawns;

namespace Repro;

partial class GameManager : Component.INetworkListener
{
	[Property]
	public GameObject ClientPrefab { get; set; }

	/// <summary>
	/// The default pawn to assign if the state doesn't.
	/// </summary>
	[Property]
	public GameObject PlayerPrefab { get; set; }

	public virtual void OnActive( Connection cn )
	{
		if ( cn is null )
			return;

		if ( !TryFindClient( cn, out var cl ) )
			cl = CreateClient( cn );

		if ( !cl.IsValid() )
		{
			Log.Warning( $"Couldn't create {typeof( Client )} for {cn}!" );
			return;
		}

		if ( State.TryGetInstance( out var state ) )
			state.OnClientJoined( cl, cn );

		if ( !cl.GetPawn().IsValid() )
			AssignDefaultPawn( cl );
	}

	public virtual void OnDisconnected( Connection cn )
	{
		if ( TryFindClient( cn, out var cl ) )
		{
			if ( State.TryGetInstance( out var state ) )
				state.OnClientDisconnected( cl, cn );

			if ( cl.IsValid() )
				cl.Destroy();
		}
	}

	protected Client CreateClient( Connection cn )
	{
		if ( !Networking.IsHost )
			return null;

		if ( !ClientPrefab.IsValid() )
		{
			Log.Warning( $"{nameof( ClientPrefab )} was not valid!" );
			return null;
		}

		var clObj = ClientPrefab.Clone();
		var cl = clObj.Components.Get<Client>();

		if ( !cl.IsValid() )
		{
			clObj.DestroyImmediate();
			return null;
		}

		cl.AssignConnection( cn );

		clObj.NetworkSpawn( enabled: true, owner: cn );

		return cl;
	}

	public bool TryFindClient( Connection cn, out Client cl )
	{
		cl = null;

		if ( cn is null || !Scene.IsValid() )
			return false;

		cl = Scene.GetAll<Client>().FirstOrDefault( c => c.ConnectionId == cn.Id );

		return cl.IsValid();
	}

	public static bool TryAssignPawn( Client cl, GameObject pawnObj, out Pawn pawn )
	{
		if ( !cl.IsValid() || !pawnObj.IsValid() )
		{
			pawn = null;
			return false;
		}

		pawn = cl.AssignPawn( pawnObj );

		if ( !pawn.IsValid() )
		{
			Log.Warning( $"Couldn't assign pawn:[{pawnObj}] for client:[{cl}]!" );
			return false;
		}

		if ( State.TryGetInstance( out var state ) )
			state.OnPawnAssigned( cl, pawn );

		return true;
	}

	protected Pawn AssignDefaultPawn( Client cl )
	{
		if ( !PlayerPrefab.IsValid() )
		{
			Log.Warning( $"No valid default pawn for client:[{cl}]!" );
			return null;
		}

		var tSpawn = FindSpawnPoint();
		var plObj = PlayerPrefab.Clone( tSpawn );

		if ( !TryAssignPawn( cl, plObj, out var pl ) )
			if ( plObj.IsValid() )
				plObj.Destroy();

		return pl;
	}
}
