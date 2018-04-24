using UnityEngine;

namespace ProjectThief
{
    public class InterActableObject : ObjectBase
    {
        [SerializeField, Tooltip("Vitrines key")]
        private Item m_itKeyItem;        
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;        
        [SerializeField, Tooltip("Lock")]
        private GameObject m_goLock;
        [SerializeField, Tooltip("position in key list")]
        private int m_iPos;
        [SerializeField, Tooltip("Item")]
        private Item m_itKey;
        [SerializeField, Tooltip("Vitrine opening sound")]
        private AudioClip m_acOpen;
        [SerializeField, Tooltip("Unlocking sound")]
        private AudioClip m_acUnlock;
        [SerializeField]
        private AudioSource m_aoSource;
        [SerializeField, Tooltip("Vitrines position in gm list")]
        private int m_iListPos;
         
        private Animator m_aAnimator;        
        private bool m_bUsed;

        private void Awake()
        {
            if (m_aoSource == null)
                m_aoSource = GetComponent<AudioSource>();

            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();

            m_aAnimator = GetComponent<Animator>();
            m_aoSource.volume = AudioManager.instance.SFXPlayVol;

            if (GameManager.instance.openedVitrines[m_iListPos])
            {
                m_bUsed = true;
                m_aAnimator.SetBool("Open", true);                
                m_goLock.SetActive(false);
                m_itKey.gameObject.SetActive(false);
            }
        }
        
        private void PlayAudio(AudioClip clip)
        {
            m_aoSource.clip = clip;
            m_aoSource.volume = AudioManager.instance.SFXPlayVol;
            m_aoSource.Play();
        }

        protected override void Activated()
        {
            if (!GameManager.instance.openedVitrines[m_iListPos])
            {
                if (IsActive)
                {                     
                    if (IsInteractable && m_itKeyItem.Collected && !m_bUsed)
                    {
                        GetMouseController.InteractCursor();
                        if (Input.GetMouseButtonDown(0))
                        {
                            m_bUsed = true;
                            m_iInventory.RemoveItem(m_itKeyItem);
                            PlayAudio(m_acUnlock);                                
                            m_goLock.SetActive(false);
                            PlayAudio(m_acOpen);
                            m_aAnimator.SetBool("Open", true);                                                              
                        }
                    }
                    else
                        GetMouseController.InspectCursor();
                }
            }
            else            
                GetMouseController.DefaultCursor();            
        }

        public void TakeItem()
        {
            m_iInventory.AddItem(m_itKey);
            m_itKey.gameObject.SetActive(false);
            GameManager.instance.keyItems[m_iPos].Collected = true;
            GameManager.instance.openedVitrines[m_iListPos] = true;
        }
    }
}