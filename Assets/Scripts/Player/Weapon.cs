using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _kenParticale;

    [SerializeField]
    private int _attack = 10;

    BoxCollider _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject.GetComponent<IDamagable>();
        
        if(hit != null)
        {
            hit.AddDamge(_attack);
        }
    }
    
}
