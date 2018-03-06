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

        private void Awake()
        {
            GameManager.instance.levelController = this;
            m_mcController = GameManager.instance.mouseController;            
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