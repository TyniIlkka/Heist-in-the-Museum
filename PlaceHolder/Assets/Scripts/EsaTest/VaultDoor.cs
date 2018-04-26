using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class VaultDoor : ObjectBase
    {        
        [SerializeField, Tooltip("Key Pieces")]
        private List<GameObject> m_lPieces;
        [SerializeField]
        private AudioClip m_acSFXEffect;
        [SerializeField, Tooltip("Vault doors wall")]
        private GameObject _wall;

        private Animator m_aAnimator;
        private bool m_bIsLocked;

        private void Awake()
        {
            m_aAnimator = GetComponent<Animator>();
        }

        private bool CheckKeys()
        {
            bool result = true;
            List<Item> check = GameManager.instance.keyItems;

            for (int i = 0; i < check.Count; i++)
            {
                if (!check[i].Collected)                
                    result = false;                
            }

            return result;
        }        

        private void AddKeyPieces()
        {
            foreach (GameObject obj in m_lPieces)
            {
                obj.SetActive(true);
            }
        }

        protected override void Activated()
        {
            if (IsActive)
            { 
                if (IsInteractable && CheckKeys())
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetButtonDown("Fire1"))
                    {                            
                        AddKeyPieces();
                        m_aAnimator.SetBool("Open", true);   
                        // TODO Hide wall
                    }
                }                
            }
            else
                GetMouseController.InspectCursor();
        }
    }
}