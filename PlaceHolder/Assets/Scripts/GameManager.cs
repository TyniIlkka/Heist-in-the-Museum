﻿using ProjectThief.States;
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
        public Transform spawnPoint;
        public Transform initialSpawn;
        public GameStateBase previousState;
        public int currentPhase;
        #endregion
        #region Lists
        [Header("Lists")]
        public List<Item> refItems;
        public List<Item> keyItems;
        public List<Item> inventory;                
        public bool[] usedlevers;        
        public bool[] openedVitrines;
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
            usedlevers = new bool[4];            
            openedVitrines = new bool[4];                       
        }         

        public void ResetGame()
        {
            for (int i = 0; i < usedlevers.Length; i++)
            {                
                usedlevers[i] = false;                   
                openedVitrines[i] = false;                                   
                refItems[i].Collected = false;
            }

            inventory.Clear();
            currentPhase = 0;
        }    
        
        public void CheckInventory()
        {
            Debug.Log("Inventory: " + inventory.Count);

            for (int i = 0; i < inventory.Count; i++)
            {
                Debug.Log("Item " + i + " : " + inventory[i]);
                if (inventory[i] == null)
                    Debug.LogError("ERROR: ITEM REFERENCE IS NULL");
            }
        }
    }
}