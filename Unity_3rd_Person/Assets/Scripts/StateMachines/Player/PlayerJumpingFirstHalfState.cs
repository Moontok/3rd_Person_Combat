using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingFirstHalfState : PlayerBaseState
{
    private readonly int JumpingSpeedHash = Animator.StringToHash("JumpingSpeed");
    private readonly int JumpingFirstHalfBlendTreeHash = Animator.StringToHash("JumpingFirstHalfBlendTree");

    private Vector3 momentum = Vector3.zero;
    private bool isTargeting = false;

    private const float CrossFadeDuration = 0.1f;

    public PlayerJumpingFirstHalfState(PlayerStateMachine stateMachine, bool isTargeting) : base(stateMachine)
    {
        this.isTargeting = isTargeting;
    }

    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;

        if (stateMachine.InputReader.MovementValue == Vector2.zero || isTargeting)
        {
            UpdateAnimator(0);
        }
        else
        {
            UpdateAnimator(stateMachine.GetCurrentSpeedRatio());
        }

        stateMachine.Animator.CrossFadeInFixedTime(JumpingFirstHalfBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerJumpingSecondHalfState(stateMachine));
            return;
        }

        FaceTarget();
    }

    public override void Exit()
    {
    }

    private void UpdateAnimator(float value)
    {
        stateMachine.Animator.SetFloat(JumpingSpeedHash, value);
    }
}
