using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Door : ObjectBase
    {
        [SerializeField, Tooltip("Room's exit position in lobby area")]
        private Transform m_v3LobbyPos;
        [SerializeField, Tooltip("Room's entrance position")]
        private Transform m_v3RoomPosition;
        [SerializeField, Tooltip("Connected room's scene name")]
        private string m_sRoomName;

        private string m_sLobbyName = "Lobby";
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
                            // TODO Move player between scenes.
                            TransportPlayer();
                        }                        
                    }
                }
            }             
        }

        /// <summary>
        /// Moves player to next position.
        /// </summary>
        private void TransportPlayer()
        {
            GameObject player = GameManager.instance.player;
            Vector3 movePos;

            if (GameManager.instance.lobbyIsActive)
            {
                 movePos = RoomPos;
                GameManager.instance.lobbyIsActive = false;
            }
            else
            {
                movePos = LobbyPos;
                GameManager.instance.lastPosition = LobbyPos;
                GameManager.instance.lobbyIsActive = true;
            }

            // TODO Fade in and fade out effect.
            player.transform.position = movePos;
        }

        protected override void OnMouseExit()
        {
            GetMouseController.DefaultCursor();
        }
    }
}