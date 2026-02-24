namespace Repro;

public abstract partial class State : Component
{
	private static State _state;

	public static bool TryGetInstance<TState>( out TState state )
		where TState : State
	{
		if ( !_state.IsValid() && Game.ActiveScene.IsValid() )
			_state = Game.ActiveScene.Get<State>();

		state = _state as TState;
		return state.IsValid();
	}

	public static bool TryGetInstance( out State state )
		=> TryGetInstance<State>( out state );
}
