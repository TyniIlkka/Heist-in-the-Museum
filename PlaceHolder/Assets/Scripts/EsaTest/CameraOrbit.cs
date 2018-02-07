using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class CameraOrbit : MonoBehaviour
    {

        // Boolean value for testing.
        public bool scriptIsActive = true;

        [SerializeField, Tooltip("Main Camera")]
        private GameObject m_goMainCamera;
        [SerializeField, Tooltip("How long does camera take to move to position")]
        private float m_fDuration = 5f;
        [SerializeField, Tooltip("Cameras rotation speed")]
        private float m_fRotationSpeed = 5f;
        [SerializeField, Tooltip("Minimum distance to player")]
        private float m_fMinDistance = 1f;
        [SerializeField, Tooltip("Maximum distance to player")]
        private float m_fMaxDistance = 10f;
        [SerializeField, Tooltip("Cameras move speed")]
        private float m_fMoveSpeed = 1f;
        [SerializeField, Tooltip("Cameras orbit speed")]
        private float m_fOrbitSpeed = 5f;
        [SerializeField, Tooltip("Cameras manual move speed")]
        private float m_fManualSpeed = 10f;

        private float m_fAngle;
        private float m_fX, m_fZ;
        private float m_fCurrentDistance;
        private bool m_bBlocked;
        private bool m_bWallLeft;
        private bool m_bWallRight;
        private Vector3 m_vStartPosition;
        private Vector3 m_vEndPosition;

        private Vector3 m_vDefaultPosition;
        // TODO camera position either left or right shoulder.

        private void Awake()
        {
            m_goMainCamera.transform.position = transform.position;
            m_vDefaultPosition = transform.localPosition;
            m_fCurrentDistance = Vector3.Distance(transform.position, transform.parent.position);
        }        

        // Update is called once per frame
        void Update()
        {
            if (scriptIsActive)
            {                 
                if (Input.GetMouseButton(1))
                {                    
                    ControlCamera();
                }
                if (Input.GetKey(KeyCode.R))
                {
                    ResetCameraPosition();
                }

                if (!m_bBlocked)
                {
                    GetMovePosition();
                }
                MoveToPosition();
                LookAtPlayer();
                AvoidBlocked();                
            }
        }        

        /// <summary>
        /// Moves object closer to player or towards direction 
        /// where object is no longer blocked by an obstacle.
        /// If camera gets stuck it is send to opposite side.
        /// </summary>
        private void AvoidBlocked()
        {
            if (m_bBlocked)
            {                
                if (m_fCurrentDistance > m_fMinDistance)
                {
                    Vector3 position = transform.localPosition;
                    Vector3 movement = new Vector3(0, 0, transform.localPosition.z * Time.deltaTime * m_fMoveSpeed);
                    position -= movement;
                    transform.localPosition = position;                    
                }
                else
                {
                    m_fCurrentDistance = m_fMinDistance;
                    CheckWalls();
                    m_fX = transform.localPosition.x;
                    if (m_bWallLeft)
                    {
                        m_fAngle = m_fAngle + m_fOrbitSpeed * Time.deltaTime;
                        m_fX = m_fCurrentDistance * Mathf.Sin(m_fAngle);
                        m_fZ = m_fCurrentDistance * Mathf.Cos(m_fAngle);
                        transform.localPosition = new Vector3(m_fX, transform.localPosition.y, m_fZ);
                    }
                    else if (m_bWallRight)
                    {
                        m_fAngle = m_fAngle - m_fOrbitSpeed * Time.deltaTime;
                        m_fX = m_fCurrentDistance * Mathf.Sin(m_fAngle);
                        m_fZ = m_fCurrentDistance * Mathf.Cos(m_fAngle);
                        transform.localPosition = new Vector3(m_fX, transform.localPosition.y, m_fZ);
                    }
                    else
                    {
                        Debug.LogError("Camerea is stuck");
                        transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y,
                            -transform.localPosition.z);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if there are walls around object.
        /// </summary>
        private void CheckWalls()
        {
            if (Physics.Raycast(transform.position, Vector3.left, 1f) && !m_bWallRight)
            {
                m_bWallLeft = true;
                m_bWallRight = false;
            }
            else if (Physics.Raycast(transform.position, Vector3.right, 1f) && !m_bWallLeft)
            {
                m_bWallLeft = false;
                m_bWallRight = true;
            }
        }

        /// <summary>
        /// Gets new move to position if that position is not blocked.
        /// </summary>
        private void GetMovePosition()
        {
            m_vStartPosition = m_goMainCamera.transform.position;
            m_vEndPosition = transform.position;
        }

        /// <summary>
        /// Moves camera to position.
        /// </summary>
        private void MoveToPosition()
        {
            m_goMainCamera.transform.position = Vector3.Lerp(m_vStartPosition, m_vEndPosition, m_fDuration);
        }

        /// <summary>
        /// Camera looks at players direction.
        /// </summary>
        private void LookAtPlayer()
        {
            Transform parent = transform.parent;

            Vector3 direction = parent.position - m_goMainCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            m_goMainCamera.transform.rotation = Quaternion.Lerp(m_goMainCamera.transform.rotation,
               targetRotation, m_fRotationSpeed * Time.deltaTime);            
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("blocked");
            m_bBlocked = true;
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("unblocked");
            m_bBlocked = false;
        }

        /// <summary>
        /// Manual control of the camera.
        /// </summary>
        private void ControlCamera()
        {
            //Zoom control
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                Debug.Log("Scroll: " + Input.GetAxis("Mouse ScrollWheel"));
                if (m_fCurrentDistance <= m_fMaxDistance && m_fCurrentDistance >= m_fMinDistance)
                {                    
                    //m_fCurrentDistance = m_fCurrentDistance * (m_fMoveSpeed * Input.GetAxis("Mouse ScrollWheel")) * Time.deltaTime;
                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        m_fCurrentDistance = m_fCurrentDistance - m_fManualSpeed * Time.deltaTime;
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        m_fCurrentDistance = m_fCurrentDistance + m_fManualSpeed * Time.deltaTime;
                    }
                }
                CheckDistance();                
            }
            Debug.Log("Distance: " + m_fCurrentDistance);            

            // Orbit control
            if (Input.GetAxis("Mouse X") != 0)
            {
                m_fAngle = m_fAngle + (m_fOrbitSpeed * Input.GetAxis("Mouse X")) * Time.deltaTime;                
            }

            m_fX = m_fCurrentDistance * Mathf.Sin(m_fAngle);
            m_fZ = m_fCurrentDistance * Mathf.Cos(m_fAngle);
            transform.localPosition = new Vector3(m_fX, transform.localPosition.y, m_fZ);
        }

        /// <summary>
        /// Checks that distance is within parameters.
        /// </summary>
        private void CheckDistance()
        {
            if (m_fCurrentDistance < m_fMinDistance)
            {
                m_fCurrentDistance = m_fMinDistance;
            }
            else if (m_fCurrentDistance > m_fMaxDistance)
            {
                m_fCurrentDistance = m_fMaxDistance;
            }
        }

        /// <summary>
        /// Resets camera position.
        /// </summary>
        public void ResetCameraPosition()
        {
            transform.localPosition = m_vDefaultPosition;
            m_fAngle = 0;
            m_fX = 0;
            m_fZ = 0;
            m_fCurrentDistance = 3;
            AvoidBlocked();
        }
    }
}