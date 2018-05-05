using ProjectThief.PathFinding;
using ProjectThief.States;
using UnityEngine;

namespace ProjectThief
{
    public class Door : ObjectBase
    {
        [SerializeField, Tooltip("Spawn Position")]
        private Transform _spawnPoint;
        [SerializeField, Tooltip("Next scene")]
        private GameStateType _nextState;        
        [SerializeField, Tooltip("Is open in previous scene")]
        private bool _isOpen;
        [SerializeField, Tooltip("Door's obstacle")]
        private GameObject _obstacle;
        [SerializeField, Tooltip("Door opening sound")]
        private AudioClip _openingEffect;
        [SerializeField, Tooltip("Bars moving sound")]
        private AudioClip _obstacleEffect;
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField, Tooltip("Delay before door can be used")]
        private float _useDelay = 2f;
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPoint;
        [SerializeField, Tooltip("Inspect info")]
        private string _inspectText = "The door is blocked by bars. I wonder if there is a way to get through.";

        private bool _isBlocked;
        private bool _opened;
        private bool _canBeUsed;        
        private float _timePassed;
        private LevelController _levelController;

        public Transform SpawnPoint { get { return _spawnPoint; } }
        public bool Open { set { _isOpen = value; } }
        public bool Blocked { set { _isBlocked = value; } }  
        public Vector3 MoveToPos { get { return _moveToPoint.position; } }

        private void Awake()
        {
            _canBeUsed = false;
            _timePassed = 0;

            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            if (_isOpen)
            {
                _isBlocked = false;                
                _obstacle.SetActive(false);
            }
            else
            {               
                _isBlocked = true;
            }

            _opened = false;
            _audioSource.volume = PlayVolume;
            _levelController = GameManager.instance.levelController;
        }

        protected override void Update()
        {
            base.Update();

            _audioSource.volume = PlayVolume;

            if (!_canBeUsed)
                Timer();

            if (_opened)
            {
                if (!_audioSource.isPlaying)
                { 
                    GameManager.instance.previousState = GameStateController.CurrentState;
                    GameStateController.PerformTransition(_nextState);
                }
            }
        }

        private void Timer()
        {
            _timePassed += Time.deltaTime;
            if (_timePassed >= _useDelay)
                _canBeUsed = true;
        }

        public void ObstacleSound()
        {            
            _audioSource.PlayOneShot(_obstacleEffect);
        }

        private void DoorOpenSound()
        {
            _audioSource.PlayOneShot(_openingEffect);
        }

        protected override void Activated()
        {
            if (IsActive)
            {
                if (!_isBlocked && _levelController.Cleared)
                {
                    if (IsInteractable)
                    {
                        GetMouseController.EnterCursor();
                        if (_canBeUsed)
                        {
                            if (!_isOpen)
                            {
                                if (Input.GetButtonDown("Fire1") && !_opened)
                                {

                                    DoorOpenSound();                                    
                                    UpdateBooleans();
                                }
                            }
                            else if (GameManager.instance.levelController.Cleared)
                            {
                                if (Input.GetButtonDown("Fire1") && !_opened)
                                {
                                    DoorOpenSound();
                                    UpdateBooleans();
                                }
                            }
                        }
                    }
                    else
                    {
                        GetMouseController.InspectCursor();

                        if (Input.GetButtonDown("Fire1") && GameManager.instance.mouseMovemet)
                        {
                            GameManager.instance.player.GetComponent<GridPlayer>().FindPath(MoveToPos);
                        }                        
                    }
                }
                else
                {
                    GetMouseController.InspectCursor();
                    if (Input.GetButtonDown("Fire1"))
                    {
                        InspectText();
                    }
                }
            }
        }

        private void UpdateBooleans()
        {
            _opened = true;
            GameManager.instance.canMove = false;
            GameManager.instance.inTransit = true;
            GameManager.instance.fadeIn = true;
            GameManager.instance.fadeInStart = true;
            GameManager.instance.levelController.Inventory.SaveInventory();
        }

        private void InspectText()
        {
            GameManager.instance.infoText = _inspectText;            

            if (!GameManager.instance.infoFadeIn)
            {
                GameManager.instance.infoFadeIn = true;
                GameManager.instance.infoFadeInStart = true;
                Debug.Log("open info");
            }
            else
            {
                GameManager.instance.resetInfoTimer = true;
            }
        }
    }
}