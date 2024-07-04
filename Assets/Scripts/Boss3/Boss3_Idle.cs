using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Idle : StateMachineBehaviour {
    [SerializeField] float attackCooldown = 5f;
    float attackTimer;
    Boss3_Attack boss3_Attack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        attackTimer = attackCooldown;
        boss3_Attack = animator.GetComponent<Boss3_Attack>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0) {
            int i = Random.Range(1, 5);
            if (i == 1) {
                animator.SetTrigger("AttackLeft");
            }
            if (i == 2) {
                animator.SetTrigger("AttackRight");
            }
            if (i == 3 || i == 4) {
                boss3_Attack.Charge();
            }
            attackTimer = attackCooldown;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger("AttackLeft");
        animator.ResetTrigger("AttackRight");
    }
}
