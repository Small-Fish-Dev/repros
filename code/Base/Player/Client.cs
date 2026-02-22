using ShrimplePawns;

namespace Repro;

[Icon( "account_box" )]
public partial class Client : ShrimplePawns.Client
{
	public static Client Local
	{
		get
		{
			if ( _local.IsValid() && _local.Network.IsOwner )
				return _local;

			return _local = Game.ActiveScene?.GetAll<Client>()
				.FirstOrDefault( cl => cl.Connection == Connection.Local );
		}

		protected set => _local = value;
	}

	private static Client _local;
}
