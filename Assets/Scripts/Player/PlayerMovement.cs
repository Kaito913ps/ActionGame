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

    // カレント移動方向
    private Vector3 _moveDirection = Vector3.zero;
    // カレント垂直方向速度
    private float _verticalSpeed = 0.0f;
    //カレント水平方向速度
    private float _moveSpeed = 0.0f;

    // controller.Move が返すコリジョンフラグ
    private CollisionFlags _collisionFlags;

    //歩き始める速度
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
        // cameraのx-z平面からforward ベクターを求める
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;

        forward = forward.normalized;
        // 右方向ベクターは常にforwardに直交
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float InputZ = Input.GetAxisRaw("Vertical");
        float InputX = Input.GetAxisRaw("Horizontal");

        //カメラと連動した進行方向を得る
        Vector3 targetDirection = InputX * right + InputZ * forward;

        //接地
        if ((_collisionFlags & CollisionFlags.CollidedBelow) != 0)
        {
            //順方向ではない？
            if (targetDirection != Vector3.zero)
            {
                //ゆっくり移動か？
                if (_moveSpeed < _wlklSpeed * 0.9)
                {
                    //即時ターン
                    _moveDirection = targetDirection.normalized;
                }
                else
                {
                    //スムーズにターン
                    _moveDirection = Vector3.RotateTowards(_moveDirection, targetDirection, _rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
                    _moveDirection = _moveDirection.normalized;
                }
            }

            //向きをスムーズに変更
            float curSmooth = _speedSmoothing * Time.deltaTime;
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

            if (Time.time - _runAfterSeconds > _walkTimeStart)
                targetSpeed *= _runSpeed;
            else
                targetSpeed *= _wlklSpeed;

            _moveSpeed = Mathf.Lerp(_moveSpeed, targetSpeed, curSmooth);

            // Animatorに移動速度のパラメータを渡す
            _animator.SetFloat("spd", _moveSpeed);
            _animator.SetBool("fall", false);

            if (_moveSpeed < _wlklSpeed * 0.3)
                _walkTimeStart = Time.time;
            _verticalSpeed = 0.0f;
        }
        else ///浮いている
        {
            //重力を適応
            _verticalSpeed -= _garavity * Time.deltaTime;
            if (_verticalSpeed < -4.0)
            {
                _animator.SetBool("fall", true);
            }
        }

        //移動量を計算
        _movement = _moveDirection * _moveSpeed + new Vector3(0, _verticalSpeed, 0);
        _movement *= Time.deltaTime;

        _collisionFlags = _characterController.Move(_movement);//キャラを移動

        //接地していると移動方向に回転
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
