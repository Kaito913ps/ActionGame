using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class SlimeController : MonoBehaviour
{
    // �L������Ԃ̒蒅
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Freeze
    };

    private Transform _target;
    public EnemyState _state;
    private NavMeshAgent _navMeshAgent;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private PlayableDirector _timeline;
    private Vector3 _destination;
    

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        SetState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[��ړI�n�ɂ��ĒǐՂ���
        if(_state == EnemyState.Chase)
        {
            if(_target == null)
            {
                SetState(EnemyState.Idle);
            }
            else
            {
                SetDestination(_target.position);
                //�ڎw�����W
                _navMeshAgent.SetDestination(GetDestination());
            }
            // �G�̌������v���C���[�̕����ɏ����Âς���
            var dir = (GetDestination() - transform.position).normalized;
            dir.y = 0;
            //LookRotation�͂���������������邽�߂�Quaternion(������������,��������i�ȗ��j )
            Quaternion setRotation = Quaternion.LookRotation(dir);
            //�Z�o���������̊p�x��G�̊p�x�ɐݒ�
            transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, _navMeshAgent.angularSpeed * 0.1f * Time.deltaTime);
        }
    }

    /// <summary>
    /// �G�L�����̏�Ԃ�ݒ肷�邽�߂̃��\�b�h
    /// </summary>
    /// <param name="tempState"></param>
    /// <param name="targetObject"></param>
    public void SetState(EnemyState tempState, Transform targetObject = null)
    {
        _state = tempState;

        //Idle���[�h
        if(tempState == EnemyState.Idle)
        {
            //�L�����̈ړ����~�߂�
            _navMeshAgent.isStopped = true;
            _animator.SetBool("chase", false);
        }

        //chase���[�h
        else if(tempState == EnemyState.Chase)
        {
            //�^�[�Q�b�g�̏����X�V
            _target = targetObject;
            //�ړI�n���^�[�Q�b�g�̈ʒu�ɐݒ�
            _navMeshAgent.SetDestination(_target.position);
            //�L�����𓮂���悤�ɂ���
            _navMeshAgent.isStopped = false;
            _animator.SetBool("chase", true);
        }

        //Attack���[�h
        else if (tempState == EnemyState.Attack)
        {
            //�L�����̈ړ����~�߂�
            _navMeshAgent.isStopped = true;
            _animator.SetBool("chase", false);
            //�U���p�̃A�j���[�V����
            _timeline.Play();
        }

        else if (tempState == EnemyState.Freeze)
        {
            Invoke("ResetState", 2.0f);
        }
    }

    /// <summary>
    ///  �G�L�����̏�Ԃ��擾���邽�߂̃��\�b�h
    /// </summary>
    /// <returns></returns>
    public EnemyState GetState()
    {
        return _state;
    }
    /// <summary>
    /// ��Ԃ�Freeze��Ԃɐݒ肷�邽�߂̃��\�b�h
    /// </summary>
    public void FreezeState()
    {
        SetState(EnemyState.Freeze);
    }

    //�@��Ԃ�Idle��Ԃɐݒ肷�邽�߂̃��\�b�h
    private void ResetState()
    {
        SetState(EnemyState.Idle); ;
    }

    /// <summary>
    /// �ړI�n��ݒ肷��
    /// </summary>
    /// <param name="position"></param>
    public void SetDestination(Vector3 position)
    {
        _destination = position;
    }

    /// <summary>
    /// �ړI�n���擾����
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDestination()
    {
        return _destination;
    }
}
