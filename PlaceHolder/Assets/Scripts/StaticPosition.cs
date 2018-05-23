using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class StaticPosition : MonoBehaviour
    {
        [SerializeField]
        private MyDirections m_eDirections;

        public MyDirections CurrentDir
        {
            get { return m_eDirections; }
            private set { m_eDirections = value; }
        }

    }
}