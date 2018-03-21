using ProjectThief.States;
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
        public Transform spawnPoint;
        public GameStateBase previousState;

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
            if (previousState != null)
                Debug.Log("Previous State: " + previousState.SceneName);
        }

        public void Reset()
        {
            
        }
    }
}