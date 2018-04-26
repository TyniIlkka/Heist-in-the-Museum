using ProjectThief.PathFinding;
using ProjectThief.States;
using UnityEngine;

namespace ProjectThief
{
    public class Door : ObjectBase
    {
        [SerializeField, Tooltip("Spawn Position")]
        private Transform m_tSpawnPoint;
        [SerializeField, Tooltip("Next scene")]
        private GameStateType _nextState;        
        [SerializeField, Tooltip("Is open in previous scene")]
        private bool m_bIsOpen;
        [SerializeField, Tooltip("Door's obstacle")]
        private GameObject m_goObstacle;
        [SerializeField, Tooltip("Door opening sound")]
        private AudioClip m_acOpen;
        [SerializeField, Tooltip("Bars moving sound")]
        private AudioClip m_acUnlock;
        [SerializeField]
        private AudioSource m_aoSource;
        [SerializeField, Tooltip("Delay before door can be used")]
        private float m_fDelay = 2f;
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPoint;

        private bool m_bIsBlocked;
        private bool m_bOpened;
        private bool m_bCanUse;        
        private float m_fTimer;
        private LevelController _levelController;

        public Transform SpawnPoint { get { return m_tSpawnPoint; } }
        public bool Open { set { m_bIsOpen = value; } }
        public bool Blocked { set { m_bIsBlocked = value; } }  
        public Vector3 MoveToPos { get { return _moveToPoint.position; } }

        private void Awake()
        {
            m_bCanUse = false;
            m_fTimer = 0;

            if (m_aoSource == null)
                m_aoSource = GetComponent<AudioSource>();

            if (m_bIsOpen)
            {
                m_bIsBlocked = false;                
                m_goObstacle.SetActive(false);
            }
            else
            {               
                m_bIsBlocked = true;
            }

            m_bOpened = false;
            m_aoSource.volume = PlayVolume;
            _levelController = GameManager.instance.levelController;
        }

        protected override void Update()
        {
            base.Update();

            m_aoSource.volume = PlayVolume;

            if (!m_bCanUse)
                Timer();

            if (m_bOpened)
            {
                if (!m_aoSource.isPlaying)
                {
                    GameManager.instance.previousState = GameStateController.CurrentState;
                    GameStateController.PerformTransition(_nextState);
                }
            }
        }

        private void Timer()
        {
            m_fTimer += Time.deltaTime;
            if (m_fTimer >= m_fDelay)
                m_bCanUse = true;
        }

        public void ObstacleSound()
        {            
            m_aoSource.PlayOneShot(m_acUnlock);
        }

        private void DoorOpenSound()
        {
            m_aoSource.PlayOneShot(m_acOpen);
        }

        protected override void Activated()
        {
            if (IsActive)
            {
                if (!m_bIsBlocked && _levelController.Cleared)
                {
                    if (IsInteractable)
                    {
                        GetMouseController.EnterCursor();
                        if (m_bCanUse)
                        {
                            if (!m_bIsOpen)
                            {
                                if (Input.GetButtonDown("Fire1") && !m_bOpened)
                                {
                                    DoorOpenSound();
                                    m_bOpened = true;
                                }
                            }
                            else if (GameManager.instance.levelController.Cleared)
                            {
                                if (Input.GetButtonDown("Fire1") && !m_bOpened)
                                {
                                    DoorOpenSound();
                                    m_bOpened = true;
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