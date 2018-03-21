using UnityEngine.UI;
using UnityEngine;
using ProjectThief.States;

namespace ProjectThief
{
    public class MenuControls : MonoBehaviour
    {        
        [SerializeField]
        private Slider m_sSfxVol;
        [SerializeField]
        private Slider m_sMusicVol;
        [SerializeField]
        private GameStateType _nextState;
        [SerializeField]
        private GameStateType _menuState;

        #region Pause controls
        [SerializeField]
        private GameObject m_goPauseMenu;
        [SerializeField]
        private GameObject m_goShade;
        [SerializeField]
        private GameObject m_goPausePlan;
        [SerializeField]
        private GameObject m_goPauseMenuBg;
        [SerializeField]
        private Button m_bPauseButton;
        [SerializeField]
        private GameObject m_goMenuConfirm;
        [SerializeField]
        private GameObject m_goExitConfirm;
        #endregion

        [SerializeField]
        private GameObject m_goPlan;

        private void Awake()
        {            
            // TODO update volume
        }

        public void NewGame()
        {
            GameManager.instance.firstSpawn = true;
            GameManager.instance.infoShown = false;            
            GameStateController.PerformTransition(_nextState);
        }

        public void Options()
        {
            m_goPauseMenu.SetActive(false);
        }

        public void Return()
        {
            m_goPauseMenu.SetActive(true);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void MusicVol()
        {

        }

        public void SFXVol()
        {

        }

        public void Pause()
        {            
            m_goPausePlan.SetActive(true);
            m_goPauseMenuBg.SetActive(true);
            m_goPauseMenu.SetActive(true);
            m_bPauseButton.interactable = false;
            GameManager.instance.canMove = false;
            Time.timeScale = 0f;            
        }

        public void Continue()
        {            
            m_goPausePlan.SetActive(false);
            m_goPauseMenuBg.SetActive(false);
            m_goPauseMenu.SetActive(false);            
            m_bPauseButton.interactable = true;
            GameManager.instance.canMove = true;
            Time.timeScale = 1f;
        }

        public void MainMenu()
        {
            m_goPauseMenu.SetActive(false);
            m_goMenuConfirm.SetActive(true);
        }

        public void MenuYes()
        {            
            m_goPauseMenu.SetActive(false);
            m_bPauseButton.interactable = true;
            Time.timeScale = 1f;
            GameStateController.PerformTransition(_menuState);
        }

        public void MenuNo()
        {
            m_goMenuConfirm.SetActive(false);
            m_goPauseMenu.SetActive(true);
        }

        public void PauseExit()
        {
            m_goPauseMenu.SetActive(false);
            m_goExitConfirm.SetActive(true);            
        }

        public void ExitYes()
        {
            ExitGame();
        }

        public void ExitNo()
        {
            m_goExitConfirm.SetActive(false);
            m_goPauseMenu.SetActive(true);
        }

        public void IntroContinue()
        {
            m_goPlan.SetActive(false);
            m_bPauseButton.interactable = true;
            GameManager.instance.canMove = true;
            Time.timeScale = 1f;
        }
    }
}