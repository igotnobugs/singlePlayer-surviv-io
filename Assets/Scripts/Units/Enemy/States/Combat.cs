using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Find target or enemy
 * Fight to the death
 * 
 * If target is dead -> Wander
 * 
 */


public class Combat : AIState
{
    [SerializeField] private Transform targetTransform;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        AI.target = AI.FindUnit(AI.sightRange);

        if (AI.target != null)
            targetTransform = AI.target.transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (targetTransform == null) {
            AI.StateMachine.SetBool("enemyInRange", false);
            return;
        }

        // Look and shoot       
        Vector3 dir = targetTransform.position - AI.transform.position;
        float distanceToTarget = Vector3.Distance(AI.transform.position, targetTransform.position);

        if (distanceToTarget <= AI.shootRange) {
            AI.Movement.Stop();
            AI.Movement.LookAt(dir, AI.rotationSpeed);
            AI.ShootWeapon(AI.Inventory.equippedWeapon);
        }
        else {
            if (distanceToTarget <= AI.chaseRange) {
                AI.Movement.MoveToPosition(targetTransform.position,
                    AI.moveSpeed,
                    AI.rotationSpeed);
            }
            else {
                AI.StateMachine.SetBool("enemyInRange", false);
            }
        }

        // Cycle weapon when out of ammo       
        if (AI.Inventory.activeAmmoStack.itemAmount <= 0) {
            for (int i = 0; i < AI.Inventory.GetWeaponCount(); i++) {
                AI.Inventory.EquipWeaponCycle(true);
                if (AI.Inventory.activeAmmoStack.itemAmount > 0) {
                    AI.StateMachine.SetBool("hasNoAmmo", false);
                    continue;
                }
            }

            // Still nothing?
            if (AI.Inventory.activeAmmoStack.itemAmount <= 0) {
                AI.StateMachine.SetBool("hasNoAmmo", true);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //base.OnStateExit(animator, stateInfo, layerIndex);
        AI.Movement.Stop();
        targetTransform = null;
    }
}
