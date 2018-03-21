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
        
        private bool m_bIsBlocked;                

        public Transform SpawnPoint { get { return m_tSpawnPoint; } }
        public bool Blocked { set { m_bIsBlocked = value; } }

        /// <summary>
        /// Door scripts initialization.
        /// </summary>
        private void Awake()
        {
            m_bIsBlocked = true;            
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