using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStatus))]
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private LayerMask _raycastLayerMask;
    private EnemyStatus _state;
    private NavMeshAgent _navMeshAgent;
    private RaycastHit[] _raycastHits = new RaycastHit[10];
    void Start()
    {
        _state = GetComponent<EnemyStatus>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    //�@�v���C���[�̈ʒu��ǐՂ��܂��B
    public void OnDetectObject(Collider collider)
    {
        if(!_state.IsMovale)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        //���m�����I�u�W�F�N�g�ɁuPlayer�v�̃^�O�����Ă���΁A���̃I�u�W�F�N�g��ǂ�������
        if(collider.CompareTag("Player"))
        {
            var positionDiff = collider.transform.position - transform.position;
            var distance = positionDiff.magnitude;
            var direction = positionDiff.normalized;
            var hitCount = Physics.RaycastNonAlloc(transform.position, direction, _raycastHits, distance, _raycastLayerMask);

            Debug.Log($"hitCount:{hitCount}");

            //�@�v���C���[�Ƃ̊Ԃɏ�Q�����Ȃ����`�F�b�N����B
            if (hitCount == 0)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.destination = collider.transform.position;
            }
            else
            {
                _navMeshAgent.isStopped = true;
            }
        }
    }
}
