using ProjectThief.PathFinding;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class LightSwitch : ObjectBase
    {
        [SerializeField, Tooltip("Connected lights")]
        private LightDistraction m_lLight;
        [SerializeField]
        private AudioSource m_aoSource;
        [SerializeField]
        private AudioClip m_acUseSFX;
        [SerializeField, Tooltip("Cooldown time")]
        private float m_fCooldown = 0.3f;
        [SerializeField, Tooltip("Has tutorial effect")]
        private bool _tutorialEffect;
        [SerializeField, Tooltip("Phase where to activate")]
        private int _activePhase;
        [SerializeField, Tooltip("Inspect text")]
        private string _inspectText = @"""I might able to distract guards with lights that are connected to this switch""";

        private float m_fTimePassed;
        private bool m_bCanUse;
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            if (m_aoSource == null)
                m_aoSource = GetComponent<AudioSource>();

            m_aoSource.volume = PlayVolume;
            m_bCanUse = true;

            if (_particleSystem == null && _tutorialEffect)
            {
                _particleSystem = GetComponent<ParticleSystem>();

                if (GameManager.instance.currentPhase == _activePhase &&
                    !GameManager.instance.tutorialeffects[0])
                    _particleSystem.Play();
            }
        }

        protected override void Update()
        {
            base.Update();
            m_aoSource.volume = PlayVolume;

            if (!m_bCanUse)
                Timer();
        }

        private void Timer()
        {            
            m_fTimePassed += Time.deltaTime;

            if (m_fTimePassed >= m_fCooldown)
            {
                m_bCanUse = true;
                m_fTimePassed = 0;               
            }
        }

        private void PlayEffect()
        {           
            m_aoSource.PlayOneShot(m_acUseSFX);
        }

        protected override void Activated()
        {
            if (IsActive)
            {
                if (IsInteractable)
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetButtonDown("Fire1") && m_bCanUse)
                    {
                        m_bCanUse = false;
                        PlayEffect();

                        if (_tutorialEffect && _particleSystem.isPlaying)
                        {
                            _particleSystem.Stop();
                            GameManager.instance.tutorialeffects[0] = true;
                        }

                        if (!m_lLight.LightIsActive)
                            m_lLight.LightActivated();
                        else
                            m_lLight.LightDeactivated();
                    }
                }
                else
                {
                    GetMouseController.InspectCursor();
                    if (Input.GetButtonDown("Fire1"))
                    {
                        InspectText();
                    }
                }
            }            
        }

        private void InspectText()
        {
            GameManager.instance.infoText = _inspectText;
            GameManager.instance.playMessageSfx = true;

            if (!GameManager.instance.infoBoxVisible)
            {
                GameManager.instance.infoFadeIn = true;
                GameManager.instance.infoFadeInStart = true;
            }
            else
            {
                GameManager.instance.resetInfoTimer = true;
                GameManager.instance.newText = true;
            }
        }
    }
}