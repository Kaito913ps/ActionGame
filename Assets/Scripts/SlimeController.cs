using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class SlimeController : MonoBehaviour
{
    // キャラ状態の定着
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
        //プレイヤーを目的地にして追跡する
        if(_state == EnemyState.Chase)
        {
            if(_target == null)
            {
                SetState(EnemyState.Idle);
            }
            else
            {
                SetDestination(_target.position);
                //目指す座標
                _navMeshAgent.SetDestination(GetDestination());
            }
            // 敵の向きをプレイヤーの方向に少しづつ変える
            var dir = (GetDestination() - transform.position).normalized;
            dir.y = 0;
            //LookRotationはある方向を向かせるためのQuaternion(向きたい方向,頭上方向（省略可） )
            Quaternion setRotation = Quaternion.LookRotation(dir);
            //算出した方向の角度を敵の角度に設定
            transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, _navMeshAgent.angularSpeed * 0.1f * Time.deltaTime);
        }
    }

    /// <summary>
    /// 敵キャラの状態を設定するためのメソッド
    /// </summary>
    /// <param name="tempState"></param>
    /// <param name="targetObject"></param>
    public void SetState(EnemyState tempState, Transform targetObject = null)
    {
        _state = tempState;

        //Idleモード
        if(tempState == EnemyState.Idle)
        {
            //キャラの移動を止める
            _navMeshAgent.isStopped = true;
            _animator.SetBool("chase", false);
        }

        //chaseモード
        else if(tempState == EnemyState.Chase)
        {
            //ターゲットの情報を更新
            _target = targetObject;
            //目的地をターゲットの位置に設定
            _navMeshAgent.SetDestination(_target.position);
            //キャラを動けるようにする
            _navMeshAgent.isStopped = false;
            _animator.SetBool("chase", true);
        }

        //Attackモード
        else if (tempState == EnemyState.Attack)
        {
            //キャラの移動を止める
            _navMeshAgent.isStopped = true;
            _animator.SetBool("chase", false);
            //攻撃用のアニメーション
            _timeline.Play();
        }

        else if (tempState == EnemyState.Freeze)
        {
            Invoke("ResetState", 2.0f);
        }
    }

    /// <summary>
    ///  敵キャラの状態を取得するためのメソッド
    /// </summary>
    /// <returns></returns>
    public EnemyState GetState()
    {
        return _state;
    }
    /// <summary>
    /// 状態をFreeze状態に設定するためのメソッド
    /// </summary>
    public void FreezeState()
    {
        SetState(EnemyState.Freeze);
    }

    //　状態をIdle状態に設定するためのメソッド
    private void ResetState()
    {
        SetState(EnemyState.Idle); ;
    }

    /// <summary>
    /// 目的地を設定する
    /// </summary>
    /// <param name="position"></param>
    public void SetDestination(Vector3 position)
    {
        _destination = position;
    }

    /// <summary>
    /// 目的地を取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDestination()
    {
        return _destination;
    }
}
