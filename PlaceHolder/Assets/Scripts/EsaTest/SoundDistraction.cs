using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class SoundDistraction : ObjectBase
    {

        //TODO: Replace with sound source variable.
        [SerializeField, Tooltip("Sound's sound source")]
        private AudioSource m_goSound;
        [SerializeField, Tooltip("Distraction range")]
        private float m_fRange = 10f;
        [SerializeField, Tooltip("Audio clip")]
        private AudioClip m_acSound;

        public bool trigger;

        Guard guard;

        private void Awake()
        {
            m_goSound = GetComponent<AudioSource>();
            m_goSound.volume = AudioManager.instance.SfxVol;
            m_goSound.clip = m_acSound;
            m_goSound.loop = true;
            //m_goSound.volume = GameManager.instance.sfxVolume;
        }

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

        protected override void OnMouseOver()
        {
            if (IsActive)
            {
                GetMouseController.InspectCursor();
                if (IsInteractable)
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetMouseButton(0))
                    {
                        Activated();
                    }
                }
            }
        }

        protected override void OnMouseExit()
        {
            GetMouseController.DefaultCursor();
        }

        public void Activated()
        {
            m_goSound.Play();;
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
            m_goSound.Stop();
            guard.Distract(this, false);
            
        }
    }
}