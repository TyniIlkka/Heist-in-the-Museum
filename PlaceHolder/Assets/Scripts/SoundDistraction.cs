using ProjectThief.PathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField, Tooltip("Has tutorial effect")]
        private bool _tutorialEffect;
        [SerializeField, Tooltip("Phase where to activate")]
        private int _activePhase;
        [SerializeField, Tooltip("Has time out")]
        private bool _hasTimeout;
        [SerializeField, Tooltip("Time out time")]
        private float _timeout = 3f;

        private float _distractTime;
        private float _time;
        private bool _active;
        private ParticleSystem _particleSystem;
        private GameObject _timerObject;
        private Image _timerImage;

        public float DistractionTime { get { return _distractTime; } }
        public Vector3 MoveToPos { get { return _moveToPoint.position; } }        

        Collider[] objects;        

        Guard guard;

        private void Awake()
        {
            _source = GetComponent<AudioSource>(); 
            _source.volume = PlayVolume;
            _source.loop = true;

            if (_hasTimeout)            
                _distractTime = _timeout;
            else
                _distractTime = _soundClip.length;

            _time = _distractTime;

            _timerObject = GameManager.instance.levelController.DistractTimerImage;
            _timerImage = _timerObject.GetComponent<Image>();

            if (_hasIdle)
                PlayAudio(_idle, true);

            if (_particleSystem == null && _tutorialEffect)
            {
                _particleSystem = GetComponent<ParticleSystem>();

                if (GameManager.instance.currentPhase == _activePhase &&
                    !GameManager.instance.tutorialeffects[1])
                    _particleSystem.Play();
            }
        }
        
        protected override void Update()
        {
            base.Update();

            _source.volume = PlayVolume;

            if (_active)
                Timer();
        }

        private void Timer()
        {
            _time -= Time.deltaTime;
            _timerImage.fillAmount = _time / _distractTime;
            if (_time <= 0)            
                DistractionInactive(); 
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
        
        public void DistractionActive()
        {
            _timerObject.SetActive(true);
            PlayAudio(_soundClip, false);
            _active = true;
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
            _timerObject.SetActive(false);
            foreach (Collider item in objects)
            {
                guard = item.GetComponent<Guard>();
                if (guard != null)
                {
                    guard.Distract(this, false);
                    _active = false;
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
                        if (_tutorialEffect)
                        {
                            _particleSystem.Stop();
                            GameManager.instance.tutorialeffects[1] = true;
                        }
                    }
                }
                else                
                    GetMouseController.InspectCursor();                
            }
        }
    }
}