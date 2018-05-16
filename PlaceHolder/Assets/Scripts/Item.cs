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
        [SerializeField, Tooltip("Info text")]
        private string _inspectText;
        [SerializeField, Tooltip("Highlight particle effect object")]
        private ParticleSystem _particleSystem;

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
                _meshRenderer = GetComponentInChildren<MeshRenderer>();

            if (_particleSystem == null && _hasHighlight)
            {
                _particleSystem = GetComponentInChildren<ParticleSystem>();
                _particleSystem.Stop();
            }            
        }

        public void HighLightItem()
        {
            if (_hasHighlight)
            {
                _meshRenderer.material = _highlightMat;
                _particleSystem.Play();
            }
        }

        public void DeHighLight()
        {
            if (_hasHighlight)
            {
                _meshRenderer.material = _defaultMat;
                _particleSystem.Stop();
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
                        if (_refPos == 13 && !_collected)
                        {
                            GameManager.instance.currentPhase++;
                            InspectText();
                        }

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

        private void InspectText()
        {
            GameManager.instance.infoText = _inspectText;

            if (!GameManager.instance.infoBoxVisible)
            {
                GameManager.instance.infoFadeIn = true;
                GameManager.instance.infoFadeInStart = true;
            }
            else
            {
                GameManager.instance.resetInfoTimer = true;
                GameManager.instance.newText = true;
            }
        }
    }
}