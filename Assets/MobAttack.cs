using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MobStatus))]

public class MobAttack : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private Collider _attackCollider;

    private MobStatus _status;

    private void Start()
    {
        _status = GetComponent<MobStatus>();
    }

    public void AttackIfPossible()
    {
        if (!_status.IsAttackable)
            return;
        _status.GoToAttackStateIfPossible();
    }
    public void OnAttackRangeEnter(Collider collider)
    {
        AttackIfPossible();
    }

    public void OnAttackStart()
    {
        _attackCollider.enabled = true;
    }

    public void OnHitAttack(Collider collider)
    {
        var targetMob = collider.GetComponent<MobStatus>();
        if (null == targetMob) 
            return;

        targetMob.Damage(1);
    }

    public void OnAttackFinished()
    {
        _attackCollider.enabled = false;
        StartCoroutine(CooldownCoroutine());
    }
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _status.GoToAttackStateIfPossible();
    }
}
