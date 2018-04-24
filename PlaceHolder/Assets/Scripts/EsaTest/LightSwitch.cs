using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class LightSwitch : ObjectBase
    {
        [SerializeField, Tooltip("Connected lights")]
        private LightDistraction m_lLights;
        [SerializeField]
        private AudioSource m_aoSource;
        [SerializeField]
        private AudioClip m_acUseSFX;

        private void Awake()
        {
            if (m_aoSource == null)
                m_aoSource = GetComponent<AudioSource>();

            m_aoSource.volume = AudioManager.instance.SFXPlayVol;
        }

        public void ResetLights()
        {
            m_lLights.ResetLight();
        }

        private void PlayEffect()
        {
            m_aoSource.volume = AudioManager.instance.SFXPlayVol;            
            m_aoSource.PlayOneShot(m_acUseSFX);
        }

        protected override void Activated()
        {
            if (IsActive)
            {                
                if (IsInteractable)
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Switch actvated");
                        PlayEffect();                        
                        if (!m_lLights.IsActive)
                            m_lLights.Activated();
                        else
                            m_lLights.ResetLight();                        
                    }
                }
                else
                    GetMouseController.InspectCursor();
            }
        }
    }
}