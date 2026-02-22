namespace Repro;

[Icon( "videogame_asset" )]
public partial class GameManager : Component
{
	protected override void OnStart()
	{
		base.OnStart();

		Networking.CreateLobby( new()
		{
			Hidden = true,
			Privacy = Sandbox.Network.LobbyPrivacy.FriendsOnly,
		} );
	}

	protected Transform FindSpawnPoint()
	{
		var points = Scene.GetAll<SpawnPoint>().ToList();

		var randomPoint = Game.Random.FromList( points );

		var tSpawn = randomPoint.IsValid()
			? randomPoint.WorldTransform
			: global::Transform.Zero;

		return tSpawn.WithScale( 1f );
	}
}
