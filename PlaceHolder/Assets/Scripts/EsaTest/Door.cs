﻿using System.Collections;
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

        private bool m_bIsBlocked;
        private MouseController m_mcMouseController;        

        public Vector3 LobbyPos { get { return m_v3LobbyPos.position; } }
        public Vector3 RoomPos { get { return m_v3RoomPosition.position; } }
        public bool Blocked { set { m_bIsBlocked = value; } }

        /// <summary>
        /// Door scripts initialization.
        /// </summary>
        private void Awake()
        {
            m_bIsBlocked = true;
            m_mcMouseController = GameManager.instance.mouseController;
        }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        protected override void OnMouseOver()
        {            
            if (IsActive)
            {
                m_mcMouseController.InteractCursor();

                if (!m_bIsBlocked)
                {
                    m_mcMouseController.EnterCursor();               

                    if (Input.GetMouseButton(0))
                    {
                        if (IsInteractable)
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
                m_mcMouseController.InspectCursor();
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
            }
            else
            {
                movePos = LobbyPos;
                GameManager.instance.lastPosition = LobbyPos;
            }

            // TODO Fade in and fade out effect.
            player.transform.position = movePos;
        }

        protected override void OnMouseExit()
        {
            m_mcMouseController.DefaultCursor();
        }
    }
}