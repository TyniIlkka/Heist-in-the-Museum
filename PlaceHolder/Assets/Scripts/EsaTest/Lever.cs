using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Lever : ObjectBase
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
        protected override void OnMouseOver()
        {
            Debug.Log("Lever");
            if (IsActive)
            {
                m_mcMouseController.InteractCursor();
                if (!m_bBroken)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Debug.Log("Activated");
                        if (IsInteractable)
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
                        if (IsInteractable)
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
                m_mcMouseController.InspectCursor();
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

        protected override void OnMouseExit()
        {
            m_mcMouseController.DefaultCursor();
            
        }
    }    
}