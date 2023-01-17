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

    //　プレイヤーの位置を追跡します。
    public void OnDetectObject(Collider collider)
    {
        if(!_state.IsMovale)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        //検知したオブジェクトに「Player」のタグがついていれば、そのオブジェクトを追いかける
        if(collider.CompareTag("Player"))
        {
            var positionDiff = collider.transform.position - transform.position;
            var distance = positionDiff.magnitude;
            var direction = positionDiff.normalized;
            var hitCount = Physics.RaycastNonAlloc(transform.position, direction, _raycastHits, distance, _raycastLayerMask);

            Debug.Log($"hitCount:{hitCount}");

            //　プレイヤーとの間に障害物がないかチェックする。
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
