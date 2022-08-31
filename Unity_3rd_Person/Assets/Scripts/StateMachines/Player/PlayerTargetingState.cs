using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.CancelEvent += OnCancel;
        stateMachine.InputReader.ToggleWalkEvent += OnToggleWalk;

        stateMachine.Animator.Play(TargetingBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.GetCurrentSpeed() * stateMachine.TargetingSpeedRatio, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
            UpdateAnimator(deltaTime, 0);
        UpdateAnimator(deltaTime, stateMachine.GetCurrentSpeedRatio());

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.CancelEvent -= OnCancel;
        stateMachine.InputReader.ToggleWalkEvent -= OnToggleWalk;
    }

    private void OnCancel()
    {
        stateMachine.Targeter.Cancel();

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime, float value)
    {
        float xMovement = stateMachine.InputReader.MovementValue.x * value;
        float yMovement = stateMachine.InputReader.MovementValue.y * value;

        stateMachine.Animator.SetFloat(TargetingForwardHash, yMovement, 0.1f, deltaTime);
        stateMachine.Animator.SetFloat(TargetingRightHash, xMovement, 0.1f, deltaTime);
    }
}
