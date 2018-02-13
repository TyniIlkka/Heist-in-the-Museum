using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField, Tooltip("Cameras vertical angle")]
        private float m_fVerticalAngle = 45;
        [SerializeField, Tooltip("Cameras starting horizontal angle")]
        private float m_fHorizontalAngle;
        [SerializeField, Tooltip("Cameras movement speed")]
        private float m_fMoveSpeed = 5;
        [SerializeField, Tooltip("Distance from player")]
        private float m_fDistance = 10;        
        [SerializeField, Tooltip("Player transform")]
        private Transform m_tPlayerTransform;       

        private float m_fAngle;

        /// <summary>
        /// Awake method for setting initial position & rotation.
        /// </summary>
        private void Awake()
        {
            transform.position = m_tPlayerTransform.position;
            transform.rotation = Quaternion.Euler(m_fVerticalAngle, m_fHorizontalAngle, 0);
            transform.position -= transform.forward * m_fDistance;
            m_fAngle = m_fHorizontalAngle;
        }

        /// <summary>
        /// Lateupdate where cameras position & rotation is set.
        /// </summary>
        private void LateUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                ControlCamera();
            }

            if (Input.GetKey(KeyCode.R))
            {
                ResetPosition();
            }           

            transform.position = m_tPlayerTransform.position;
            transform.rotation = Quaternion.Euler(m_fVerticalAngle, m_fAngle, 0);
            transform.position -= transform.forward * m_fDistance;
        }

        /// <summary>
        /// Rotates camera around player.
        /// </summary>
        private void ControlCamera()
        {
            if (Input.GetAxis("Mouse X") != 0)
            {
                m_fAngle += (m_fMoveSpeed * Input.GetAxis("Mouse X")) * Time.deltaTime;
            }            
        }

        /// <summary>
        /// Resets cameras rotation to default.
        /// </summary>
        private void ResetPosition()
        {
            m_fAngle = m_fHorizontalAngle;
        }
    }
}