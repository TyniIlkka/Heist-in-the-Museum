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

        private MouseController m_mcMouseController;

        // Change to private when player can interact whit this.
        public bool m_bIsActive;        
        public bool m_bIsInteractable;

        public bool IsActive { set { m_bIsActive = value; } }
        public bool IsInteractable { set { m_bIsInteractable = value; } }

        private void Awake()
        {
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();
            m_mcMouseController = GameManager.instance.mouseController;
        }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        private void OnMouseOver()
        {              
            if (m_bIsActive)
            {
                m_mcMouseController.InteractCursor();
                if (m_itKeyItem.Collected)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (m_bIsInteractable)
                        {
                            m_iInventory.RemoveItem(m_itKeyItem);
                        }
                        else
                        {
                            // TODO Move player to position to interact with object.
                            // And interact with object?
                        }
                    }
                }
                else
                {
                    Debug.Log("You don't have correct item for this.");
                }
            }
            else
            {
                m_mcMouseController.InspectCursor();
            }
        }

        private void OnMouseExit()
        {
            m_mcMouseController.DefaultCursor();
        }

    }
}