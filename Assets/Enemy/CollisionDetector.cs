using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CollisionDetector : MonoBehaviour
{
    [SerializeField]private TriggerEvent onTriggerEnter = new TriggerEvent();
    [SerializeField] private TriggerEvent onTriggerStay = new TriggerEvent();
    

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke(other);
    }
    [Serializable]
    public class TriggerEvent:UnityEvent<Collider>
    {

    }
}