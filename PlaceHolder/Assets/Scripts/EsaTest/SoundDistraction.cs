using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class SoundDistraction : MonoBehaviour
    {

        //TODO: Replace with sound source variable.
        [SerializeField, Tooltip("Sound's sound source")]
        private GameObject m_goSound;
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
            //m_goSound.SetActive(true);
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
            m_goSound.SetActive(false);
            guard.Distract(this, false);
            
        }
    }
}