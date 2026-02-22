using Sandbox.Citizen;
using ShrimplePawns;
using SCC = ShrimpleCharacterController.ShrimpleCharacterController;

namespace Repro;

public partial class Player : Pawn
{
	[RequireComponent]
	public SCC Controller { get; set; }

	[Property]
	public SkinnedModelRenderer Renderer { get; set; }

	[Property]
	public GameObject Camera { get; set; }

	[Property]
	[Range( 50f, 200f )]
	public float WalkSpeed { get; set; } = 100f;

	[Property]
	[Range( 100f, 500f )]
	public float RunSpeed { get; set; } = 300f;

	[Property]
	[Range( 25f, 100f )]
	public float DuckSpeed { get; set; } = 50f;

	[Property]
	[Range( 200f, 500f )]
	public float JumpStrength { get; set; } = 350f;

	public Angles EyeAngles { get; set; }

	protected override void OnStart()
	{
		base.OnStart();

		Renderer = Components.Get<SkinnedModelRenderer>( FindMode.EverythingInSelfAndDescendants );

		if ( !Camera.IsValid() )
			Camera = Scene.Camera?.GameObject;
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		if ( IsProxy || !Controller.IsValid() )
			return;

		var wishDirection = Input.AnalogMove.Normal * Rotation.FromYaw( EyeAngles.yaw ) * GameObject.WorldRotation;
		var isDucking = Input.Down( "Duck" );
		var isRunning = Input.Down( "Run" );
		var wishSpeed = isDucking ? DuckSpeed :
			isRunning ? RunSpeed : WalkSpeed;

		Controller.WishVelocity = wishDirection * wishSpeed;

		if ( Input.Pressed( "Jump" ) && Controller.IsOnGround )
		{
			Controller.Punch( -Controller.AppliedGravity.Normal * JumpStrength );
			Renderer?.Set( "b_jump", true );
		}

		if ( !Renderer.IsValid() )
			return;

		var oldX = Renderer.GetFloat( "move_x" );
		var oldY = Renderer.GetFloat( "move_y" );

		var velocity = Controller.Velocity;
		var runScale = 1f / Renderer.Transform.World.UniformScale;
		var moveX = Vector3.Dot( velocity, Renderer.WorldRotation.Forward ) * runScale;
		var moveY = Vector3.Dot( velocity, Renderer.WorldRotation.Right ) * runScale;

		var newX = MathX.Lerp( oldX, moveX, Time.Delta * 10f );
		var newY = MathX.Lerp( oldY, moveY, Time.Delta * 10f );

		Renderer.Set( "move_x", newX );
		Renderer.Set( "move_y", newY );

		Renderer.Set( "b_grounded", Controller.IsOnGround );
		Renderer.Set( "duck", isDucking ? 1f : 0f );
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		if ( IsProxy )
			return;

		EyeAngles += Input.AnalogLook;
		EyeAngles = EyeAngles.WithPitch( MathX.Clamp( EyeAngles.pitch, -89.5f, 89.5f ) );

		if ( Renderer.IsValid() )
			Renderer.WorldRotation = Rotation.Slerp( Renderer.WorldRotation, Rotation.FromYaw( EyeAngles.yaw ), Time.Delta * 15f );

		if ( Camera.IsValid() )
		{
			var eyePos = WorldTransform.PointToWorld( Vector3.Up * 64f );
			var rEye = WorldTransform.RotationToWorld( EyeAngles );

			eyePos += rEye.Backward * 256f;

			Camera.WorldRotation = rEye;
			Camera.WorldPosition = eyePos;
		}
	}
}
