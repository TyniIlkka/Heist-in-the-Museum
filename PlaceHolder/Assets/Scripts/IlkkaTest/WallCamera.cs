using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class WallCamera : MonoBehaviour
    {

        // Boolean value for testing.
        public bool scriptIsActive = true;

        [SerializeField]
        private Camera m_cWallCamera;
        [SerializeField]
        private Vector3 m_vMaxRotation;
        [SerializeField]
        private Vector3 m_vMinRotation;
        [SerializeField]
        private float m_fSpeed = 5f;

        public Transform target;

        private Vector3 rotation;

        private bool m_bBlocked;
        private Vector3 m_vStartPosition;
        private Vector3 m_vEndPosition;



        private void Awake()
        {
            if (scriptIsActive)
            {
                
                m_cWallCamera = GetComponent<Camera>();
                //m_goMainCamera.transform.position = this.transform.position;
            }

            //rotation = m_cWallCamera;
            
        }

        // Update is called once per frame
        void Update()
        {
            if (scriptIsActive)
            {
                if (!m_bBlocked)
                {
                    //LookAtPlayer();
                    WallCameraLookAtPlayer();
                }  
            }
            if (m_cWallCamera != null)
            {
                if (m_cWallCamera.transform.rotation.x < m_vMaxRotation.x || m_cWallCamera.transform.rotation.x > m_vMinRotation.x)
                {
                    if (m_cWallCamera.transform.rotation.y < m_vMaxRotation.y || m_cWallCamera.transform.rotation.y > m_vMinRotation.y)
                    {
                        m_bBlocked = false;
                    }
                    else
                    {
                        m_bBlocked = true;
                    }
                }
                else
                {
                    m_bBlocked = true;
                }
            }
        }

        /// <summary>
        /// Camera looks at players direction.
        /// </summary>
        private void LookAtPlayer()
        {
            Transform parent = transform.parent;

            Vector3 direction = parent.position - m_cWallCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            m_cWallCamera.transform.rotation = Quaternion.Lerp(m_cWallCamera.transform.rotation,
               targetRotation, m_fSpeed * Time.deltaTime);
        }

        private void WallCameraLookAtPlayer()
        {
            
            transform.LookAt(target);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("blocked by: " + other.name);
            m_bBlocked = true;
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("unblocked");
            m_bBlocked = false;
        }
    }
}
