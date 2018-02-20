using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.PathFinding;

namespace ProjectThief {

    public enum MyDirections
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public class Guard : CharacterBase {

        [SerializeField]
        Player player;
        [SerializeField, Range(0, 100)]
        private float m_fDetectionRange;

        [SerializeField, Tooltip("if True, guard is moving, otherwise static")]
        private bool m_bMoving;

        [SerializeField]
        private float m_fMovementSpeed;
        [SerializeField]
        private float m_fTurnSpeed;

        private float step;

        private Guard guard;
        private Vector3 m_vTargetPosition;

        [SerializeField, Tooltip("Which Direction Guard is looking if not patrolling: ")]
        private MyDirections m_eDirection;
        private Vector3 m_vDirection;
        private float m_fHeadingAngle;

        [SerializeField, Tooltip("Position for static guard.")]
        private Vector3 m_vPosition;


        public void Start()
        {
            guard = GetComponent<Guard>();
            
        }

        private void Update()
        {
            switch (m_eDirection)
            {
                case MyDirections.North:
                    transform.forward = new Vector3(0f, 0f, 1f);
                    break;
                case MyDirections.East:
                    transform.forward = new Vector3(1f, 0f, 0f);
                    break;
                case MyDirections.South:
                    transform.forward = new Vector3(0f, 0f, -1f);
                    break;
                case MyDirections.West:
                    transform.forward = new Vector3(-1f, 0f, 0f);
                    break;
            }
        }

        private void FixedUpdate()
        {
            CanSeePlayer();
        }

        public bool CanSeePlayer()
        {
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position - transform.position;

            if (Physics.Raycast (transform.position, rayDirection, out hit, m_fDetectionRange))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    Debug.Log("Player is detected, but no end imported");
                }
                Debug.DrawLine(transform.position, hit.point, Color.red);
                Debug.Log("Hit something");
                //TODo: To GameManager send player detected!
                
            }

            return true;
        }

        /// <summary>
        /// Move method to 
        /// </summary>
        public override void Move(Vector3 direction)
        {
            

        }

        /// <summary>
        /// Turn method
        /// </summary>
        public override void Turn(Vector3 target)
        {
            Vector3 direction = target - transform.position;
            direction.y = transform.position.y;
            direction = direction.normalized;
            float turnSpeedRad = Mathf.Deg2Rad * m_fTurnSpeed * Time.deltaTime;
            Vector3 rotation = Vector3.RotateTowards(transform.forward,
                direction, turnSpeedRad, 0f);
            transform.rotation = Quaternion.LookRotation(rotation, transform.up);
        }

    }
}