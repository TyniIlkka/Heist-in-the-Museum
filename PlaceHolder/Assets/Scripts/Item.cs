using ProjectThief.PathFinding;
using UnityEngine;

namespace ProjectThief
{
    public class Item : ObjectBase
    {
        [SerializeField, Tooltip("Item's icon image")]
        private Texture _itemIcon;
        [SerializeField, Tooltip("Inventory object")]
        private Inventory _inventory;          
        [SerializeField, Tooltip("Items position in GM's reference list")]
        private int _refPos;
        [Header("Highlight")]
        [SerializeField, Tooltip("Meshrenderer")]
        private MeshRenderer _meshRenderer;
        [SerializeField, Tooltip("Has highlight material")]
        private bool _hasHighlight;
        [SerializeField, Tooltip("Default material")]
        private Material _defaultMat;
        [SerializeField, Tooltip("Highlight material")]
        private Material _highlightMat;
        [SerializeField, Tooltip("Highlight material Position")]
        private int _materialListPos;

        private bool _collected;
        private int _slotPosition;

        public Texture ItemImage { get { return _itemIcon; } }        
        public bool Collected { get { return _collected; } set { _collected = value; } }         
        public int Slot { get { return _slotPosition; } set { _slotPosition = value; } }         
        public int RefPos { get { return _refPos; } }

        private void Awake()
        {
            if (_inventory == null)
                _inventory = FindObjectOfType<Inventory>();

            if (GameManager.instance.refItems[RefPos].Collected)
            {
                _collected = true;
                gameObject.SetActive(false);
            }

            if (_meshRenderer == null && _hasHighlight)
                _meshRenderer = GetComponent<MeshRenderer>();
        }

        protected override void Update()
        {
            base.Update();

            if (_hasHighlight)
            {
                Debug.Log("Highlight is active");
                Debug.Log("Object is Active: " + IsActive);
                Debug.Log("Material in use: " + _meshRenderer.materials[_materialListPos]);

                if (IsActive)
                {
                    _meshRenderer.materials[_materialListPos] = _highlightMat;
                    Debug.Log("Highlight material set");
                }
                else
                {
                    _meshRenderer.materials[_materialListPos] = _defaultMat;
                    Debug.Log("Default material set");
                }
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
                        _collected = true;
                        _inventory.AddItem(this);
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