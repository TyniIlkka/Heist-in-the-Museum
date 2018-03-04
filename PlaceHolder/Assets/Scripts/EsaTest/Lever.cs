using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Lever : MonoBehaviour
    {
        [SerializeField, Tooltip("Obstacle Animator")]
        private Animator m_aObstacleAnim;
        [SerializeField, Tooltip("Door")]
        private Door m_dDoor;
        [SerializeField, Tooltip("Marks if lever is broken")]
        private bool m_bBroken;
        [SerializeField, Tooltip("Levers handle")]
        private GameObject m_goHandle;
        [SerializeField, Tooltip("Needed item")]
        private Item m_itNeededItem;

        // Set private when done
        public bool m_bInteractable;
        public bool m_bActive;

        private Animator m_aLeverAnim;
        private MouseController m_mcMouseController;

        private void Awake()
        {
            m_aLeverAnim = GetComponentInChildren<Animator>();
            m_mcMouseController = GameManager.instance.mouseController;
        }

        private void Update()
        {
            if (m_bBroken)
            {
                m_goHandle.SetActive(false);
            }
            else
            {
                m_goHandle.SetActive(true);
            }
        }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        private void OnMouseOver()
        {
            Debug.Log("Lever");
            if (m_bActive)
            {
                m_mcMouseController.Interact = true;
                if (!m_bBroken)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Debug.Log("Activated");
                        if (m_bInteractable)
                        {
                            m_aLeverAnim.SetBool("Activated", true);
                            m_aObstacleAnim.SetBool("Open", true);
                            m_dDoor.Blocked = false;
                        }
                        else
                        {
                            // TODO Move player closer.
                        }
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (m_bInteractable)
                        {
                            if (m_itNeededItem.Collected)
                            {
                                m_bBroken = false;
                            }
                            else
                            {
                                Debug.Log("You don't have correct item for this.");
                            }
                        }
                        else
                        {
                            // TODO Move player closer.
                        }
                    }
                }
            }
            else
            {
                m_mcMouseController.Inspect = true;
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

        private void OnMouseExit()
        {
            m_mcMouseController.Interact = false;
            m_mcMouseController.Inspect = false;
        }
    }    
}