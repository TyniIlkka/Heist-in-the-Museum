using UnityEngine;

namespace ProjectThief
{
    public class Item : ObjectBase
    {
        [SerializeField, Tooltip("Item's image")]
        private Texture m_tItemTexture;
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;
        [SerializeField, Tooltip("Position close to object")]
        private Vector3 m_v3MoveToPos;        

        private bool m_bCollected;
        private int m_iSlotPosition;
        private MouseController m_mcMouseController;

        public Texture ItemImage { get { return m_tItemTexture; } }        
        public bool Collected { get { return m_bCollected; } set { m_bCollected = value; } }         
        public int Slot { get { return m_iSlotPosition; } set { m_iSlotPosition = value; } }

        private void Awake()
        {
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();
            m_mcMouseController = GameManager.instance.mouseController;
        }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        protected override void OnMouseOver()
        {            
            if (IsActive)
            {
                Debug.Log("Over object: " + this.gameObject.name);
                m_mcMouseController.InteractCursor();
                if (Input.GetMouseButtonDown(0)) 
                {                    
                    if (IsInteractable)
                    {
                        m_bCollected = true;
                        m_iInventory.AddItem(this);
                        gameObject.SetActive(false);
                        m_mcMouseController.DefaultCursor();
                    }
                    else
                    {
                        // TODO if player is not in interaction range move player to position close to object.
                    }
                }                
            }
            else
            {
                m_mcMouseController.InteractCursor();
            }
        }

        protected override void OnMouseExit()
        {
            m_mcMouseController.DefaultCursor();
        }
    }
}