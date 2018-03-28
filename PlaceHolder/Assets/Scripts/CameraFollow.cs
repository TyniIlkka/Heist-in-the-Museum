using UnityEngine;

namespace ProjectThief
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField, Tooltip("Cameras vertical angle")]
        private float m_fVerticalAngle = 45;
        [SerializeField, Tooltip("Cameras horizontal angle")]
        private float m_fHorizontalAngle;
        [SerializeField, Tooltip("Cameras movement speed")]
        private float m_fMoveSpeed = 90;
        [SerializeField, Tooltip("Distance from player")]
        private float m_fDistance = 10;        
        [SerializeField, Tooltip("Player transform")]
        private Transform m_tPlayerTransform; 

        public float Distance { get { return m_fDistance; } set { m_fDistance = value; } }

        /// <summary>
        /// Awake method for setting initial position & rotation.
        /// </summary>
        private void Awake()
        {
            if (m_tPlayerTransform == null)
                m_tPlayerTransform = GameManager.instance.player.transform;

            transform.position = m_tPlayerTransform.position;                      
            transform.rotation = Quaternion.Euler(m_fVerticalAngle, m_fHorizontalAngle, 0);
            transform.position -= transform.forward * m_fDistance;                  
        }

        /// <summary>
        /// Lateupdate where cameras position & rotation is set.
        /// </summary>
        private void LateUpdate()
        {
            Vector3 oldPos = transform.position;
            Vector3 newPos = m_tPlayerTransform.position;
            newPos -= transform.forward * m_fDistance;

            transform.position = Vector3.Slerp(oldPos, newPos, m_fMoveSpeed * Time.deltaTime);

            // Testing
            transform.rotation = Quaternion.Euler(m_fVerticalAngle, m_fHorizontalAngle, 0);
        }         
    }
}