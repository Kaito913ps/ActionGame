using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    float _timer;
    Transform _player;

    [SerializeField]
    float _ChaseRange = 8f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer = 0;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ƒpƒgƒ[ƒ‹
        _timer += Time.deltaTime;
        if (_timer > 5)
            animator.SetBool("onPatrolling", true);

        float distance = Vector3.Distance(_player.position, animator.transform.position);

        //player‚Æ‚Ì‹——£‚ª‹ß‚Ã‚¢‚½‚ç
        if (distance < _ChaseRange)
            animator.SetBool("onChasing", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
