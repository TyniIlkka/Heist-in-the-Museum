using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectThief
{
    public class DistractSound : MonoBehaviour
    {
        private bool m_bSoundOn;

        public bool SoundOn
        {
            get { return m_bSoundOn; }
            set { m_bSoundOn = value; }
        }
    }
}
