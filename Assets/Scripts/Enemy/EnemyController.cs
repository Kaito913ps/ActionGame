using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour
{
  
    [SerializeField]
    private int _hp = 200;

    int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
        }
    }

    

   

}