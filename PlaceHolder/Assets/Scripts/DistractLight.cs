using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class DistractLight : MonoBehaviour
    {
        [SerializeField]
        private bool m_bLightOn;

        public bool LightOn
        {
            get { return m_bLightOn; }
            set { m_bLightOn = value;}
        }
    }
}
