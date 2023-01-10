using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _wlklSpeed = 2.0f;
    [SerializeField]
    private float _runSpeed = 4.0f;
    [SerializeField]
    private Collider _WeaponCollider;

    private Vector3 _movement;

    private float _garavity = 20.0f;
    private float _speedSmoothing = 10.0f;
    private float _rotateSpeed = 500.0f;
    private float _runAfterSeconds = 0.1f;

    // �J�����g�ړ�����
    private Vector3 _moveDirection = Vector3.zero;
    // �J�����g�����������x
    private float _verticalSpeed = 0.0f;
    //�J�����g�����������x
    private float _moveSpeed = 0.0f;

    // controller.Move ���Ԃ��R���W�����t���O
    private CollisionFlags _collisionFlags;

    //�����n�߂鑬�x
    private float _walkTimeStart = 0.0f;

    private CharacterController _characterController;
    private Animator _animator;


    void Start()
    {
        _moveDirection = transform.TransformDirection(Vector3.forward);
        _characterController = this.GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Attack();
    }

    public void Move()
    {
        Transform cameraTransform = Camera.main.transform;
        // camera��x-z���ʂ���forward �x�N�^�[�����߂�
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;

        forward = forward.normalized;
        // �E�����x�N�^�[�͏��forward�ɒ���
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float InputZ = Input.GetAxisRaw("Vertical");
        float InputX = Input.GetAxisRaw("Horizontal");

        //�J�����ƘA�������i�s�����𓾂�
        Vector3 targetDirection = InputX * right + InputZ * forward;

        //�ڒn
        if ((_collisionFlags & CollisionFlags.CollidedBelow) != 0)
        {
            //�������ł͂Ȃ��H
            if (targetDirection != Vector3.zero)
            {
                //�������ړ����H
                if (_moveSpeed < _wlklSpeed * 0.9)
                {
                    //�����^�[��
                    _moveDirection = targetDirection.normalized;
                }
                else
                {
                    //�X���[�Y�Ƀ^�[��
                    _moveDirection = Vector3.RotateTowards(_moveDirection, targetDirection, _rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
                    _moveDirection = _moveDirection.normalized;
                }
            }

            //�������X���[�Y�ɕύX
            float curSmooth = _speedSmoothing * Time.deltaTime;
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

            if (Time.time - _runAfterSeconds > _walkTimeStart)
                targetSpeed *= _runSpeed;
            else
                targetSpeed *= _wlklSpeed;

            _moveSpeed = Mathf.Lerp(_moveSpeed, targetSpeed, curSmooth);

            // Animator�Ɉړ����x�̃p�����[�^��n��
            _animator.SetFloat("spd", _moveSpeed);
            _animator.SetBool("fall", false);

            if (_moveSpeed < _wlklSpeed * 0.3)
                _walkTimeStart = Time.time;
            _verticalSpeed = 0.0f;
        }
        else ///�����Ă���
        {
            //�d�͂�K��
            _verticalSpeed -= _garavity * Time.deltaTime;
            if (_verticalSpeed < -4.0)
            {
                _animator.SetBool("fall", true);
            }
        }

        //�ړ��ʂ��v�Z
        _movement = _moveDirection * _moveSpeed + new Vector3(0, _verticalSpeed, 0);
        _movement *= Time.deltaTime;

        _collisionFlags = _characterController.Move(_movement);//�L�������ړ�

        //�ڒn���Ă���ƈړ������ɉ�]
        if ((_collisionFlags & CollisionFlags.CollidedBelow) != 0)
        {
            transform.rotation = Quaternion.LookRotation(_moveDirection);
        }
    }

    public void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("Slash", true);
        }
    }

    void WeaponON()
    {
        _WeaponCollider.enabled = true;
    }

    void WeaponOFF()
    {
        _WeaponCollider.enabled = false;
        _animator.SetBool("Slash", false);
    }
}
