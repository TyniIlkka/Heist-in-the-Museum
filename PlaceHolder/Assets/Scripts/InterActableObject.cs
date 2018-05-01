using ProjectThief.PathFinding;
using UnityEngine;

namespace ProjectThief
{
    public class InterActableObject : ObjectBase
    {
        [SerializeField, Tooltip("Display case's key")]
        private Item _keyItem;        
        [SerializeField, Tooltip("Inventory object")]
        private Inventory _inventory;        
        [SerializeField, Tooltip("Lock")]
        private GameObject _lock;
        [SerializeField, Tooltip("position in key list")]
        private int _keyPos;
        [SerializeField, Tooltip("Vault's key item")]
        private Item _vaultKeyItem;
        [SerializeField, Tooltip("Display case's opening sound")]
        private AudioClip _openingEffect;
        [SerializeField, Tooltip("Unlocking sound")]
        private AudioClip _unlockEffect;
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField, Tooltip("Display case's position in gm list")]
        private int _boolListPos;
        [SerializeField, Tooltip("Display case's door")]
        private GameObject _doorObject;
        [SerializeField, Tooltip("Doors open rotation")]
        private float _openRotation = 140;
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPos;
         
        private Animator _animator;        
        private bool _used;

        public Vector3 MoveToPos { get { return _moveToPos.position; } }

        public bool trigger;

        private void Awake()
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            if (_inventory == null)
                _inventory = FindObjectOfType<Inventory>();

            _animator = GetComponent<Animator>();
            _audioSource.volume = AudioManager.instance.SFXPlayVol;

            if (GameManager.instance.openedVitrines[_boolListPos])
            {
                _used = true;
                _animator.enabled = false;
                _doorObject.transform.localEulerAngles = new Vector3(_doorObject.transform.localEulerAngles.x,
                    _doorObject.transform.localEulerAngles.y, _openRotation);
                Debug.Log("Door rotation " + _doorObject.transform.localEulerAngles);
                _lock.SetActive(false);
                _vaultKeyItem.gameObject.SetActive(false);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (GameManager.instance.openedVitrines[_boolListPos] && trigger)
            {
                _doorObject.transform.localEulerAngles = new Vector3(_doorObject.transform.localEulerAngles.x,
                    _doorObject.transform.localEulerAngles.y, _openRotation);
                Debug.Log("Door rotation " + _doorObject.transform.localEulerAngles);
            }
        }

        private void PlayAudio(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.volume = AudioManager.instance.SFXPlayVol;
            _audioSource.Play();
        }

        protected override void Activated()
        {
            if (!GameManager.instance.openedVitrines[_boolListPos])
            {
                if (IsActive)
                {
                    if (IsInteractable && _keyItem.Collected && !_used)
                    {
                        GetMouseController.InteractCursor();
                        if (Input.GetButtonDown("Fire1"))
                        {
                            _used = true;
                            _inventory.RemoveItem(_keyItem);
                            PlayAudio(_unlockEffect);
                            _lock.SetActive(false);
                            PlayAudio(_openingEffect);
                            _animator.SetBool("Open", true);
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
            _inventory.AddItem(_vaultKeyItem);
            _vaultKeyItem.gameObject.SetActive(false);
            GameManager.instance.keyItems[_keyPos].Collected = true;
            GameManager.instance.openedVitrines[_boolListPos] = true;
        }
    }
}