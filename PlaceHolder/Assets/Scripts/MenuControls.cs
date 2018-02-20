using UnityEngine.UI;
using UnityEngine;

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

        private void Awake()
        {
            // TODO update volume
        }

        public void NewGame()
        {

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
            m_goShade.SetActive(true);
            m_goPauseMenu.SetActive(true);
            m_bPauseButton.interactable = false;
            //view1 & 2?
            Time.timeScale = 0f;
        }

        public void Continue()
        {
            m_goShade.SetActive(false);
            m_goPauseMenu.SetActive(false);
            m_bPauseButton.interactable = true;
            //view1 & 2?
            Time.timeScale = 1f;
        }

    }
}