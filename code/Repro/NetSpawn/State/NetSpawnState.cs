namespace Repro;

[Icon( "celebration" )]
public sealed partial class NetSpawnState : State
{
	[Property, InlineEditor, WideMode]
	public List<NetSpawnPropData> Props { get; set; }

	public override void OnLocalPlayerRespawn( Player pl )
	{
		base.OnLocalPlayerRespawn( pl );

		if ( !pl.IsValid() )
			return;

		pl.Transform.ClearInterpolation();

		if ( Props is null || Props.Count <= 0 )
			return;

		foreach ( var data in Props )
			Spawn( pl.GameObject, in data );
	}

	public void Spawn( GameObject targetObj, in NetSpawnPropData data )
	{
		if ( !targetObj.IsValid() )
			return;

		if ( data.Order is NetSpawnOrder.None )
			return;

		if ( !data.Model.IsValid() )
		{
			Log.Warning( "No valid spawner model!" );
			return;
		}

		var obj = Scene.CreateObject();

		obj.WorldTransform = targetObj.WorldTransform.ToWorld( data.Offset );

		if ( data.Model.IsValid() )
		{
			var r = obj.Components.Create<ModelRenderer>();
			r.Model = data.Model;

			obj.Name = r.Model.ResourceName;
		}

		if ( data.Order is NetSpawnOrder.Before )
		{
			obj.NetworkSpawn( Connection.Local );
			obj.SetParent( targetObj );
		}
		else if ( data.Order is NetSpawnOrder.After )
		{
			obj.SetParent( targetObj );
			obj.NetworkSpawn( Connection.Local );
		}
	}
}
