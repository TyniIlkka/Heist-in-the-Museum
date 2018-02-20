using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class InterActableObject : MonoBehaviour
    {
        [SerializeField, Tooltip("Key Item")]
        private Item m_itKeyItem;        
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;
        [SerializeField, Tooltip("Position close to object")]
        private Vector3 m_v3MoveToPos;

        // Set is active private when it can be activated by the proximity to the player.
        public bool m_bIsActive;
        // Set is interactable privata when it can be activated by the players proximity.
        public bool m_bIsInteractable;

        private void Awake()
        {
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();
        }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        private void OnMouseOver()
        {              
            if (m_bIsActive)
            {
                // TODO Mouse animations                
                if (m_bIsInteractable)
                {
                    if (m_itKeyItem.Collected)
                    {
                        // TODO Mouse animations  
                        if (Input.GetMouseButtonDown(0))                        
                            m_iInventory.RemoveItem(m_itKeyItem);                        
                    }
                    else
                    {
                        // TODO Mouse animations  
                    }
                }
                else
                {
                    // TODO Move player to position to interact with object.
                }
            }
            else
            {
                // TODO Mouse animations  
            }
        }

    }
}