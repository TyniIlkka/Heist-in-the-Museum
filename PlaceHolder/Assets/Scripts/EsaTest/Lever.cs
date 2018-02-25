using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Lever : MonoBehaviour
    {
        [SerializeField, Tooltip("Obstacle")]
        private GameObject m_goObstacle;

        // Set private when done
        public bool m_bBroken;
        public bool m_bInteractable;
        public bool m_bActive;

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        private void OnMouseOver()
        {
            if (m_bActive)
            {
                if (!m_bBroken)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (m_bInteractable)
                        {
                            // TODO activate obstacles animation and move it aside.
                        }
                        else
                        {
                            // TODO Move player closer.
                        }
                    }
                }
                else
                {
                    // Mouse animations?
                }
            }
            else
            {
                // TODO mouse animations.
            }
        }

        /// <summary>
        /// Resets lever's and obstacle's animation
        /// </summary>
        public void ResetLever()
        {
            // TODO Reset obstacles animation.
        }
    }    
}