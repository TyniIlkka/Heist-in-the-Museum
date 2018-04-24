﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class LightDistraction : MonoBehaviour
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
        
        private bool m_bIsActive;
        Guard guard;
        Collider[] objects;

        public bool IsActive { get { return m_bIsActive; } set { m_bIsActive = value; } }        

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_fRange);
        }

        public void Activated()
        {
            Debug.Log("Light Activated");
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
                        Debug.Log("Guard distracted");
                    }
                }                
            }
        }

        public void ResetLight()
        {
            Debug.Log("Light Deactivated");
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
                    Debug.Log("Guard no longer distracted");
                }
            }
        }
    }
}