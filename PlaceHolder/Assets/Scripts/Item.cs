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
        [SerializeField, Tooltip("Items position in GM's bool list")]
        private int m_iPosition;
        [SerializeField]
        private int m_iRefPos;

        private bool m_bCollected;
        private int m_iSlotPosition;        

        public Texture ItemImage { get { return m_tItemTexture; } }        
        public bool Collected { get { return m_bCollected; } set { m_bCollected = value; } }         
        public int Slot { get { return m_iSlotPosition; } set { m_iSlotPosition = value; } }
        public int Position { get { return m_iPosition; } }   
        public int RefPos { get { return m_iRefPos; } }


        private void Awake()
        {
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();

            if (GameManager.instance.collectedItems[Position])
            {
                Debug.Log("Item already Collected");
                m_bCollected = true;
                gameObject.SetActive(false);
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
                if (IsInteractable) 
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetMouseButtonDown(0))
                    {                        
                        m_bCollected = true;
                        m_iInventory.AddItem(this);
                        gameObject.SetActive(false);
                        GetMouseController.DefaultCursor();
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