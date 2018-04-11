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

        private bool m_bIsBlocked;
        private bool m_bOpened;

        public Transform SpawnPoint { get { return m_tSpawnPoint; } }
        public bool Open { set { m_bIsOpen = value; } }
        public bool Blocked { set { m_bIsBlocked = value; } }  

        private void Awake()
        {
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
            m_aoSource.volume = AudioManager.instance.SFXPlayVol;
        }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        protected override void OnMouseOver()
        {            
            if (IsActive)
            {
                GetMouseController.InspectCursor();

                if (!m_bIsBlocked)
                { 
                    if (IsInteractable)
                    {
                        GetMouseController.EnterCursor();
                        if (!m_bIsOpen)
                        {
                            if (Input.GetMouseButtonDown(0) && !m_bOpened)
                            {
                                DoorOpenSound();
                                m_bOpened = true;                                
                            }
                        }
                        else if (GameManager.instance.levelController.Cleared)
                        {
                            if (Input.GetMouseButtonDown(0) && !m_bOpened)
                            {
                                DoorOpenSound();
                                m_bOpened = true;                                
                            }
                        }
                    }
                }
            }             
        }

        private void Update()
        {
            if (m_bOpened)
            {
                if (!m_aoSource.isPlaying)
                {
                    GameManager.instance.previousState = GameStateController.CurrentState;
                    GameStateController.PerformTransition(_nextState);
                }
            }
        }

        protected override void OnMouseExit()
        {
            GetMouseController.DefaultCursor();
        }

        public void ObstacleSound()
        {            
            m_aoSource.PlayOneShot(m_acUnlock);
        }

        private void DoorOpenSound()
        {
            m_aoSource.PlayOneShot(m_acOpen);
        }
    }
}