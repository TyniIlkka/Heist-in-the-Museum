using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectThief
{
    public class LevelController : MonoBehaviour
    {
        private MouseController m_mcController;

        [SerializeField]
        private GameObject m_goDefeat;
        [SerializeField]
        private GameObject m_goVictory;
        [SerializeField]
        private GameObject m_goPlayer;
        [SerializeField, Tooltip("Info screen")]
        private GameObject m_goScreen;

        private void Awake()
        {
            GameManager.instance.levelController = this;
            GameManager.instance.player = m_goPlayer;
            m_mcController = GameManager.instance.mouseController;
            m_goScreen.SetActive(true);
        }

        // Update is called once per frame
        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                m_mcController.DefaultCursor();
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
        
        
    }
}