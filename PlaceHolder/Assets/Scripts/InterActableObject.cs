using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class InteractableObject : ObjectBase
    {
        [SerializeField, Tooltip("Key Item")]
        private Item m_itKeyItem;        
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;
        [SerializeField, Tooltip("Position close to object")]
        private Vector3 m_v3MoveToPos;         

        private void Awake()
        {
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();            
        }        

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        protected override void OnMouseOver()
        {              
            if (IsActive)
            {
                GetMouseController.InspectCursor();
                if (IsInteractable)
                {
                    if (m_itKeyItem.Collected)
                    {
                        GetMouseController.InteractCursor();
                        if (Input.GetMouseButtonDown(0))
                        {
                            m_iInventory.RemoveItem(m_itKeyItem);
                        }                        
                    }
                }                
            }            
        }

        protected override void OnMouseExit()
        {
            GetMouseController.DefaultCursor();
        }       
    }
}