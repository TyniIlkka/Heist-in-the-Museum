using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Item : MonoBehaviour
    {
        [SerializeField, Tooltip("Item's image")]
        private Texture m_tItemTexture;
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;
        // Set is active as private when it can be activated by the proximity to the player.
        public bool m_bIsActive;        
        private bool m_bCollected;

        public Texture ItemImage { get { return m_tItemTexture; } }        
        public bool Collected { get { return m_bCollected; } set { m_bCollected = value; } }    
        public bool IsActive { set { m_bIsActive = value; } }

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
                Debug.Log("Over object: " + this.gameObject.name);
                // TODO Add Mouse Animation
                if (Input.GetMouseButtonDown(0) && !m_bCollected)
                {
                    m_bCollected = true;
                    m_iInventory.AddItem(this);
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                // TODO Add Mouse Animation ?
            }
        }        
    }
}