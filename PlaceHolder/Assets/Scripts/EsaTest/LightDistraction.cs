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
        Guard guard;

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

                //for (int i = 0; i < objects.Length; i++)
                //{
                //    if (objects[i].GetComponent<Guard>() != null)
                //    {
                //        objects[i].GetComponent<Guard>().Distract(true, this); 
                //    }
                //}
            }
        }

        public void ResetLight()
        {
            m_goLight.SetActive(false);
            guard.Distract(this, false);
        }
    }
}