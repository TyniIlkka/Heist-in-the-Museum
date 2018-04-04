using System.Collections.Generic;
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
        [SerializeField, Tooltip("Main Camera")]
        private CameraFollow m_sCameraScript;
        [SerializeField, Tooltip("Camera's distance from player")]
        private float m_fDist = 7f;
        [SerializeField, Tooltip("Camera's angle in room")]
        private float m_fAngle = 0;
        [SerializeField, Tooltip("Items neede to collect to advance into next phase")]
        private List<Item> m_lKeyItems;

        private Vector3 m_v3SpawnPosition;        
        private Quaternion m_qSpawnRotation;              

        private void Awake()
        {
            Debug.Log("Current state: " + GameStateController.CurrentState);
            m_sCameraScript.Distance = m_fDist;
            m_sCameraScript.Angle = m_fAngle;

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
            CheckKeyItems();
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

            if (result)
            {
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
            GameManager.instance.infoShown = true;
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

                else
                    Debug.LogError("ERROR Spawnpoint Not Found!");
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