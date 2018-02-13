using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.PathFinding;

namespace ProjectThief {
    public class Player : CharacterBase
    {
        [SerializeField]
        private float m_fMovementSpeed;
        [SerializeField]
        private float m_fTurnSpeed;
        [SerializeField]
        private GameObject m_gMoveHere;
        [SerializeField]
        private PathGridManager m_pgmGrid;

        private Node currentNode;
        private Vector3 m_vTargetPosition;

        private void Awake()
        {
            if (m_gMoveHere == null)
            {
                Debug.Log("Your Target point is Missing!");
            }
        }

        // Update is called once per frame
        void Update()
        {
            m_vTargetPosition = m_pgmGrid.CurrentWaypoint.m_vPosition;
        }

        private Node GetWaypoint()
        {
            Node result = m_pgmGrid.CurrentWaypoint;
            Vector3 toWaypointVector = m_pgmGrid.CurrentWaypoint.m_vPosition - transform.position;
            float toWaypointSqr = toWaypointVector.sqrMagnitude;
            float sqrArriveDistance = _arriveDistance * _arriveDistance;
            if (toWaypointSqr <= sqrArriveDistance)
            {
                result = m_pgmGrid.Path.GetNextWaypoint(CurrentWaypoint, ref _direction);
            }

            return result;
        }

        /// <summary>
        /// Move method to 
        /// </summary>
        public override void Move(Vector3 direction)
        {
            direction = direction.normalized;
            Vector3 position = transform.position + direction * m_fMovementSpeed * Time.deltaTime;
            transform.position = position;
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
