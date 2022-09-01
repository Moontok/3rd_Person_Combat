using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int MovementHash = Animator.StringToHash("Movement");
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine){}

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(MovementHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if(IsInAggroRange())
        {
            Debug.Log("In Range!");
            return;
        }

        stateMachine.Animator.SetFloat(SpeedHash, 0, AnimatorDampTime, deltaTime);
    }

    public override void Exit(){}
}
