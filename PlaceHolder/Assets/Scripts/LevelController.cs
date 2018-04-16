using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ProjectThief.States;
using UnityEngine.UI;

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
        [SerializeField]
        private Button m_bPauseButton;
        [SerializeField]
        private GameObject m_goContinueText;
        [SerializeField]
        private RoomReset m_sRoomReset;
        [SerializeField, Tooltip("Main Camera")]
        private CameraFollow m_sCameraScript;
        [SerializeField]
        private Inventory m_sInventory;
        [SerializeField]
        private MenuControls m_sMenuControlscript;
        [SerializeField, Tooltip("Initial Spawn location")]
        private Transform m_tInitialSpawn;
        [SerializeField, Tooltip("Scenes Doors")]
        private List<Door> m_lDoors;
        [SerializeField, Tooltip("Room position in list (starts from 0)")]
        private int m_iPos;        
        [SerializeField, Tooltip("Camera's distance from player")]
        private float m_fDist = 7f;
        [SerializeField, Tooltip("Camera's angle in room")]
        private float m_fAngle = 0;
        [SerializeField, Tooltip("Items neede to collect to advance into next phase")]
        private List<Item> m_lKeyItems;         
        [SerializeField]
        private bool m_bCanBeCleared;

        private Vector3 m_v3SpawnPosition;        
        private Quaternion m_qSpawnRotation;
        private bool m_bJustCleared;
        private bool m_bIsCleared;
        private bool m_bPaused;
        private float m_fDelay = 0;

        public bool JustCleared { get { return m_bJustCleared; } }
        public int ListPos { get { return m_iPos; } }
        public bool Cleared { get { return m_bIsCleared; } set { m_bIsCleared = value; } }
        public Inventory Inventory { get { return m_sInventory; } }
        public RoomReset RoomReset { get { return m_sRoomReset; } }

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

            m_bJustCleared = false;            
            m_sCameraScript.Distance = m_fDist;
            m_sCameraScript.Angle = m_fAngle;

            GameManager.instance.levelController = this;            
            m_mcController = GameManager.instance.mouseController;
            GameManager.instance.player = SpawnPlayer();               

            if (!GameManager.instance.infoShown)
                Intro();     
            else
            {
                GameManager.instance.canMove = true;
                Time.timeScale = 1f;
                m_goDefeat.SetActive(false);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (!GameManager.instance.infoShown)
            {
                IntroEnd();
                m_fDelay += 0.02f;
            }            

            MouseOverUICheck();
            if (GameManager.instance.infoShown)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (!m_bPaused)
                    {
                        m_sMenuControlscript.Pause();
                        m_bPaused = true;
                    }
                    else
                    {
                        m_sMenuControlscript.Continue();
                        m_bPaused = false;
                    }
                }

                if (!m_bIsCleared && m_bCanBeCleared)
                    CheckKeyItems();
            }
        }

        /// <summary>
        /// Checks if mouse is over an ui element.
        /// </summary>
        private void MouseOverUICheck()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                
                m_mcController.DefaultCursor();
                GameManager.instance.canMove = false;
                GameManager.instance.mouseOverUI = true;
            }
            else
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
            GameManager.instance.canMove = false;
            Time.timeScale = 0f;
            m_goDefeat.SetActive(true);            
        }

        /// <summary>
        /// If player get to exit point with 
        /// objective this method is called.
        /// </summary>
        public void PlayerEscaped()
        {
            GameManager.instance.canMove = false;
            Time.timeScale = 0f;
            m_goVictory.SetActive(true);
        }
        
        private void Intro()
        {
            m_goScreen.SetActive(true);
            GameManager.instance.canMove = false;
            Time.timeScale = 0f;            
        }

        private void IntroEnd()
        {
            if (m_fDelay >= 5f)
            {
                m_goContinueText.SetActive(true);

                if (Input.anyKey)
                {
                    GameManager.instance.infoShown = true;
                    m_goScreen.SetActive(false);
                    m_bPauseButton.interactable = true;
                    GameManager.instance.canMove = true;
                    Time.timeScale = 1f;
                    m_fDelay = 0;
                }
            }
        }

        private GameObject SpawnPlayer()
        {
            GameObject player = GameManager.instance.playerPrefab;

            if (!GameManager.instance.firstSpawn)
            {
                // TODO update spawn position & rotation.

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

            if (GameStateController.CurrentState.SceneName == "Lobby")
            {
                if (GameManager.instance.previousState.SceneName == "RoomVault")
                    result = m_lDoors[0].SpawnPoint.position;

                else if (GameManager.instance.previousState.SceneName == "Room1")
                    result = m_lDoors[1].SpawnPoint.position;

                else if (GameManager.instance.previousState.SceneName == "Room2")
                    result = m_lDoors[2].SpawnPoint.position;

                else if (GameManager.instance.previousState.SceneName == "MainMenu")
                    result = m_tInitialSpawn.position;

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

            if (GameStateController.CurrentState.SceneName == "Lobby")
            {
                if (GameManager.instance.previousState.SceneName == "RoomVault")
                    result = m_lDoors[0].SpawnPoint.rotation;

                else if (GameManager.instance.previousState.SceneName == "Room1")
                    result = m_lDoors[1].SpawnPoint.rotation;

                else if (GameManager.instance.previousState.SceneName == "Room2")
                    result = m_lDoors[2].SpawnPoint.rotation;

                else if (GameManager.instance.previousState.SceneName == "MainMenu")
                    result = m_tInitialSpawn.rotation;

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