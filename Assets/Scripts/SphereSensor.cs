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

            //�p�x�����߂�
            var angle = Vector3.Angle(transform.forward, playerDirection);

            if (angle <= _searchAngle)
            {
                //�v���C���[�Ƃ̊Ԃɏ�Q�����Ȃ��Ƃ�
                if (!Physics.Linecast(transform.position + Vector3.up, other.transform.position + Vector3.up, _obstacleLayer))
                {
                    if (Vector3.Distance(other.transform.position, transform.position) <= _searchArea.radius * 0.5f && Vector3.Distance(other.transform.position, transform.position) >= _searchArea.radius * 0.05f)
                    {
                        //�Z���T�[�ɓ������v���C���[���^�[�Q�b�g�ɐݒ肵��,�ǐՏ�ԂɈڍs����.�B
                        _slimemove.SetState(SlimeController.EnemyState.Attack);
                    }
                    else if(Vector3.Distance(other.transform.position, transform.position) <= _searchArea.radius && Vector3.Distance(other.transform.position, transform.position) >= _searchArea.radius *0.5f && _slimemove._state == SlimeController.EnemyState.Idle)
                    {
                        // �Z���T�[�ɓ������v���C���[���^�[�Q�b�g�ɐݒ肵�āA�ǐՏ�ԂɈڍs����B
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
    //�T�[�`����p�x�\��
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -_searchAngle, 0f) * transform.forward, _searchAngle * 2f, _searchArea.radius);
    }
    #endif
}
