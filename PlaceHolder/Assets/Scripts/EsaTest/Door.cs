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
                                    _opened = true;
                                    GameManager.instance.canMove = false;
                                    Debug.Log("In transit. " + "Can move: " + GameManager.instance.canMove);
                                }
                            }
                            else if (GameManager.instance.levelController.Cleared)
                            {
                                if (Input.GetButtonDown("Fire1") && !_opened)
                                {
                                    DoorOpenSound();
                                    _opened = true;
                                    GameManager.instance.canMove = false;
                                    Debug.Log("In transit. " + "Can move: " + GameManager.instance.canMove);
                                }
                            }
                        }
                    }
                    else
                    {
                        GetMouseController.InspectCursor();

                        if (Input.GetButtonDown("Fire1"))
                        {
                            GameManager.instance.player.GetComponent<GridPlayer>().FindPath(MoveToPos);
                        }                        
                    }
                }
                else
                {
                    GetMouseController.InspectCursor();
                }
            }
        }
    }
}