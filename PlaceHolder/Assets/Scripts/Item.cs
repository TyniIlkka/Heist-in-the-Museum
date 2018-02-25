using UnityEngine;

namespace ProjectThief
{
    public class Item : MonoBehaviour
    {
        [SerializeField, Tooltip("Item's image")]
        private Texture m_tItemTexture;
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;
        [SerializeField, Tooltip("Position close to object")]
        private Vector3 m_v3MoveToPos;

        // Set is active private when it can be activated by the proximity to the player.
        public bool m_bIsActive;
        // Set is interactable privata when it can be activated by the players proximity.
        public bool m_bIsInteractable;
        private bool m_bCollected;
        private int m_iSlotPosition;

        public Texture ItemImage { get { return m_tItemTexture; } }        
        public bool Collected { get { return m_bCollected; } set { m_bCollected = value; } }    
        public bool IsActive { set { m_bIsActive = value; } }
        public bool IsInteractable { set { m_bIsInteractable = value; } }
        public int Slot { get { return m_iSlotPosition; } set { m_iSlotPosition = value; } }

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
                if (Input.GetMouseButtonDown(0)) 
                {
                    // TODO Add Mouse Animation
                    if (m_bIsInteractable)
                    {
                        m_bCollected = true;
                        m_iInventory.AddItem(this);
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        // TODO if player is not in interaction range move player to position close to object.
                    }
                }                
            }
            else
            {
                // TODO Add Mouse Animation ?
            }
        }        
    }
}