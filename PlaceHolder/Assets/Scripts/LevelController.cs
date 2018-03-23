﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ProjectThief.States;

namespace ProjectThief
{
    public class LevelController : MonoBehaviour
    {
        private MouseController m_mcController;

        [SerializeField]
        private GameObject m_goDefeat;
        [SerializeField]
        private GameObject m_goVictory;        
        [SerializeField, Tooltip("Info screen")]
        private GameObject m_goScreen;
        [SerializeField, Tooltip("Initial Spawn location")]
        private Transform m_tInitialSpawn;
        [SerializeField, Tooltip("Scenes Doors")]
        private List<Door> m_lDoors;
        [SerializeField, Tooltip("Room position in list (starts from 0)")]
        private int m_iPos;
        
        private Vector3 m_v3SpawnPosition;        
        private Quaternion m_qSpawnRotation;
        private bool m_bSaved;        

        private void Awake()
        {
            Debug.Log("Current state: " + GameStateController.CurrentState);
            m_bSaved = false;

            GameManager.instance.levelController = this;            
            m_mcController = GameManager.instance.mouseController;
            GameManager.instance.player = SpawnPlayer();               

            if (!GameManager.instance.infoShown)
                Intro();            
        }

        // Update is called once per frame
        private void Update()
        {
            MouseOverHudCheck();           
        }

        /// <summary>
        /// Checks if mouse is over hud.
        /// </summary>
        private void MouseOverHudCheck()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                m_mcController.DefaultCursor();
                GameManager.instance.canMove = false;
            }
            else
            {
                GameManager.instance.canMove = true;
            }
        }

        /// <summary>
        /// If player is found call then 
        /// this method is called.
        /// </summary>
        public void PlayerFound()
        {            
            m_goDefeat.SetActive(true);            
        }

        /// <summary>
        /// If player get to exit point with 
        /// objective this method is called.
        /// </summary>
        public void PlayerEscaped()
        {
            m_goVictory.SetActive(true);
        }
        
        private void Intro()
        {
            m_goScreen.SetActive(true);
            GameManager.instance.canMove = false;
            Time.timeScale = 0f;
            GameManager.instance.infoShown = true;
        }

        private GameObject SpawnPlayer()
        {
            GameObject player = GameManager.instance.playerPrefab;

            if (!GameManager.instance.firstSpawn)
            {
                // TODO update spawn position & rotation.

                m_v3SpawnPosition = m_lDoors[0].SpawnPoint.position;
                m_qSpawnRotation = m_lDoors[0].SpawnPoint.rotation;

                return Instantiate(player, m_v3SpawnPosition, m_qSpawnRotation);
            }

            else
            {
                m_v3SpawnPosition = m_tInitialSpawn.position;
                m_qSpawnRotation = m_tInitialSpawn.rotation;
                GameManager.instance.firstSpawn = false;

                return Instantiate(player, m_v3SpawnPosition, m_qSpawnRotation);
            }
        }     
    }
}