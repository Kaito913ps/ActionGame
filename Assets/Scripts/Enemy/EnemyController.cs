using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour, IDamagable
{
  
    [SerializeField]
    private int _hp = 100;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

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

    
    public void AddDamge(int damage)
    {
        _hp -= damage;
        if(_hp <= 0)
        {
            _animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
          
            Destroy(this.gameObject, 3f);
        }
        else
        {
            _animator.SetTrigger("damage");
        }
    }



}