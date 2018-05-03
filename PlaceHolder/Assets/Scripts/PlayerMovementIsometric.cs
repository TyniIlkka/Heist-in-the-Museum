using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class PlayerMovementIsometric : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed = 5f;
        [SerializeField]
        private float _gravity = 20f;
        [SerializeField]
        private Transform _playerTransform;

        private CharacterController _charCont;
        private Vector3 _movement;
        private Animator _playerAnimator;

        private void Awake()
        {
            _charCont = GetComponent<CharacterController>();
            _playerAnimator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (_charCont.isGrounded)
            {
                _movement = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

                if (_movement != Vector3.zero)
                {
                    Vector3 newRotation = _movement;
                    transform.rotation = Quaternion.LookRotation(newRotation);
                    _playerTransform.rotation = Quaternion.LookRotation(newRotation);

                    float offsetY = _playerTransform.eulerAngles.y;
                    _playerTransform.eulerAngles = new Vector3(_playerTransform.eulerAngles.x,
                        offsetY + 45, _playerTransform.eulerAngles.z);

                    _playerAnimator.SetBool("Moving", true);
                    _movement = transform.forward + transform.right;
                    _movement *= _moveSpeed;
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
    }
}