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
        
        


        [SerializeField, Tooltip("if True, guard is moving, otherwise static")]
        private bool m_bMoving;

        [SerializeField]
        private float m_fMovementSpeed;
        [SerializeField]
        private float m_fTurnSpeed;

        #region Detecting variables.


        [SerializeField, Tooltip("Which Direction Guard is looking if not patrolling: ")]
        private MyDirections m_eDirection;
        private Vector3 m_vDirection;

        [SerializeField]
        private LayerMask m_lmDetectHitMask;
        [SerializeField, Range(0, 100), Tooltip("How far guard detect on facing direction.")]
        private float m_fMaxDetectionRange;
        [SerializeField, Range(0, 100), Tooltip("How close guard detect to every direction.")]
        private float m_fMinDetectionRange;

        [SerializeField, Range(0, 90), Tooltip("FieldOfView of guard.")]
        private float m_fFieldOfView;

        [SerializeField, Tooltip("Position for static guard.")]
        private Vector3 m_vPosition;
        #endregion

        public void Start()
        {
            
            
        }

        private void Update()
        {
            if (!m_bMoving)
            {
                switch (m_eDirection)
                {
                    case MyDirections.North:
                        transform.forward = new Vector3(0f, 0f, 1f);
                        m_vDirection = new Vector3(0f, 0f, 1f);
                        break;
                    case MyDirections.East:
                        transform.forward = new Vector3(1f, 0f, 0f);
                        m_vDirection = new Vector3(1f, 0f, 0f);
                        break;
                    case MyDirections.South:
                        transform.forward = new Vector3(0f, 0f, -1f);
                        m_vDirection = new Vector3(0f, 0f, -1f);
                        break;
                    case MyDirections.West:
                        transform.forward = new Vector3(-1f, 0f, 0f);
                        m_vDirection = new Vector3(-1f, 0f, 0f);
                        break;
                }
            }
        }

        private void FixedUpdate()
        {
            CanSeePlayer();
            if (CanSeePlayer())
            {
                Debug.Log("GameLost");
                GameManager.instance.levelController.PlayerFound();
            }
            Debug.DrawLine(transform.forward, m_vDirection * m_fMaxDetectionRange);

        }

        public bool CanSeePlayer()
        {
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position - transform.position;
            if (Physics.Raycast(transform.position, rayDirection, out hit, m_fMaxDetectionRange))
            {
                if ((hit.collider.gameObject.GetComponent<Player>() != null) && (hit.distance <= m_fMinDetectionRange))
                {
                    Debug.Log("Player too close guard");
                    return true;
                }
            }

            if ((Vector3.Angle(rayDirection, transform.forward)) <= m_fFieldOfView * 0.5f)
            {
                if (Physics.Raycast(transform.position, rayDirection, out hit, m_fMaxDetectionRange))
                {
                    if (hit.collider.gameObject.GetComponent<Player>() != null)
                    {
                        
                        Debug.DrawLine(transform.position, hit.point, Color.red);
                        Debug.Log(hit);
                        return true;
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.green);
                        Debug.Log(hit);
                    }
                    

                }
            }

            

                return false;
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