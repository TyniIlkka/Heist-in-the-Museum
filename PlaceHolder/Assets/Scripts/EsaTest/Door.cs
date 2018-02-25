using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Door : MonoBehaviour
    {
        [SerializeField, Tooltip("Room's exit position in lobby area")]
        private Vector3 m_v3LobbyPos;
        [SerializeField, Tooltip("Room's entrance position")]
        private Vector3 m_v3RoomPosition;

        // Change to private when player can interact with objects.
        public bool m_bIsBlocked;
        public bool m_bIsActive;
        public bool m_bIsInteractable;

        public Vector3 LobbyPos { get { return m_v3LobbyPos; } }
        public Vector3 RoomPos { get { return m_v3RoomPosition; } }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        private void OnMouseOver()
        {            
            if (m_bIsActive)
            {
                if (!m_bIsBlocked)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (m_bIsInteractable)
                        {
                            TransportPlayer();
                        }
                        else
                        {
                            // TODO Move player to interactable distance.
                        }
                    }
                }
            } 
            else
            {
                // Mouse animations.
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
                 movePos = m_v3RoomPosition;                
            }
            else
            {
                movePos = m_v3LobbyPos;
                GameManager.instance.lastPosition = m_v3LobbyPos;
            }

            // TODO Fade in and fade out effect.
            player.transform.position = movePos;
        }
    }
}