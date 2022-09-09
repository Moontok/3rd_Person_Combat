using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingSecondHalfState : PlayerBaseState
{
    private readonly int JumpingSecondHalfBlendTreeHash = Animator.StringToHash("JumpingSecondHalfBlendTree");

    private Vector3 momentum = Vector3.zero;
    private float airTime = 0;

    private const float CrossFadeDuration = 0.1f;

    public PlayerJumpingSecondHalfState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;

        stateMachine.Animator.CrossFadeInFixedTime(JumpingSecondHalfBlendTreeHash, CrossFadeDuration);

        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);
        airTime += deltaTime;

        if (stateMachine.Controller.isGrounded)
            ReturnToLocomotion();
        else if (airTime >= stateMachine.StartFallingTime)
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }
}
