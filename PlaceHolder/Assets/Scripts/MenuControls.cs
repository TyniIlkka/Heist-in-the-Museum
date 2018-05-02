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
        private Slider m_sMasterVol;
        [SerializeField]
        private GameStateType _nextState;
        [SerializeField]
        private GameStateType _menuState;
        [SerializeField]
        private GameObject m_goView1;
        [SerializeField]
        private GameObject m_goView2;
        [SerializeField, Tooltip("Continue button")]
        private Button m_bContinueButton;

        #region Pause controls
        [SerializeField]
        private GameObject m_goPauseMenu;
        [SerializeField]
        private GameObject m_goPauseOptions;
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

        private AudioManager m_amAudioManager;        

        private void Awake()
        {
            m_amAudioManager = GameManager.instance.audioManager;
            
            m_sSfxVol.value = (int)(m_amAudioManager.SfxVol * 100);
            m_sMusicVol.value = (int)(m_amAudioManager.MusicVol * 100);
            m_sMasterVol.value = (int)(m_amAudioManager.MasterVol * 100);
        }

        private void Update()
        {
            if (GameStateController.CurrentState.SceneName == "MainMenu")
            {
                if (GameManager.instance.canContinue)
                    m_bContinueButton.gameObject.SetActive(true);
                else
                    m_bContinueButton.gameObject.SetActive(false);
            }
        }

        public void NewGame()
        {
            GameManager.instance.ResetGame();
            GameManager.instance.firstSpawn = true;
            GameManager.instance.infoShown = false;
            GameManager.instance.previousState = GameStateController.CurrentState;
            GameStateController.PerformTransition(_nextState);
        }

        public void Options()
        {
            m_goView1.SetActive(false);
            m_goView2.SetActive(true);
        }

        public void Back()
        {
            m_goView2.SetActive(false);
            m_goView1.SetActive(true);   
            // TODO Save volume?
        }

        public void PauseMenu()
        {
            m_goPauseMenu.SetActive(false);
            m_goPauseOptions.SetActive(true);
        }

        public void Return()
        {
            m_goPauseOptions.SetActive(false);
            m_goPauseMenu.SetActive(true);
            // TODO Save volume?
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void MusicVol()
        {
            m_amAudioManager.MusicVol = (m_sMusicVol.value / 100);
        }

        public void SFXVol()
        {
            m_amAudioManager.SfxVol = (m_sSfxVol.value / 100);
        }

        public void MasterVol()
        {
            m_amAudioManager.MasterVol = (m_sMasterVol.value / 100);
        }

        public void Pause()
        {
            GameManager.instance.levelController.Paused = true;
            m_goPausePlan.SetActive(true);
            m_goPauseMenuBg.SetActive(true);
            m_goPauseMenu.SetActive(true);
            //m_bPauseButton.interactable = false;
            GameManager.instance.canMove = false;
            Time.timeScale = 0f;            
        }

        public void Continue()
        {
            GameManager.instance.levelController.Paused = false;
            m_goPausePlan.SetActive(false);
            m_goPauseMenuBg.SetActive(false);
            m_goPauseMenu.SetActive(false);
            m_goPauseOptions.SetActive(false);
            //m_bPauseButton.interactable = true;
            GameManager.instance.canMove = true;
            Time.timeScale = 1f;
        }

        public void MainMenu()
        {
            m_goPauseMenu.SetActive(false);
            m_goMenuConfirm.SetActive(true);
        }

        public void ReturnMenuOther()
        {            
            m_goPauseMenu.SetActive(false);
            //m_bPauseButton.interactable = true;
            GameManager.instance.canContinue = true;
            GameManager.instance.continueState = GameStateController.CurrentState;
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
        
        public void LoadCheckpoint()
        {            
            GameManager.instance.levelController.RoomReset.ResetRoom();
            GameStateType resetState = GameStateController.CurrentState.StateType;            
            GameStateController.PerformTransition(resetState);
        }

        public void ReturnMenuVictory()
        {
            m_goPauseMenu.SetActive(false);
            //m_bPauseButton.interactable = true;
            GameManager.instance.canContinue = true;
            Time.timeScale = 1f;
            GameStateController.PerformTransition(_menuState);
        }

        public void ContinueGame()
        {
            Debug.Log("Pressed continue");
            GameStateController.PerformTransition(
                GameManager.instance.continueState.StateType);
        }
    }
}