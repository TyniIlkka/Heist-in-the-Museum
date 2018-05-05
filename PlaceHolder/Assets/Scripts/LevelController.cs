﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ProjectThief.States;
using UnityEngine.UI;

namespace ProjectThief
{
    public class LevelController : MonoBehaviour
    {
        private MouseController m_mcController;

        [SerializeField, Header("Hud items")]
        private GameObject m_goEndBackground;
        [SerializeField]
        private GameObject m_goDefeat;
        [SerializeField]
        private GameObject m_goVictory;
        [SerializeField, Tooltip("Info screen")]
        private GameObject m_goScreen;
        [SerializeField]
        private Button m_bPauseButton;
        [SerializeField]
        private GameObject m_goContinueText;
        [SerializeField, Header("Scripts")]
        private RoomReset m_sRoomReset;
        [SerializeField, Tooltip("Main Camera")]
        private CameraFollow m_sCameraScript;
        [SerializeField]
        private Inventory m_sInventory;
        [SerializeField]
        private MenuControls m_sMenuControlscript;
        [SerializeField, Tooltip("Initial Spawn location"), Header("Other")]
        private Transform m_tInitialSpawn;
        [SerializeField, Tooltip("Scenes Doors")]
        private List<Door> m_lDoors;
        [SerializeField, Tooltip("Room position in list (starts from 0)")]
        private int m_iPos; 
        [SerializeField, Tooltip("Items neede to collect to advance into next phase")]
        private List<Item> m_lKeyItems;         
        [SerializeField]
        private bool m_bCanBeCleared;
        [SerializeField]
        private float _infoWaitTime = 5f;
        [SerializeField, Tooltip("Camera's distance from player"), Header("Camera settings")]
        private float m_fDist = 7f;
        [SerializeField, Tooltip("Camera's horizontal angle in room")]
        private float m_fHorizontalAngle = 0;
        [SerializeField, Tooltip("Camera's vertical angle in room")]
        private float m_fVerticalAngle = 35;

        private Vector3 m_v3SpawnPosition;        
        private Quaternion m_qSpawnRotation;
        private bool m_bJustCleared;
        private bool m_bIsCleared;
        private bool m_bPaused;
        private bool _timeStopped;
        private float _delay = 0;

        public bool JustCleared { get { return m_bJustCleared; } }
        public int ListPos { get { return m_iPos; } }
        public bool Cleared { get { return m_bIsCleared; } set { m_bIsCleared = value; } }
        public Inventory Inventory { get { return m_sInventory; } }
        public RoomReset RoomReset { get { return m_sRoomReset; } }
        public bool Paused { get { return m_bPaused; } set { m_bPaused = value; } }

        private void Awake()
        {
            if (m_sInventory == null)
                m_sInventory = FindObjectOfType<Inventory>();
            if (m_sRoomReset == null)
                m_sRoomReset = GetComponent<RoomReset>();

            if (m_bCanBeCleared)
            {
                if (GameManager.instance.clearedRooms[m_iPos])
                    m_bIsCleared = true;
            }
            else
            {
                m_bIsCleared = true;
            }

            m_bPaused = false;
            m_bJustCleared = false;            
            m_sCameraScript.Distance = m_fDist;
            m_sCameraScript.HorizontalAngle = m_fHorizontalAngle;
            m_sCameraScript.VerticalAngle = m_fVerticalAngle;

            GameManager.instance.levelController = this;
            GameManager.instance.inTransit = false;
            m_mcController = GameManager.instance.mouseController;
            GameManager.instance.player = SpawnPlayer();               

            if (!GameManager.instance.infoShown)
                Intro();     
            else
            {
                GameManager.instance.canMove = true;                
                m_goDefeat.SetActive(false);
                m_goEndBackground.SetActive(false);                
            }

            Time.timeScale = 1;
            _timeStopped = false;
            GameManager.instance.fadeInStart = true;
            GameManager.instance.fadeIn = false;
            Debug.Log("fade out");
        }

        // Update is called once per frame
        private void Update()
        {
            if (!GameManager.instance.infoShown)
            {
                _delay += Time.deltaTime;
                IntroEnd();                
            }            
            
            if (GameManager.instance.infoShown)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (!m_bPaused)                   
                        m_sMenuControlscript.Pause();
                    else
                        m_sMenuControlscript.Continue();
                }

                if (!m_bIsCleared && m_bCanBeCleared)
                    CheckKeyItems();
            }

            MouseOverUICheck();
        }

        /// <summary>
        /// Checks if mouse is over an ui element.
        /// </summary>
        private void MouseOverUICheck()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {                
                m_mcController.DefaultCursor();
                //GameManager.instance.canMove = false;
                GameManager.instance.mouseOverUI = true;
            }
            else if (!GameManager.instance.inTransit && GameManager.instance.infoShown)
            {
                GameManager.instance.canMove = true;
                GameManager.instance.mouseOverUI = false;
            }
        }

        /// <summary>
        /// Checks if key items needed to advance into 
        /// next phase have been collected.
        /// </summary>
        private void CheckKeyItems()
        {
            bool result = true;

            for (int i = 0; i < m_lKeyItems.Count; i++)
            {
                if (!m_lKeyItems[i].Collected)
                {
                    result = false;
                }
            }

            if (result && !m_bJustCleared)
            {
                m_bJustCleared = true;
                m_bIsCleared = true;
                GameManager.instance.clearedRooms[m_iPos] = true;
                GameManager.instance.currentPhase++;
            }
        }

        /// <summary>
        /// If player is found call then 
        /// this method is called.
        /// </summary>
        public void PlayerFound()
        {
            if (!_timeStopped)
                Time.timeScale = 0f;

            GameManager.instance.canMove = false;            
            _timeStopped = true;
            m_goEndBackground.SetActive(true);
            m_goDefeat.SetActive(true);            
        }

        /// <summary>
        /// If player get to exit point with 
        /// objective this method is called.
        /// </summary>
        public void PlayerEscaped()
        {
            if (!_timeStopped)
                Time.timeScale = 0f;

            GameManager.instance.canMove = false;
            _timeStopped = true;
            m_goEndBackground.SetActive(true);
            m_goVictory.SetActive(true);
        }
        
        private void Intro()
        {
            m_goScreen.SetActive(true);
            GameManager.instance.canMove = false;                       
        }

        private void IntroEnd()
        {
            if (_delay >= _infoWaitTime)
            {
                m_goContinueText.SetActive(true);

                if (Input.anyKey)
                {
                    GameManager.instance.infoShown = true;
                    m_goScreen.SetActive(false);
                    m_goContinueText.SetActive(false);
                    GameManager.instance.canMove = true;
                    Time.timeScale = 1f;
                    _delay = 0;

                    GameManager.instance.infoFadeIn = true;
                    GameManager.instance.infoFadeInStart = true;
                }
            }
        }

        private GameObject SpawnPlayer()
        {
            GameObject player = GameManager.instance.playerPrefab;

            if (!GameManager.instance.firstSpawn)
            {
                m_v3SpawnPosition = SpawnPosition();
                m_qSpawnRotation = SpawnRotation();

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
        
        private Vector3 SpawnPosition()
        {
            Vector3 result = Vector3.zero;

            if (GameStateController.CurrentState.SceneName == "Tutorial")
            {
                if (GameManager.instance.previousState.SceneName == "Lobby")
                    result = m_lDoors[0].SpawnPoint.position;
                else
                    result = m_tInitialSpawn.position;
            }
            else if (GameStateController.CurrentState.SceneName == "Lobby")
            {
                if (GameManager.instance.previousState.SceneName == "RoomVault")
                    result = m_lDoors[0].SpawnPoint.position;

                else if (GameManager.instance.previousState.SceneName == "Room1")
                    result = m_lDoors[1].SpawnPoint.position;

                else if (GameManager.instance.previousState.SceneName == "Room2")
                    result = m_lDoors[2].SpawnPoint.position;

                else if (GameManager.instance.previousState.SceneName == "Tutorial")
                    result = m_lDoors[3].SpawnPoint.position;

                else
                    Debug.LogError("ERROR Spawnpoint Not Found! ");                
            }
            else if (GameStateController.CurrentState.SceneName == "Room1")
            {
                if (GameManager.instance.previousState.SceneName == "Lobby")
                    result = m_lDoors[0].SpawnPoint.position;

                else if (GameManager.instance.previousState.SceneName == "Room3")
                    result = m_lDoors[1].SpawnPoint.position;

                else
                    Debug.LogError("ERROR Spawnpoint Not Found!");
            }
            else
                result = m_lDoors[0].SpawnPoint.position;

            return result;
        }

        private Quaternion SpawnRotation()
        {
            Quaternion result = Quaternion.identity;

            if (GameStateController.CurrentState.SceneName == "Tutorial")
            {
                if (GameManager.instance.previousState.SceneName == "Lobby")
                    result = m_lDoors[0].SpawnPoint.rotation;
                else
                    result = m_tInitialSpawn.rotation;
            }
            else if (GameStateController.CurrentState.SceneName == "Lobby")
            {
                if (GameManager.instance.previousState.SceneName == "RoomVault")
                    result = m_lDoors[0].SpawnPoint.rotation;

                else if (GameManager.instance.previousState.SceneName == "Room1")
                    result = m_lDoors[1].SpawnPoint.rotation;

                else if (GameManager.instance.previousState.SceneName == "Room2")
                    result = m_lDoors[2].SpawnPoint.rotation;

                else if (GameManager.instance.previousState.SceneName == "Tutorial")
                    result = m_lDoors[3].SpawnPoint.rotation;

                else
                    Debug.LogError("ERROR Spawnpoint Not Found!");
            }
            else if (GameStateController.CurrentState.SceneName == "Room1")
            {
                if (GameManager.instance.previousState.SceneName == "Lobby")
                    result = m_lDoors[0].SpawnPoint.rotation;

                else if (GameManager.instance.previousState.SceneName == "Room3")
                    result = m_lDoors[1].SpawnPoint.rotation;

                else
                    Debug.LogError("ERROR Spawnpoint Not Found!");
            }
            else
                result = m_lDoors[0].SpawnPoint.rotation;

            return result;
        }
    }
}