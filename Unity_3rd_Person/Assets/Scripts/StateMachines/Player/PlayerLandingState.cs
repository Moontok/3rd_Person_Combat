using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    private readonly int FallingToLandingHash = Animator.StringToHash("falling_to_landing");
    private readonly int FallingToRollHash = Animator.StringToHash("falling_to_roll");

    private Vector3 momentum = Vector3.zero;

    private const float CrossFadeDuration = 0.1f;

    public PlayerLandingState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;

        if (momentum.x > 0)
            stateMachine.Animator.CrossFadeInFixedTime(FallingToRollHash, CrossFadeDuration);
        else
            stateMachine.Animator.CrossFadeInFixedTime(FallingToLandingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !stateMachine.Animator.IsInTransition(0))
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
    }
}
