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

        private ParticleSystem particleSystem;
        private CharacterController _charCont;
        private Vector3 _movement;
        private Animator _playerAnimator;
        [SerializeField]
        private Player player;

        private void Awake()
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
            player = GetComponent<Player>();
            _charCont = GetComponent<CharacterController>();
            _playerAnimator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            Move();
            MoveSpeed();
            //RippleEffect();
            speed = player.Speed;
        }

        private void Move()
        {
            if (GameManager.instance.canMove)
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
                        _playerAnimator.SetFloat("Speed", player.Speed);

                        if (player.Speed > _moveSpeedSneak)
                        {
                            particleSystem.transform.localScale = new Vector3(3, 3, 3);
                        }
                        else
                        {
                            particleSystem.transform.localScale = new Vector3(1, 1, 1);
                        }
                        _movement = transform.forward + transform.right;
                        _movement *= player.Speed;
                    }
                    else
                    {
                        particleSystem.transform.localScale = new Vector3(0, 0, 0);
                        _playerAnimator.SetBool("Moving", false);
                        
                    }
                }
                else
                {
                    particleSystem.transform.localScale = new Vector3(0, 0, 0);
                    _playerAnimator.SetBool("Moving", false);
                }

                _movement.y -= _gravity * Time.deltaTime;
                _charCont.Move(_movement * Time.deltaTime);
            }
            else
            {
                _movement = new Vector3(0,0,0d);
            }
        }

        private void MoveSpeed()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                player.Speed = _moveSpeedSneak;
                particleSystem.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                player.Speed = _moveSpeedWalk;
            }
        }

        private void RippleEffect()
        {
            var main = particleSystem.main;
            if (player.Speed > _moveSpeedSneak)
            {
                main.startSize = 3f;
            }
            else if (player.Speed <= _moveSpeedSneak && player.Speed > 0.99f)
            {
                main.startSize = 1f;
            }
            else
            {

                main.startSize = 0f;
                //soundWaves.Stop();
            }
        }
    }
}