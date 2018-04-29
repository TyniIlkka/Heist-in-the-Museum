using ProjectThief.States;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        #region Variables
        [Header("Variables")]
        public GameObject player;
        public GameObject playerPrefab;
        public MouseController mouseController;
        public AudioManager audioManager;
        public LevelController levelController;        
        public bool canMove;
        public bool infoShown;
        public bool firstSpawn;
        public bool mouseOverUI;
        public bool canContinue;
        public Transform spawnPoint;
        public Transform initialSpawn;
        public GameStateBase previousState;
        public GameStateBase continueState;
        public int currentPhase;
        public LayerMask rayCastLayers;        
        #endregion
        #region Lists
        [Header("Lists")]
        public List<Item> refItems;
        public List<Item> keyItems;
        public List<Item> inventory;                
        public bool[] usedlevers;        
        public bool[] openedVitrines;
        public bool[] clearedRooms;
        [SerializeField] List<Guard> guards;
        #endregion

        private void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }            

            inventory = new List<Item>();            
            usedlevers = new bool[5];            
            openedVitrines = new bool[4];
            clearedRooms = new bool[5];
            canContinue = false;
        }         

        public void ResetGame()
        {
            for (int i = 0; i < refItems.Count; i++)
            {
                refItems[i].Collected = false;

                if (i < openedVitrines.Length)
                {                    
                    keyItems[i].Collected = false;
                    openedVitrines[i] = false;                    
                }
                if (i < clearedRooms.Length)
                {
                    clearedRooms[i] = false;
                    usedlevers[i] = false;
                }
            }

            inventory.Clear();
            currentPhase = 0;
        }            
    }
}