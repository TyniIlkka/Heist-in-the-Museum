﻿using System.Collections;
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

        private void Awake()
        {
            m_aLeverAnim = GetComponentInChildren<Animator>();            
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
            if (IsActive)
            {
                GetMouseController.InspectCursor();
                if (!m_bBroken)
                {
                    if (IsInteractable)
                    {
                        GetMouseController.InteractCursor();
                        if (Input.GetMouseButton(0))
                        {
                            m_aLeverAnim.SetBool("Activated", true);
                            m_aObstacleAnim.SetBool("Open", true);
                            m_dDoor.Blocked = false;
                        }                        
                    }
                }
                else
                {
                    if (IsInteractable)
                    {                        
                        if (m_itNeededItem.Collected)
                        {
                            GetMouseController.InteractCursor();
                            if (Input.GetMouseButton(0))
                            {
                                m_bBroken = false;
                            }                            
                        }                        
                    }
                }
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
            GetMouseController.DefaultCursor();            
        }
    }    
}