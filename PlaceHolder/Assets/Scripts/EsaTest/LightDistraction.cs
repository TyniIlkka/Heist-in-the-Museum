using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class LightDistraction : MonoBehaviour
    {
        [SerializeField, Tooltip("Light's light source")]
        private GameObject m_goLight;
        [SerializeField, Tooltip("Distraction range")]
        private float m_fRange = 10f;

        public bool trigger;
        private bool m_bIsActive;
        Guard guard;

        public bool IsActive { get { return m_bIsActive; } set { m_bIsActive = value; } }

        private void Update()
        {
            if (trigger)
            {
                Activated();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_fRange);
        }

        public void Activated()
        {            
            m_goLight.SetActive(true);
            m_bIsActive = true;
            Collider[] objects = Physics.OverlapSphere(transform.position, m_fRange);

            if (objects.Length > 0)
            {
                foreach (Collider item in objects)
                {
                    guard = item.GetComponent<Guard>(); 
                    if (guard != null)
                    {
                        guard.Distract(this, true);
                    }
                }                
            }
        }

        public void ResetLight()
        {
            m_goLight.SetActive(false);
            m_bIsActive = false;
            guard.Distract(this, false);
        }
    }
}