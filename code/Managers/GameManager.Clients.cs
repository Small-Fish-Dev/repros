namespace Repro;

public partial class GameManager : Component.INetworkListener
{
	[Property]
	public GameObject ClientPrefab { get; set; }

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

		if ( !PlayerPrefab.IsValid() )
		{
			Log.Warning( $"Couldn't create pawn for {cl}!" );
			return;
		}

		cl.AssignPawn( PlayerPrefab.Clone() );
	}

	public virtual void OnDisconnected( Connection cn )
	{
		if ( TryFindClient( cn, out var cl ) )
			cl.Destroy();
	}

	protected Client CreateClient( Connection cn )
	{
		if ( !Networking.IsHost || !ClientPrefab.IsValid() )
			return null;

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
}
