using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameObject player;
        public MouseController mouseController;
        public LevelController levelController;
        public Vector3 lastPosition;
        public bool lobbyIsActive;
        public bool moveToObject;
        public IObject targetObject;

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
    }
}