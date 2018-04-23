using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class LightSwitch : ObjectBase
    {
        [SerializeField, Tooltip("Connected lights")]
        private List<LightDistraction> m_lLights = new List<LightDistraction>();
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
            for (int i = 0; i < m_lLights.Count; i++)
            {
                m_lLights[i].ResetLight();
            }
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
                        PlayEffect();
                        for (int i = 0; i < m_lLights.Count; i++)
                        {
                            if (!m_lLights[i].IsActive)
                                m_lLights[i].Activated();
                            else
                                m_lLights[i].ResetLight();
                        }
                    }
                }
                else
                    GetMouseController.InspectCursor();
            }
        }
    }
}