using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _kenParticale;
    private void OnTriggerEnter(Collider other)
    {
        if(TryGetComponent<EnemyController>(out var enemyController))
        {

        }
    }
}
