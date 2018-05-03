using System.Collections.Generic;
using UnityEngine;
using ProjectThief.PathFinding;

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
        [SerializeField, Tooltip("Vault key parts")]
        private List<Item> _keyParts;

        [SerializeField]
        private DynamicMapUpdate _pathUpdater;

        private Animator m_aAnimator;
        private bool m_bIsLocked;
        private Inventory _inventory;
        private Vector3 lastPosition;
        Bounds lastBounds;

        private void Awake()
        {
            m_aAnimator = GetComponent<Animator>();
            _inventory = FindObjectOfType<Inventory>();
        }

        private void Start()
        {
            lastPosition = transform.position;
            lastBounds = GetComponent<Renderer>().bounds;
            UpdateMapOnce();
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

            for (int i = 0; i < _keyParts.Count; i++)
            {
                _inventory.RemoveItem(_keyParts[i]);
            }
        }

        public void HideWall()
        {
            _wall.SetActive(false);
        }

        protected override void Activated()
        {
            Debug.Log("vault");

            if (IsActive)
            { 
                if (IsInteractable && CheckKeys())
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetButtonDown("Fire1"))
                    {                            
                        AddKeyPieces();
                        UpdateMapOnce();
                        m_aAnimator.SetBool("Open", true);
                        
                        
                        Debug.Log("Updated map!");
                    }
                }                
            }
            else
                GetMouseController.InspectCursor();
        }

        private void UpdateMapOnce()
        {
            Bounds bR = GetComponent<Renderer>().bounds;
            Pathfinder.Instance.DynamicRaycastUpdate(lastBounds);
            Pathfinder.Instance.DynamicRaycastUpdate(bR);
            lastPosition = transform.position;
            lastBounds = bR;
        }
    }
}