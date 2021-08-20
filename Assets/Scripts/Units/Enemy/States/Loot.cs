using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Find a weapon
 *  No weapon in range -> Wander
 *  Else -> Get weapon -> Idle
 */

public class Loot : AIState 
{
    private Vector3 lootPosition;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        AI.lootTarget = AI.FindWeapon(AI.sightRange);
        
        if (AI.StateMachine.GetBool("hasNoAmmo")) {
            AI.lootTarget = AI.FindAnything(AI.sightRange);
        }
        //AI.Inventory.OnInventoryUpdated += CheckInventory;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (AI.lootTarget == null) {
            AI.Movement.Stop();
            AI.StateMachine.SetBool("lootInRange", false);
        }
        else {
            lootPosition = AI.lootTarget.transform.position;
            AI.Movement.MoveToPosition(lootPosition, 
                AI.moveSpeed, 
                AI.rotationSpeed,
                () => AI.StateMachine.SetBool("lootInRange", false));
        }

        // Found weapon
        if (AI.Inventory.equippedWeapon != AI.Inventory.defaultWeapon) {
            AI.StateMachine.SetBool("hasWeapon", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //base.OnStateExit(animator, stateInfo, layerIndex);

        lootPosition = Vector3.zero;
        //AI.Inventory.OnInventoryUpdated -= CheckInventory;
    }

    private void CheckInventory() {
        // Crashes the game, check later
        // Check active ammo stack
        //if (AI.Inventory.GetWeaponCount() < 1 || AI.Inventory.activeAmmoStack == null) return;
        /*
        if (AI.Inventory.activeAmmoStack.itemAmount <= 0) {
            for (int i = 0; i < AI.Inventory.GetWeaponCount(); i++) {
                AI.Inventory.EquipWeaponCycle();
                if (AI.Inventory.activeAmmoStack == null) {
                    continue;
                }
                if (AI.Inventory.activeAmmoStack.itemAmount > 0) {
                    AI.StateMachine.SetBool("hasNoAmmo", false);
                    break;
                }
            }

            // Still nothing?
            if (AI.Inventory.activeAmmoStack.itemAmount <= 0) {
                AI.StateMachine.SetBool("hasNoAmmo", true);
            }
        }
        */
        
    }
}
