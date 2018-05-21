using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class LightDistraction : ObjectBase
    {
        [SerializeField, Tooltip("Light's light source")]
        private GameObject m_goLight;
        [SerializeField, Tooltip("Distraction range")]
        private float m_fRange = 10f;
        [SerializeField, Tooltip("Default material")]
        private Material m_mDefaultMat;
        [SerializeField, Tooltip("Lit material")]
        private Material m_mLitMaterial;
        [SerializeField, Tooltip("Light")]
        private GameObject m_goLightObject;
        [SerializeField, Tooltip("Inspect Text")]
        private string _inspectText = @"""That lamp might get the guard's attention.""";

        private bool m_bIsActive;
        Guard guard;
        Collider[] objects;

        public bool LightIsActive { get { return m_bIsActive; } set { m_bIsActive = value; } }        

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_fRange);
        }

        public void LightActivated()
        {            
            m_goLight.SetActive(true);
            m_bIsActive = true;
            objects = Physics.OverlapSphere(transform.position, m_fRange);
            m_goLightObject.GetComponent<MeshRenderer>().material = m_mLitMaterial;

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

        public void LightDeactivated()
        {
            m_goLight.SetActive(false);
            m_bIsActive = false;
            //guard.Distract(this, false);
            m_goLightObject.GetComponent<MeshRenderer>().material = m_mDefaultMat;

            foreach (Collider item in objects)
            {
                guard = item.GetComponent<Guard>();
                if (guard != null)
                {
                    guard.Distract(this, false);
                }
            }
        }

        private void InspectText()
        {
            GameManager.instance.infoText = _inspectText;

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

        protected override void Activated()
        {
            if (IsActive)
            {
                GetMouseController.InspectCursor();
                if (Input.GetButtonDown("Fire1"))
                {
                    InspectText();
                }
            }
        }
    }
}