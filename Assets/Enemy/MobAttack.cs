using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MobStatus))]

public class MobAttack : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private Collider _attackCollider;
    [SerializeField] private int _damage = 10;
    private MobStatus _status;

    private void Start()
    {
        _status = GetComponent<MobStatus>();
    }

    public void AttackIfPossible()
    {
        // ステータスと衝突したオブジェクトで攻撃可否を判断
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
        //if(collider != null)
        //{
            var targetMob = collider.GetComponent<MobStatus>();
            if (null == targetMob)
                return;

            targetMob.Damage(_damage);
        //}
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
