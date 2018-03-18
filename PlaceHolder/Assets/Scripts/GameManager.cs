using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameObject player;
        public GameObject playerPrefab;
        public MouseController mouseController;
        public LevelController levelController;        
        public bool canMove;
        public bool infoShown;
        public bool firstSpawn;

        [SerializeField] List<Guard> guards;

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
            }
        }

        private void Update()
        {
            
        }

        public void Reset()
        {
            
        }
    }
}