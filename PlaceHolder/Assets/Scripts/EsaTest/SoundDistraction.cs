﻿using System.Collections;
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

        /// <summary>
        /// For testing purposes.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (trigger)
            {
                DistractionActive();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_fRange);
        }
        
        public void DistractionActive()
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
                        if (guard.Moving)
                        {
                            guard.Distract(this, true);
                        }
                        
                    }
                }
            }
        }

        public void ResetSound()
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