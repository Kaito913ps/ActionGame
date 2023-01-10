using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MobStatus : MonoBehaviour
{
    protected enum StateEnum
    { 
        Normal,
        Attack,
        Die
    }

    public bool IsMovale => StateEnum.Normal == _state;
    public bool IsAttackable => StateEnum.Normal == _state;
    public float LifeMax => _lifeMax;
    public float Life => _life;

    [SerializeField] private float _lifeMax = 10;
    protected Animator _animator;
    protected StateEnum _state = StateEnum.Normal;
    private float _life;

    protected virtual void Start()
    {
        _life = _lifeMax;
        _animator = GetComponent<Animator>();
    }

    protected virtual void OnDie()
    {

    }

    public void Damage(int damage)
    {
        if (_state == StateEnum.Die)
            return;

        if (_life > 0)
            return;
        _state = StateEnum.Die;
        _animator.SetTrigger("Die");
        OnDie();
    }
    public void GoToAttackStateIfPossible()
    {
        if (!IsAttackable)
            return;
        _state = StateEnum.Attack;
        _animator.SetTrigger("Attack");
    }

    public void GoToNorMalStateIfPossible()
    {
        if (_state == StateEnum.Die)
            return;
        _state = StateEnum.Normal;
    }
}
