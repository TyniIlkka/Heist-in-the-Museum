using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectThief
{
    public class MenuControls : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_goMenuView1;
        [SerializeField]
        private GameObject m_goMenuView2;
        [SerializeField]
        private Slider m_sSfxVol;
        [SerializeField]
        private Slider m_sMusicVol;

        [SerializeField]
        private GameObject m_goPauseMenu;
        [SerializeField]
        private GameObject m_goShade;
        [SerializeField]
        private Button m_bPauseButton;
        [SerializeField]
        private GameObject m_goMenuConfirm;
        [SerializeField]
        private GameObject m_goExitConfirm;        

        private void Awake()
        {            
            // TODO update volume
        }

        public void NewGame()
        {
            SceneManager.LoadScene("Level");
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
            m_goShade.SetActive(true);
            m_goPauseMenu.SetActive(true);
            m_bPauseButton.interactable = false;
            Time.timeScale = 0f;            
        }

        public void Continue()
        {
            m_goShade.SetActive(false);
            m_goPauseMenu.SetActive(false);
            m_bPauseButton.interactable = true;
            Time.timeScale = 1f;
        }

        public void MainMenu()
        {
            m_goPauseMenu.SetActive(false);
            m_goMenuConfirm.SetActive(true);
        }

        public void MenuYes()
        {
            m_goShade.SetActive(false);
            m_goPauseMenu.SetActive(false);
            m_bPauseButton.interactable = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
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
    }
}