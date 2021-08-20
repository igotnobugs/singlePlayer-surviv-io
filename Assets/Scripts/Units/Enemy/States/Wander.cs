using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : AIState
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        AI.target = AI.FindUnit(AI.sightRange);
        AI.lootTarget = AI.FindWeapon(AI.sightRange);
        AI.StartWandering();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //base.OnStateUpdate(animator, stateInfo, layerIndex);
        
        if (AI.target != null) {
            AI.StateMachine.SetBool("enemyInRange", true);
        } else {
            AI.target = AI.FindUnit(AI.sightRange);
            AI.StateMachine.SetBool("enemyInRange", false);
        }

        if (AI.lootTarget != null) {
            AI.StateMachine.SetBool("lootInRange", true);
        } else {
            AI.lootTarget = AI.FindAnything(AI.sightRange);
            AI.StateMachine.SetBool("lootInRange", false);
        }

        if (AI.Inventory.equippedWeapon != AI.Inventory.defaultWeapon) {
            AI.StateMachine.SetBool("hasWeapon", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //base.OnStateExit(animator, stateInfo, layerIndex);
        AI.StopWandering();
        AI.Movement.Stop();
    }
}
