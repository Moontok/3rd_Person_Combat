using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private float airTime = 0f;

    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.ToggleWalkEvent += OnToggleWalk;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (!stateMachine.Controller.isGrounded)
        {
            airTime += deltaTime;
            if (airTime >= stateMachine.StartFallingTime)
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
                return;
            }
        }
        else
        {
            airTime = 0f;
        }


        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);

        Move(movement * stateMachine.GetCurrentSpeed() * stateMachine.TargetingSpeedRatio, deltaTime);

        UpdateAnimator(deltaTime, stateMachine.GetCurrentSpeedRatio());

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.ToggleWalkEvent -= OnToggleWalk;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnTarget()
    {
        stateMachine.Targeter.Cancel();

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void OnDodge()
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero) return;

        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingFirstHalfState(stateMachine, true));
    }

    private Vector3 CalculateMovement(float deltaTime)
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
