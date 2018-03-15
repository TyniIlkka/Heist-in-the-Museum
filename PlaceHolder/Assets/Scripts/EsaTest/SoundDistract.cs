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
            m_goSound.SetActive(true);
            Collider[] objects = Physics.OverlapSphere(transform.position, m_fRange);

            if (objects.Length > 0)
            {
                foreach (Collider item in objects)
                {
                    Guard guard = item.GetComponent<Guard>();
                    if (guard != null)
                    {
                        guard.Distract(true, this);
                    }
                }
            }
        }

        public void ResetLight()
        {
            m_goSound.SetActive(false);
        }
    }
}