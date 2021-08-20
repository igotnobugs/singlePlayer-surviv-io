using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AIState
{  
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        AI.Movement.Stop();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (AI.Inventory.equippedWeapon == AI.Inventory.defaultWeapon) {
            if (AI.Inventory.GetWeaponCount() > 1) {
                // Cycle weapon?
            }
            else {
                AI.StateMachine.SetBool("hasWeapon", false);
            }
        } else {
            AI.StateMachine.SetBool("hasWeapon", true);
        }

    }

}
