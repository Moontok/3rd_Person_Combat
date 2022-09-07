using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    private float airTime = 0f;

    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.ToggleWalkEvent += OnToggleWalk;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
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

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.GetCurrentSpeed(), deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            UpdateAnimator(deltaTime, 0);
            return;
        }

        UpdateAnimator(deltaTime, stateMachine.GetCurrentSpeedRatio());

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.ToggleWalkEvent -= OnToggleWalk;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) return;

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingFirstHalfState(stateMachine, false));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }

    private void UpdateAnimator(float deltaTime, float value)
    {
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, value, AnimatorDampTime, deltaTime);
    }
}
