using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoCenter : AIState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //base.OnStateUpdate(animator, stateInfo, layerIndex);

        AI.Movement.MoveToPosition(new Vector3(0, 0),
            AI.moveSpeed,
            AI.rotationSpeed);

        AI.target = AI.FindUnit(AI.sightRange);
        if (AI.target != null) {
            AI.StateMachine.SetBool("enemyInRange", true);
        }

        AI.lootTarget = AI.FindAnything(AI.sightRange);
        if (AI.lootTarget != null) {
            AI.StateMachine.SetBool("lootInRange", true);
        }


    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AI.Movement.Stop();
        AI.StateMachine.SetFloat("gameTime", 15);
    }
}
