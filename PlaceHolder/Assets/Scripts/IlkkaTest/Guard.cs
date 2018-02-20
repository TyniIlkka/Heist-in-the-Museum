using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.PathFinding;

namespace ProjectThief {
    public class Guard : CharacterBase {

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
        private int m_iDirection;
        [SerializeField, Tooltip("Position for static guard.")]
        private Vector3 m_vPosition;


        public int Direction
        {
            get { return m_iDirection; }
            set { m_iDirection = value; }
        }

        public Vector3 Position
        {
            get { return m_vPosition; }
            set { m_vPosition = value; }
        }

        public void Start()
        {
            guard = GetComponent<Guard>();
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