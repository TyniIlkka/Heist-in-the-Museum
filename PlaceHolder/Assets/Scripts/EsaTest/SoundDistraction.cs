using ProjectThief.PathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class SoundDistraction : ObjectBase
    {        
        [SerializeField, Tooltip("Sound's sound source")]
        private AudioSource _source;
        [SerializeField, Tooltip("Distraction range")]
        private float _range = 15f;
        [SerializeField, Tooltip("Audio clip")]
        private AudioClip _soundClip;
        [SerializeField, Tooltip("Idle audio clip")]
        private AudioClip _idle;
        [SerializeField, Tooltip("Object has idle audio")]
        private bool _hasIdle;
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPoint;

        private float m_fDistractTime;
        private float m_fTime;
        private bool m_bActive;

        public Vector3 MoveToPos { get { return _moveToPoint.position; } }        

        Collider[] objects;        

        Guard guard;

        private void Awake()
        {
            _source = GetComponent<AudioSource>(); 
            _source.volume = PlayVolume;
            _source.loop = true;
            m_fDistractTime = _soundClip.length;

            if (_hasIdle)
                PlayAudio(_idle, true);
        }
        
        protected override void Update()
        {
            base.Update();

            _source.volume = PlayVolume;

            if (m_bActive)
                Timer();
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
            Gizmos.DrawWireSphere(transform.position, _range);
        }
        
        public void DistractionActive()
        {
            PlayAudio(_soundClip, false);
            m_bActive = true;
            objects = Physics.OverlapSphere(transform.position, _range);

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
            Debug.Log("Distraction active");
        }

        public void DistractionInactive()
        {
            foreach (Collider item in objects)
            {
                guard = item.GetComponent<Guard>();
                if (guard != null)
                {
                    guard.Distract(this, false);
                    m_bActive = false;
                }
            }
            Debug.Log("Distraction inactive");
        }

        public void PlayAudio(AudioClip clip, bool isIdle)
        {
            _source.volume = AudioManager.instance.SFXPlayVol;
            if (!isIdle)
            {                
                _source.PlayOneShot(clip);
            }
            else
            {
                _source.clip = clip;
                _source.Play();
            }
        }

        protected override void Activated()
        {
            if (IsActive)
            {
                if (IsInteractable)
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetButtonDown("Fire1"))
                    {
                        DistractionActive();
                    }
                }
                else
                {
                    GetMouseController.InspectCursor();                    

                    if (Input.GetButtonDown("Fire1"))
                    {
                        GameManager.instance.player.GetComponent<GridPlayer>().FindPath(MoveToPos);
                    }
                }
            }
        }
    }
}