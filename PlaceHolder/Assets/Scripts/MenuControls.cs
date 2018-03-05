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

        private MouseController m_mcController;

        private void Awake()
        {
            m_mcController = GameManager.instance.mouseController;
            // TODO update volume
        }

        public void NewGame()
        {
            SceneManager.LoadScene("Level");
        }

        public void Options()
        {

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
            //m_goShade.SetActive(true);
            //m_goPauseMenu.SetActive(true);
            //m_bPauseButton.interactable = false;
            //// Animation
            //Time.timeScale = 0f;
            Debug.Log("Click");
        }

        public void Continue()
        {
            m_goShade.SetActive(false);
            m_goPauseMenu.SetActive(false);
            m_bPauseButton.interactable = true;
            // Animation
            Time.timeScale = 1f;
        }

    }
}