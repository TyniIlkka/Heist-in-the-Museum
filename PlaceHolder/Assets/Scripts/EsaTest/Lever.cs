using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Lever : ObjectBase
    {
        [SerializeField, Tooltip("Obstacle Animator")]
        private Animator m_aObstacleAnim;
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;
        [SerializeField, Tooltip("Door")]
        private Door m_dDoor;
        [SerializeField, Tooltip("Marks if lever is broken")]
        private bool m_bBroken;
        [SerializeField, Tooltip("Levers handle")]
        private GameObject m_goHandle;
        [SerializeField, Tooltip("Needed item")]
        private Item m_itNeededItem;
        [SerializeField, Tooltip("Position in GM's bool list")]
        private int m_iPos;        

        private Animator m_aLeverAnim;        

        private void Awake()
        {
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();

            m_aLeverAnim = GetComponentInChildren<Animator>();
            m_itNeededItem = GameManager.instance.refItems[m_itNeededItem.RefPos];

            Init();
        }        

        private void Init()
        {           
            if (GameManager.instance.usedlevers[m_iPos])
            {
                m_goHandle.SetActive(true);
                m_aLeverAnim.SetBool("Activated", true);                
                m_dDoor.Open = true;
                m_dDoor.Blocked = false;
            } 
            else
            {
                m_goHandle.SetActive(false);
                m_bBroken = true;
            }
        }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        protected override void OnMouseOver()
        {            
            // TODO Change to raycast ScreenpointTo ray.

            if (IsActive)
            {
                GetMouseController.InspectCursor();
                
                if (IsInteractable)
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetMouseButtonDown(0))
                    {
                        m_iInventory.RemoveItem(m_itNeededItem);
                        m_goHandle.SetActive(true);
                        m_aLeverAnim.SetBool("Activated", true);
                        m_aObstacleAnim.SetBool("Open", true);
                        m_dDoor.ObstacleSound();
                        m_dDoor.Open = false;
                        m_dDoor.Blocked = false;
                        GameManager.instance.usedlevers[m_iPos] = true;
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