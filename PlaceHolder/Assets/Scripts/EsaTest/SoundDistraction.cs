using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class SoundDistraction : ObjectBase
    {

        //TODO: Replace with sound source variable.
        [SerializeField, Tooltip("Sound's sound source")]
        private AudioSource m_aoSound;
        [SerializeField, Tooltip("Distraction range")]
        private float m_fRange = 10f;
        [SerializeField, Tooltip("Audio clip")]
        private AudioClip m_acSound;
        [SerializeField, Tooltip("Idle audio clip")]
        private AudioClip m_acIdle;
        [SerializeField, Tooltip("Object has idle audio")]
        private bool m_bHasIdle;

        public bool trigger;

        Guard guard;

        private void Awake()
        {
            m_aoSound = GetComponent<AudioSource>(); 
            m_aoSound.volume = AudioManager.instance.SFXPlayVol;            
            m_aoSound.loop = true;

            if (m_bHasIdle)
                PlayAudio(m_acIdle);
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
            PlayAudio(m_acSound);
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
            m_aoSound.Stop();
            guard.Distract(this, false);
            
        }

        public void PlayAudio(AudioClip clip)
        {
            m_aoSound.volume = AudioManager.instance.SFXPlayVol;
            m_aoSound.clip = clip;
            m_aoSound.Play();
        }
    }
}