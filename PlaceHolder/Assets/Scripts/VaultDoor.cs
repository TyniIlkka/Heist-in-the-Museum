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
        [SerializeField, Tooltip("Inspect text")]
        private string _inspectText = @"""The treasure is stored behind this door.""";
        [SerializeField, Tooltip("Alpha highlight object")]
        private List<Renderer> _aHighlights;
        [SerializeField, Tooltip("Highlight sides")]
        private List<GameObject> _highlights;
        [SerializeField, Tooltip("Fade effect duration")]
        private float _duration;
        [SerializeField, Tooltip("Delay between effects")]
        private float _delay = 3f;
        [SerializeField, Tooltip("Fade amount"), Range(0, 1)]
        private float _fade = 0.5f;
        [SerializeField, Tooltip("Vault door open rotation")]
        private Vector3 _openRotation = new Vector3(-90, 0, -90);

        [SerializeField]
        private DynamicMapUpdate _pathUpdater;

        private Animator m_aAnimator;
        private bool m_bIsLocked;
        private Inventory _inventory;
        private Vector3 lastPosition;
        Bounds lastBounds;
        private bool _highlight;
        private bool _fadeStart;
        private bool _fadeIn;
        private bool _fadeRunning;
        private float _startTime;
        private float _r, _g, _b;
        private float _time;

        private void Awake()
        {
            m_aAnimator = GetComponent<Animator>();
            _inventory = FindObjectOfType<Inventory>();

            _r = _aHighlights[0].material.color.r;
            _g = _aHighlights[0].material.color.g;
            _b = _aHighlights[0].material.color.b;

            _time = _delay;            

            if (GameManager.instance.vaultDoorOpen)
            {
                _wall.SetActive(false);
                m_aAnimator.enabled = false;
                transform.localEulerAngles = _openRotation;
            }

            Debug.Log("door: " + transform.gameObject + " rotation: " + transform.localEulerAngles);
        }

        private void Start()
        {
            lastPosition = transform.position;
            lastBounds = GetComponent<Renderer>().bounds;
        }

        protected override void Update()
        {
            base.Update();

            if (_highlight)
            {                
                SideHighlight();
                AlphaHighlight();
            }
        }

        private void SideHighlight()
        {
            if (!_highlights[0].activeInHierarchy)
            {
                for (int i = 0; i < _highlights.Count; i++)
                {
                    _highlights[i].SetActive(true);
                }
            }
        }

        private void AlphaHighlight()
        {
            if (_fadeStart)
            {
                _fadeStart = false;
                _fadeRunning = true;
                _startTime = Time.time;
            }

            if (_fadeIn && _aHighlights[0].material.color.a == _fade)
            {
                if (_fadeRunning)
                    _fadeRunning = false;

                Timer();
            }

            else if (!_fadeIn && _aHighlights[0].material.color.a == 0)
            {
                _fadeRunning = false;
                _highlight = false;
            }

            if (_fadeRunning)
            {
                if (_fadeIn && _aHighlights[0].material.color.a != _fade)
                    FadeIn();

                else if (!_fadeIn && _aHighlights[0].material.color.a != 0)
                    FadeOut();
            }
        }

        private void Timer()
        {
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                _fadeIn = false;
                _fadeStart = true;
                _time = _delay;
            }
        }

        private void FadeIn()
        {
            float progress = Time.time - _startTime;
            _aHighlights[0].material.color = Color.Lerp(_aHighlights[0].material.color, new Vector4(_r, _g, _b, _fade), progress / _duration);
            _aHighlights[1].material.color = Color.Lerp(_aHighlights[1].material.color, new Vector4(_r, _g, _b, _fade), progress / _duration);
            _aHighlights[2].material.color = Color.Lerp(_aHighlights[2].material.color, new Vector4(_r, _g, _b, _fade), progress / _duration);
            _aHighlights[3].material.color = Color.Lerp(_aHighlights[3].material.color, new Vector4(_r, _g, _b, _fade), progress / _duration);
        }

        private void FadeOut()
        {
            float progress = Time.time - _startTime;
            _aHighlights[0].material.color = Color.Lerp(_aHighlights[0].material.color, new Vector4(_r, _g, _b, 0), progress / _duration);
            _aHighlights[1].material.color = Color.Lerp(_aHighlights[1].material.color, new Vector4(_r, _g, _b, 0), progress / _duration);
            _aHighlights[2].material.color = Color.Lerp(_aHighlights[2].material.color, new Vector4(_r, _g, _b, 0), progress / _duration);
            _aHighlights[3].material.color = Color.Lerp(_aHighlights[3].material.color, new Vector4(_r, _g, _b, 0), progress / _duration);
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
            if (IsActive)
            { 
                if (IsInteractable && CheckKeys())
                {
                    GetMouseController.InteractCursor();
                    if (Input.GetButtonDown("Fire1"))
                    {                            
                        AddKeyPieces();
                        m_aAnimator.SetBool("Open", true);
                        GameManager.instance.vaultDoorOpen = true;
                    }
                } 
                else
                {
                    GetMouseController.InspectCursor();
                    if (Input.GetButtonDown("Fire1"))
                    {
                        InspectText();
                        if (!_highlight)
                        {
                            _highlight = true;
                            _fadeStart = true;
                            _fadeIn = true;
                        }
                    }
                }
            }
        }

        private void InspectText()
        {
            GameManager.instance.infoText = _inspectText;
            GameManager.instance.playMessageSfx = true;

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