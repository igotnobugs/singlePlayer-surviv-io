using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : StateMachineBehaviour 
{
    public AI AI { get; set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (AI == null) {
            AI = animator.GetComponent<AI>();
        }

        //AI.target = AI.FindUnit(AI.sightRange);
        //AI.lootTarget = AI.FindLoot(AI.sightRange);
    }
}
