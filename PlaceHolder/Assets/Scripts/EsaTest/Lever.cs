using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Lever : MonoBehaviour
    {
        [SerializeField, Tooltip("Obstacle Animator")]
        private Animator m_aObstacleAnim;

        // Set private when done
        public bool m_bBroken;
        public bool m_bInteractable;
        public bool m_bActive;

        private Animator m_aLeverAnim;        

        private void Awake()
        {
            m_aLeverAnim = GetComponentInChildren<Animator>();            
        }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        private void OnMouseOver()
        {
            Debug.Log("Lever");
            if (m_bActive)
            {
                if (!m_bBroken)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Debug.Log("Activated");
                        if (m_bInteractable)
                        {
                            m_aLeverAnim.SetBool("Activated", true);
                            m_aObstacleAnim.SetBool("Open", true);
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

            if (Input.GetKey(KeyCode.R))
            {
                ResetLever();
            }
        }

        /// <summary>
        /// Resets lever's and obstacle's animation
        /// </summary>
        public void ResetLever()
        {
            m_aLeverAnim.SetBool("Activated", false);
            m_aObstacleAnim.SetBool("Open", false);
        }
    }    
}