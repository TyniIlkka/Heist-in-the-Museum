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

        private bool m_bIsBlocked;

        public Transform SpawnPoint { get { return m_tSpawnPoint; } }
        public bool Open { set { m_bIsOpen = value; } }
        public bool Blocked { set { m_bIsBlocked = value; } }


        private void Awake()
        {
            if (m_bIsOpen)
            {
                m_bIsBlocked = false;
                m_goObstacle.SetActive(false);
            }
            else
            {               
                m_bIsBlocked = true;
            }
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
                        if (Input.GetMouseButton(0))
                        {
                            GameManager.instance.previousState = GameStateController.CurrentState;
                            GameStateController.PerformTransition(_nextState);
                        }                        
                    }
                }
            }             
        }

        protected override void OnMouseExit()
        {
            GetMouseController.DefaultCursor();
        }
    }
}