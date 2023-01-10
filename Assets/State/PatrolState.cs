using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PatrolState : StateMachineBehaviour
{
    float _timer;

    
    List<Transform> _wayPoints = new List<Transform>();
    NavMeshAgent _agent;

    Transform _player;
    float _chaseRange = 8f;


    //OnStateEnter �́A�J�ڂ��J�n����A�X�e�[�g�}�V�������̏�Ԃ̕]�����J�n�����Ƃ��ɌĂяo����܂��B
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = animator.GetComponent<NavMeshAgent>();
        _agent.speed = 1.5f;
        _timer = 0;
        GameObject go = GameObject.FindGameObjectWithTag("Waypoints");
        //waypoint�����񂷂�ꏊ
        foreach (Transform t in go.transform)
            _wayPoints.Add(t);

        _agent.SetDestination(_wayPoints[Random.Range(0, _wayPoints.Count)].position);
    }

    //OnStateUpdate �́AOnStateEnter �R�[���o�b�N�� OnStateExit �R�[���o�b�N�̊Ԃ̊e Update �t���[���ŌĂяo����܂��B
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // �ړI�n�_�܂ł̋���(remainingDistance)���ړI�n�̎�O�܂ł̋���(stoppingDistance)�ȉ��ɂȂ�����
        if (_agent.remainingDistance <= _agent.stoppingDistance)
            _agent.SetDestination(_wayPoints[Random.Range(0, _wayPoints.Count)].position);

        _timer += Time.deltaTime;
        if (_timer > 10)
            animator.SetBool("onPatrolling", false);

        float distance = Vector3.Distance(_player.position, animator.transform.position);
        if (distance < _chaseRange)
            animator.SetBool("onChasing", true);
    }

    // OnStateExit �́A�J�ڂ��I�����A�X�e�[�g �}�V�������̏�Ԃ̕]�����I�������Ƃ��ɌĂяo����܂��B
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_agent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
