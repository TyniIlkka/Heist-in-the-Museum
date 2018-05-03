using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class PlayerMovementIsometric : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeedWalk = 3f;
        [SerializeField]
        private float _moveSpeedSneak = 1f;

        [SerializeField]
        private float _gravity = 20f;
        [SerializeField]
        private Transform _playerTransform;

        public float speed;
        private CharacterController _charCont;
        private Vector3 _movement;
        private Animator _playerAnimator;
        [SerializeField]
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
            _charCont = GetComponent<CharacterController>();
            _playerAnimator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            Move();
            MoveSpeed();
            speed = player.Speed;
        }

        private void Move()
        {
            if (_charCont.isGrounded)
            {
                _movement = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));

                if (_movement != Vector3.zero)
                {
                    Vector3 newRotation = _movement;
                    transform.rotation = Quaternion.LookRotation(newRotation);
                    _playerTransform.rotation = Quaternion.LookRotation(newRotation);

                    float offsetY = _playerTransform.eulerAngles.y;
                    _playerTransform.eulerAngles = new Vector3(_playerTransform.eulerAngles.x,
                        offsetY + 45, _playerTransform.eulerAngles.z);

                    _playerAnimator.SetBool("Moving", true);
                    //TODO: Replace to value
                    _playerAnimator.SetFloat("Speed", player.Speed);
                    //

                    _movement = transform.forward + transform.right;
                    _movement *= player.Speed;
                }
                else
                {
                    _playerAnimator.SetBool("Moving", false);
                }                
            }
            else
            {
                _playerAnimator.SetBool("Moving", false);
            }

            _movement.y -= _gravity * Time.deltaTime;
            _charCont.Move(_movement * Time.deltaTime);
        }

        private void MoveSpeed()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                player.Speed = _moveSpeedSneak;
            }
            else
            {
                player.Speed = _moveSpeedWalk;
            }
        }
    }
}