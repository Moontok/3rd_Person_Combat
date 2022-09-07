using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallingHash = Animator.StringToHash("falling");

    private Vector3 momentum = Vector3.zero;

    private const float CrossFadeDuration = 0.1f;

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;

        stateMachine.Animator.CrossFadeInFixedTime(FallingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.isGrounded)
            ReturnToLocomotion();

        FaceTarget();
    }

    public override void Exit()
    {
    }
}
