using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SphereSensor : MonoBehaviour
{
    [SerializeField]
    private SphereCollider _searchArea = default;
    [SerializeField]
    private float _searchAngle = 45f;

    private LayerMask _obstacleLayer = default;
    private SlimeController _slimemove = default;

    void Start()
    {
        _slimemove = transform.parent.GetComponent<SlimeController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var playerDirection = other.transform.position - transform.position;

            //角度を求める
            var angle = Vector3.Angle(transform.forward, playerDirection);

            if (angle <= _searchAngle)
            {
                //プレイヤーとの間に障害物がないとき
                if (!Physics.Linecast(transform.position + Vector3.up, other.transform.position + Vector3.up, _obstacleLayer))
                {
                    if (Vector3.Distance(other.transform.position, transform.position) <= _searchArea.radius * 0.5f && Vector3.Distance(other.transform.position, transform.position) >= _searchArea.radius * 0.05f)
                    {
                        //センサーに入ったプレイヤーをターゲットに設定して,追跡状態に移行する.。
                        _slimemove.SetState(SlimeController.EnemyState.Attack);
                    }
                    else if(Vector3.Distance(other.transform.position, transform.position) <= _searchArea.radius && Vector3.Distance(other.transform.position, transform.position) >= _searchArea.radius *0.5f && _slimemove._state == SlimeController.EnemyState.Idle)
                    {
                        // センサーに入ったプレイヤーをターゲットに設定して、追跡状態に移行する。
                        _slimemove.SetState(SlimeController.EnemyState.Chase, other.transform);
                    }
                }
            }
            else if (angle > _searchAngle)
            {
                _slimemove.SetState(SlimeController.EnemyState.Idle);
            }
        }
    }

#if UNITY_EDITOR
    //サーチする角度表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -_searchAngle, 0f) * transform.forward, _searchAngle * 2f, _searchArea.radius);
    }
    #endif
}
