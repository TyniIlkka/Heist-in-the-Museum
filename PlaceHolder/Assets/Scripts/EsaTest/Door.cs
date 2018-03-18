using ProjectThief.States;
using UnityEngine;

namespace ProjectThief
{
    public class Door : ObjectBase
    {
        [SerializeField, Tooltip("Room's exit position in lobby area")]
        private Transform m_v3LobbyPos;
        [SerializeField, Tooltip("Room's entrance position")]
        private Transform m_v3RoomPosition;
        [SerializeField, Tooltip("Next scene")]
        private GameStateType _nextState;
        
        private bool m_bIsBlocked;                

        public Vector3 LobbyPos { get { return m_v3LobbyPos.position; } }
        public Vector3 RoomPos { get { return m_v3RoomPosition.position; } }
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