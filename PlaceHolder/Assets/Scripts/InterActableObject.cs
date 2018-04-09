using UnityEngine;

namespace ProjectThief
{
    public class InterActableObject : ObjectBase
    {
        [SerializeField, Tooltip("Key Item ref list pos")]
        private int m_itKeyItemPos;        
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;        
        [SerializeField, Tooltip("Lock")]
        private GameObject m_goLock;
        [SerializeField, Tooltip("position in key list")]
        private int m_iPos;
        [SerializeField, Tooltip("Item")]
        private Item m_itKey;
         
        private Animator m_aAnimator;

        private void Awake()
        {            
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();

            m_aAnimator = GetComponent<Animator>();
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
                    if (GameManager.instance.refItems[m_itKeyItemPos].Collected)
                    {
                        GetMouseController.InteractCursor();
                        if (Input.GetMouseButtonDown(0))
                        {
                            m_iInventory.RemoveItem(GameManager.instance.refItems[m_itKeyItemPos]);
                            Debug.Log("opened");
                            m_goLock.SetActive(false);
                            m_aAnimator.SetBool("Open", true);
                            m_iInventory.AddItem(m_itKey);
                            GameManager.instance.keyItems[m_iPos].Collected = true;
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