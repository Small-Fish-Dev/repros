using Sandbox;

namespace Repro;

[Icon( "videogame_asset" )]
public class GameManager : Component
{
	[Property]
	public GameObject PlayerPrefab { get; set; }

	protected override void OnStart()
	{
		base.OnStart();

		if ( !Scene.Get<Player>().IsValid() )
			SpawnPlayer();
	}

	public virtual GameObject SpawnPlayer()
	{
		if ( !PlayerPrefab.IsValid() )
			return null;

		var points = Scene.GetAll<SpawnPoint>().ToList();

		var randomPoint = Game.Random.FromList( points );

		var tSpawn = randomPoint.IsValid()
			? randomPoint.WorldTransform
			: global::Transform.Zero;

		return PlayerPrefab.Clone( tSpawn.WithScale( 1f ) );
	}
}
