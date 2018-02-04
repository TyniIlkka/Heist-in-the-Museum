using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{

    // Boolean value for testing.
    public bool scriptIsActive = true;

    [SerializeField]
    private GameObject m_goMainCamera;
    [SerializeField]
    private float m_fDuration = 5f;
    [SerializeField]
    private float m_fSpeed = 5f;    

    private bool m_bBlocked;    
    private Vector3 m_vStartPosition;
    private Vector3 m_vEndPosition;    

    private void Awake()
    {
        m_goMainCamera.transform.position = this.transform.position;
    }

    private void Test()
    {
        m_goMainCamera.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        if (scriptIsActive)
        {            
            if (!m_bBlocked)
            {                
                GetMovePosition();
            }
            MoveToPosition();
            LookAtPlayer();
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
           targetRotation, m_fSpeed * Time.deltaTime);

        m_goMainCamera.transform.Translate(direction * 0 * Time.deltaTime);
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
