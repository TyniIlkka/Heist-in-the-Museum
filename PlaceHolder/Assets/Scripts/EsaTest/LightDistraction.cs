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

        private void Update()
        {
            if (trigger)
            {
                Activated();
            }
        }

        public void Activated()
        {            
            m_goLight.SetActive(true);
            Collider[] objects = Physics.OverlapSphere(transform.position, m_fRange);

            if (objects.Length > 0)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i].GetComponent<Guard>() != null)
                    {
                        objects[i].GetComponent<Guard>().Distract(true, this); 
                    }
                }
            }
        }

        public void ResetLight()
        {
            m_goLight.SetActive(false);
        }
    }
}