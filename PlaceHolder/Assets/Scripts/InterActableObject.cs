using ProjectThief.PathFinding;
using UnityEngine;

namespace ProjectThief
{
    public class InterActableObject : ObjectBase
    {
        [SerializeField, Tooltip("Display case's key")]
        private Item m_itKeyItem;        
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;        
        [SerializeField, Tooltip("Lock")]
        private GameObject m_goLock;
        [SerializeField, Tooltip("position in key list")]
        private int m_iPos;
        [SerializeField, Tooltip("Vault's key item")]
        private Item m_itKey;
        [SerializeField, Tooltip("Display case's opening sound")]
        private AudioClip m_acOpen;
        [SerializeField, Tooltip("Unlocking sound")]
        private AudioClip m_acUnlock;
        [SerializeField]
        private AudioSource m_aoSource;
        [SerializeField, Tooltip("Display case's position in gm list")]
        private int m_iListPos;
        [SerializeField, Tooltip("Display case's door")]
        private GameObject m_goDoor;
        [SerializeField, Tooltip("Doors open rotation")]
        private float m_fRotationZ = 140;
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPos;
         
        private Animator m_aAnimator;        
        private bool m_bUsed;

        public Vector3 MoveToPos { get { return _moveToPos.position; } }

        public bool trigger;

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
                m_aAnimator.enabled = false;
                m_goDoor.transform.localEulerAngles = new Vector3(m_goDoor.transform.localEulerAngles.x,
                    m_goDoor.transform.localEulerAngles.y, m_fRotationZ);
                Debug.Log("Door rotation " + m_goDoor.transform.localEulerAngles);
                m_goLock.SetActive(false);
                m_itKey.gameObject.SetActive(false);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (GameManager.instance.openedVitrines[m_iListPos] && trigger)
            {
                m_goDoor.transform.localEulerAngles = new Vector3(m_goDoor.transform.localEulerAngles.x,
                    m_goDoor.transform.localEulerAngles.y, m_fRotationZ);
                Debug.Log("Door rotation " + m_goDoor.transform.localEulerAngles);
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
                        if (Input.GetButtonDown("Fire1"))
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
                    {
                        GetMouseController.InspectCursor();

                        GameManager.instance.player.GetComponent<GridPlayer>().FindPath(MoveToPos);
                    }
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