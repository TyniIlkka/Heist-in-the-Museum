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
        [SerializeField]
        private float _arriveDistance;

        Player player;
        Coroutine MoveIE;
        private List<Node> pathList;

        private Node currentNode;
        private Vector3 m_vTargetPosition;

        private void Start()
        {
            
            //m_pgmGrid.CurrentWaypoint = m_pgmGrid.GetFirstNodeOnPath();
            //if (m_pgmGrid.CurrentWaypoint == null)
            //{
                //Debug.Log("Your Target point is Missing!");
            //}
        }

        // Update is called once per frame
        void Update()
        {
            player = GetComponent<Player>();
            pathList = m_pgmGrid.Path;
            StartCoroutine(moveObject());
            //Move(transform.forward);
            //Turn(currentNode.m_vPosition);
        }

        private Node GetWaypoint()
        {
            Node result = m_pgmGrid.CurrentWaypoint;
            Vector3 toWaypointVector = m_pgmGrid.CurrentWaypoint.m_vPosition - transform.position;
            float toWaypointSqr = toWaypointVector.sqrMagnitude;
            float sqrArriveDistance = _arriveDistance * _arriveDistance;
            if (toWaypointSqr <= sqrArriveDistance)
            {
                result = m_pgmGrid.GetNextWaypoint(m_pgmGrid.CurrentWaypoint);
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
            if (currentNode.m_vPosition != transform.position )
            {
                transform.position = position;
            }
            
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

        IEnumerator moveObject()
        {
            for (int i = 0; i < pathList.Count; i++)
            {
                MoveIE = StartCoroutine(Moving(i));
                yield return MoveIE;
            }
        }

        IEnumerator Moving(int currentPosition)
        {
            while (player.transform.position != pathList[currentPosition].m_vPosition)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, pathList[currentPosition].m_vPosition, m_fMovementSpeed * Time.deltaTime);
                //Move(transform.forward);
                Turn(pathList[currentPosition].m_vPosition);
                yield return null;
            }
        }
    }
}
