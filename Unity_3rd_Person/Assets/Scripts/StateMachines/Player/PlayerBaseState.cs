
public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine = null;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
