using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class LightSwitch : ObjectBase
    {
        [SerializeField, Tooltip("Connected lights")]
        private List<LightDistraction> m_lLights = new List<LightDistraction>();

        private MouseController m_mcMouseController;

        private void Awake()
        {            
            m_mcMouseController = GameManager.instance.mouseController;
        }

        protected override void OnMouseExit()
        {
            if (IsActive)
            {
                m_mcMouseController.InspectCursor();
                if (IsInteractable)
                {
                    m_mcMouseController.InteractCursor();
                    if (Input.GetMouseButton(0))
                    {
                        for (int i = 0; i < m_lLights.Count; i++)
                        {
                            m_lLights[i].Activated();
                        }
                    }
                }
            }
        }

        protected override void OnMouseOver()
        {
            m_mcMouseController.DefaultCursor();
        }

        public void ResetLights()
        {
            for (int i = 0; i < m_lLights.Count; i++)
            {
                m_lLights[i].ResetLight();
            }
        }
    }
}