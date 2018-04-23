using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class SoundDistraction : ObjectBase
    {        
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

        private float m_fDistractTime;
        private float m_fTime;
        private bool m_bActive;

        public bool trigger;

        Guard guard;

        private void Awake()
        {
            m_aoSound = GetComponent<AudioSource>(); 
            m_aoSound.volume = AudioManager.instance.SFXPlayVol;            
            m_aoSound.loop = true;
            m_fDistractTime = m_acSound.length;

            if (m_bHasIdle)
                PlayAudio(m_acIdle, true);
        }

        /// <summary>
        /// For testing purposes.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (m_bActive)
                Timer();

            if (trigger)
            {
                DistractionActive();
            }
        }

        private void Timer()
        {
            m_fTime += Time.deltaTime;
            if (m_fTime >= m_fDistractTime)            
                DistractionInactive(); 
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_fRange);
        }
        
        public void DistractionActive()
        {
            PlayAudio(m_acSound, false);
            m_bActive = true;
            Collider[] objects = Physics.OverlapSphere(transform.position, m_fRange);

            if (objects.Length > 0)
            {
                foreach (Collider item in objects)
                {
                    guard = item.GetComponent<Guard>();
                    if (guard != null)
                    {
                        if (guard.Moving)
                        {
                            guard.Distract(this, true);
                        }
                        
                    }
                }
            }
        }

        public void DistractionInactive()
        {            
            guard.Distract(this, false);
            m_bActive = false;
        }

        public void PlayAudio(AudioClip clip, bool isIdle)
        {
            m_aoSound.volume = AudioManager.instance.SFXPlayVol;
            if (!isIdle)
            {                
                m_aoSound.PlayOneShot(clip);
            }
            else
            {
                m_aoSound.clip = clip;
                m_aoSound.Play();
            }
        }

        protected override void Activated()
        {
            if (IsActive)
            {
                GetMouseController.InspectCursor();
                if (IsInteractable)
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetMouseButton(0))
                    {
                        DistractionActive();
                    }
                }
            }
        }
    }
}