namespace Repro;

[Icon( "videogame_asset" )]
public partial class GameManager : Component
{
	protected override void OnStart()
	{
		base.OnStart();

		Networking.CreateLobby( new()
		{
			Privacy = Sandbox.Network.LobbyPrivacy.FriendsOnly,
		} );
	}
}
