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
        private float m_fMoveSpeed = 90;
        [SerializeField, Tooltip("Distance from player")]
        private float m_fDistance = 10;
        [SerializeField, Tooltip("Reset Camera speed")]
        private float m_fResetspeed = 5;
        [SerializeField, Tooltip("Player transform")]
        private Transform m_tPlayerTransform;       

        private float m_fAngle;
        private bool m_bReset;
        private Quaternion m_qOrginalRotation;

        /// <summary>
        /// Awake method for setting initial position & rotation.
        /// </summary>
        private void Awake()
        {
            if (m_tPlayerTransform == null)
                m_tPlayerTransform = GameManager.instance.player.transform;

            transform.position = m_tPlayerTransform.position;
            m_fHorizontalAngle = m_tPlayerTransform.rotation.eulerAngles.y;
            Debug.Log("angle: " + m_fHorizontalAngle);            
            transform.rotation = Quaternion.Euler(m_fVerticalAngle, m_fHorizontalAngle, 0);
            transform.position -= transform.forward * m_fDistance;
            m_fAngle = m_fHorizontalAngle;
            m_qOrginalRotation = transform.rotation;            
        }

        /// <summary>
        /// Lateupdate where cameras position & rotation is set.
        /// </summary>
        private void LateUpdate()
        {
            if (Input.GetMouseButton(1) && !m_bReset)            
                ControlCamera();
            
            else          
                m_bReset = true;
            
            ResetPosition();

            transform.position = m_tPlayerTransform.position;            

            transform.position -= transform.forward * m_fDistance;
        }

        /// <summary>
        /// Rotates camera around player.
        /// </summary>
        private void ControlCamera()
        {
            if (Input.GetAxis("Mouse X") != 0)            
                m_fAngle += (m_fMoveSpeed * Input.GetAxis("Mouse X")) * Time.deltaTime;

            Debug.Log("rotating");
            transform.rotation = Quaternion.Euler(m_fVerticalAngle, m_fAngle, 0);
        }

        /// <summary>
        /// Resets cameras rotation to default.
        /// </summary>
        private void ResetPosition()
        {
            if (m_bReset)
            {                
                transform.rotation = Quaternion.Slerp(transform.rotation, m_qOrginalRotation, m_fResetspeed * Time.deltaTime);
                if (transform.rotation == m_qOrginalRotation)                   
                {
                    m_fAngle = m_fHorizontalAngle;
                    m_bReset = false;
                }
            }
        }
    }
}