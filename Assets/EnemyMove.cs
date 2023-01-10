using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStatus))]
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMove : MonoBehaviour
{
    private EnemyStatus _state;
    private NavMeshAgent _navMeshAgent;
    void Start()
    {
        _state = GetComponent<EnemyStatus>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDetectObject(Collider collider)
    {
        if(!_state.IsMovale)
        {
            _navMeshAgent.isStopped = true;
        }
    }
}
