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
        public bool guardsCanMove;
        public bool infoShown;
        public bool firstSpawn;
        public bool mouseOverUI;
        public bool canContinue;
        public bool inTransit;
        public bool fadeIn;
        public bool fadeInStart;
        public bool infoFadeIn;
        public bool infoFadeInStart;
        public bool resetInfoTimer;
        public bool initialMenu;
        public bool infoBoxVisible;
        public bool newText;
        public string infoText;
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
        public List<Item> savedInventory;
        public bool[] usedlevers;        
        public bool[] openedVitrines;
        public bool[] clearedRooms;
        public bool[] infoTextShown;
        public bool[] tutorialeffects;
        [SerializeField] List<Guard> guards;
        #endregion
        [Header("Debug variables")]
        public bool mouseMovemet;

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
            initialMenu = true;
            infoTextShown = new bool[7];
            tutorialeffects = new bool[2];
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
                if (i < infoTextShown.Length)
                    infoTextShown[i] = false;
                if (i < tutorialeffects.Length)
                    tutorialeffects[i] = false;
            }

            inventory.Clear();
            savedInventory.Clear();
            currentPhase = 0;
        }   
        
        public void SaveInventory(List<Item> inventory)
        {
            savedInventory.Clear();

            foreach (Item item in inventory)
            {
                Item savedItem = refItems[item.RefPos];
                savedInventory.Add(savedItem);
            }
        }
    }
}