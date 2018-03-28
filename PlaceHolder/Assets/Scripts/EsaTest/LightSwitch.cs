using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class LightSwitch : ObjectBase
    {
        [SerializeField, Tooltip("Connected lights")]
        private List<LightDistraction> m_lLights = new List<LightDistraction>();        

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
                        for (int i = 0; i < m_lLights.Count; i++)
                        {
                            m_lLights[i].Activated();
                        }
                    }
                }
            }
        }

        protected override void OnMouseExit()
        {
            GetMouseController.DefaultCursor();
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