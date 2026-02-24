namespace Repro;

public struct NetSpawnPropData
{
	[Property]
	public NetSpawnOrder Order { get; set; } = NetSpawnOrder.None;

	[Property]
	public Model Model { get; set; }

	[Property]
	public Transform Offset { get; set; } = new( Vector3.Up * 80f );

	public NetSpawnPropData()
	{
	}
}
