using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private Transform[] _enemyPos;
    [SerializeField]
    private ParticleSystem _particle;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            
            EnemySpawner();
           
            gameObject.GetComponent<BoxCollider>().enabled = false;
            
        }
    }

    void EnemySpawner()
    {
        for(var i = 0; i < _enemyPos.Length; i++)
        {
            Instantiate(_enemy, _enemyPos[i].position, Quaternion.identity);
            _particle = _enemy.GetComponentInChildren<ParticleSystem>();
            _particle?.Stop();
            _particle?.Play();

        }
        
    }
}
