using ProjectThief.PathFinding;
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
        [SerializeField, Tooltip("Items position in GM's ref list")]
        private int m_iRefPos;
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPoint;

        private bool m_bCollected;
        private int m_iSlotPosition;        

        public Texture ItemImage { get { return m_tItemTexture; } }        
        public bool Collected { get { return m_bCollected; } set { m_bCollected = value; } }         
        public int Slot { get { return m_iSlotPosition; } set { m_iSlotPosition = value; } }         
        public int RefPos { get { return m_iRefPos; } }
        public Vector3 MoveToPos { get { return _moveToPoint.position; } }

        private void Awake()
        {
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();

            if (GameManager.instance.refItems[RefPos].Collected)
            {
                m_bCollected = true;
                gameObject.SetActive(false);
            }
        }

        protected override void Activated()
        {
            if (IsActive)
            {

                if (IsInteractable)
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetButtonDown("Fire1"))
                    {
                        m_bCollected = true;
                        m_iInventory.AddItem(this);
                        gameObject.SetActive(false);
                        GameManager.instance.refItems[RefPos].Collected = true;
                        GetMouseController.DefaultCursor();
                    }
                }
                else                
                    GetMouseController.InspectCursor();                    
            }
        }
    }
}