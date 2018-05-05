using UnityEngine.UI;
using UnityEngine;
using ProjectThief.States;

namespace ProjectThief
{
    public class MenuControls : MonoBehaviour
    {        
        [SerializeField, Header("Audio sliders")]
        private Slider _sfxVol;
        [SerializeField]
        private Slider _musicVol;
        [SerializeField]
        private Slider _masterVol;
        [SerializeField, Header("Game state")]
        private GameStateType _nextState;
        [SerializeField]
        private GameStateType _menuState;
        [SerializeField, Header("Main menu views")]
        private GameObject _menuView1;
        [SerializeField]
        private GameObject _menuView2;
        [SerializeField, Tooltip("Continue button")]
        private Button _continueButton;
        [SerializeField, Header("Fade In/Out")]
        private RawImage _fadeScreen;
        [SerializeField, Tooltip("Fade in/ out effect duration")]
        private float _duration = 2;

        #region Pause controls
        [SerializeField, Header("Pause menu")]
        private GameObject _pauseMenu;
        [SerializeField]
        private GameObject _pauseOptions;        
        [SerializeField]
        private GameObject _pauseBg;
        [SerializeField]
        private GameObject _pauseMenuBg;        
        [SerializeField]
        private GameObject _menuConfirm;
        [SerializeField]
        private GameObject _exitConfirm;
        #endregion

        [SerializeField, Header("Info screen")]
        private GameObject _infoBg;

        [SerializeField, Header("Info text")]
        private RawImage _textBg;
        [SerializeField]
        private Text _infoText;
        [SerializeField, Tooltip("Text hide delay")]
        private float _delay = 5f;
        [SerializeField, Tooltip("Info fade effect duration")]
        private float _infoDuration = 2f;

        private AudioManager _audioManager;
        private float _r, _g, _b;
        private float _start, _infoStart;
        private bool _newGame;
        private bool _returnMenu;
        private float _timePassed;
        private float _rInfo, _gInfo, _bInfo;
        private float _rText, _gText, _bText;
        private bool _infoVisible;

        private void Awake()
        {
            _audioManager = GameManager.instance.audioManager;
            _newGame = false;
            _returnMenu = false;

            _sfxVol.value = (int)(_audioManager.SfxVol * 100);
            _musicVol.value = (int)(_audioManager.MusicVol * 100);
            _masterVol.value = (int)(_audioManager.MasterVol * 100);

            _r = _fadeScreen.color.r;
            _g = _fadeScreen.color.g;
            _b = _fadeScreen.color.b;

            if (GameStateController.CurrentState.SceneName != "MainMenu")
            {
                _rInfo = _textBg.color.r;
                _gInfo = _textBg.color.g;
                _bInfo = _textBg.color.b;

                _rText = _infoText.color.r;
                _gText = _infoText.color.g;
                _bText = _infoText.color.b;
            }

            if (GameStateController.CurrentState.SceneName == "MainMenu")
            {
                if (!GameManager.instance.initialMenu)
                    _fadeScreen.color = new Vector4(_r, _g, _b, 1);

                if (_fadeScreen.color.a != 0)
                {
                    GameManager.instance.fadeInStart = true;
                    GameManager.instance.fadeIn = false;
                }

                GameManager.instance.initialMenu = false;
            }
        }

        private void Update()
        {
            if (GameStateController.CurrentState.SceneName == "MainMenu")
            {
                if (GameManager.instance.canContinue)
                    _continueButton.gameObject.SetActive(true);
                else
                    _continueButton.gameObject.SetActive(false);
            }

            if (GameManager.instance.fadeInStart)
            {                
                _start = Time.time;
                GameManager.instance.fadeInStart = false;

                if (GameManager.instance.fadeIn)                
                    _fadeScreen.color = new Vector4(_r, _g, _b, 0);   
                else
                    _fadeScreen.color = new Vector4(_r, _g, _b, 1);
            }

            if (GameManager.instance.fadeIn && _fadeScreen.color.a != 1)            
                FadeIn();
            
            if (!GameManager.instance.fadeIn && _fadeScreen.color.a != 0)            
                FadeOut();
            

            if (_newGame && _fadeScreen.color.a == 1)
            {
                _newGame = false;
                GameManager.instance.previousState = GameStateController.CurrentState;
                GameStateController.PerformTransition(_nextState);
            }
            if (_returnMenu && _fadeScreen.color.a == 1)
            {
                _returnMenu = false;
                GameStateController.PerformTransition(_menuState);
            }

            if (GameStateController.CurrentState.SceneName != "MainMenu")
            {
                if (GameManager.instance.infoFadeInStart)
                {
                    _infoStart = Time.time;
                    GameManager.instance.infoFadeInStart = false;

                    if (GameManager.instance.infoFadeIn)
                    {
                        _textBg.color = new Vector4(_rInfo, _gInfo, _bInfo, 0);
                        _infoVisible = true;
                    }
                    else
                        _textBg.color = new Vector4(_rInfo, _gInfo, _bInfo, 1);
                }

                if (_textBg != null)
                {
                    if (_infoVisible && _textBg.color.a == 1)
                        InfoTimer();

                    if (_infoVisible && _textBg.color.a != 1)
                        FadeInInfo();

                    if (!_infoVisible && _textBg.color.a != 0)
                        FadeOutInfo();
                }
            }
        }

        private void InfoTimer()
        {
            _timePassed += Time.deltaTime;

            if (_timePassed >= _delay)
            {
                _timePassed = 0;
                _infoVisible = false;
                _infoStart = Time.time;
            }
        }

        private void FadeInInfo()
        {
            float progress = Time.time - _infoStart;
            _infoText.color = Color.Lerp(_infoText.color, new Vector4(_rText, _gText, _bText, 1), progress / _infoDuration);
            _textBg.color = Color.Lerp(_textBg.color, new Vector4(_rInfo, _gInfo, _bInfo, 1), progress / _infoDuration);
            Debug.Log("Fade in");
        }

        private void FadeOutInfo()
        {
            float progress = Time.time - _infoStart;
            _infoText.color = Color.Lerp(_infoText.color, new Vector4(_rText, _gText, _bText, 0), progress / _infoDuration);
            _textBg.color = Color.Lerp(_textBg.color, new Vector4(_rInfo, _gInfo, _bInfo, 0), progress / _infoDuration);
            Debug.Log("Fade out");
        }

        private void FadeIn()
        {
            float progress = Time.time - _start;
            _fadeScreen.color = Color.Lerp(_fadeScreen.color, new Vector4(_r, _g, _b, 1), progress / _duration);            
        }

        private void FadeOut()
        {
            float progress = Time.time - _start;
            _fadeScreen.color = Color.Lerp(_fadeScreen.color, new Vector4(_r, _g, _b, 0), progress / _duration);
        }

        public void NewGame()
        {
            GameManager.instance.fadeIn = true;
            GameManager.instance.fadeInStart = true;
            GameManager.instance.ResetGame();
            GameManager.instance.firstSpawn = true;
            GameManager.instance.infoShown = false;
            _newGame = true;
        }

        public void Options()
        {
            _menuView1.SetActive(false);
            _menuView2.SetActive(true);
        }

        public void Back()
        {
            _menuView2.SetActive(false);
            _menuView1.SetActive(true);   
        }

        public void PauseMenu()
        {
            _pauseMenu.SetActive(false);
            _pauseOptions.SetActive(true);
        }

        public void Return()
        {
            _pauseOptions.SetActive(false);
            _pauseMenu.SetActive(true);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void MusicVol()
        {
            _audioManager.MusicVol = (_musicVol.value / 100);
        }

        public void SFXVol()
        {
            _audioManager.SfxVol = (_sfxVol.value / 100);
        }

        public void MasterVol()
        {
            _audioManager.MasterVol = (_masterVol.value / 100);
        }

        public void Pause()
        {
            GameManager.instance.levelController.Paused = true;
            _pauseBg.SetActive(true);
            _pauseMenuBg.SetActive(true);
            _pauseMenu.SetActive(true);
            GameManager.instance.canMove = false;
            Time.timeScale = 0f;            
        }

        public void Continue()
        {
            GameManager.instance.levelController.Paused = false;
            _pauseBg.SetActive(false);
            _pauseMenuBg.SetActive(false);
            _pauseMenu.SetActive(false);
            _pauseOptions.SetActive(false);
            _menuConfirm.SetActive(false);
            _exitConfirm.SetActive(false);
            GameManager.instance.canMove = true;
            Time.timeScale = 1f;
        }

        public void MainMenu()
        {
            _pauseMenu.SetActive(false);
            _menuConfirm.SetActive(true);
        }

        public void ReturnMenuOther()
        {            
            _pauseMenu.SetActive(false);
            GameManager.instance.canContinue = true;
            GameManager.instance.continueState = GameStateController.CurrentState;
            Time.timeScale = 1f;
            _returnMenu = true;
            GameManager.instance.fadeIn = true;
            GameManager.instance.fadeInStart = true;
        }

        public void MenuNo()
        {
            _menuConfirm.SetActive(false);
            _pauseMenu.SetActive(true);
        }

        public void PauseExit()
        {
            _pauseMenu.SetActive(false);
            _exitConfirm.SetActive(true);            
        }

        public void ExitYes()
        {
            ExitGame();
        }

        public void ExitNo()
        {
            _exitConfirm.SetActive(false);
            _pauseMenu.SetActive(true);
        } 
        
        public void LoadCheckpoint()
        {            
            GameManager.instance.levelController.RoomReset.ResetRoom();
            GameStateType resetState = GameStateController.CurrentState.StateType;            
            GameStateController.PerformTransition(resetState);
        }

        public void ReturnMenuVictory()
        {
            _pauseMenu.SetActive(false);
            GameManager.instance.canContinue = true;
            Time.timeScale = 1f;
            _returnMenu = true;
            GameManager.instance.fadeIn = true;
            GameManager.instance.fadeInStart = true;
        }

        public void ContinueGame()
        {
            GameStateController.PerformTransition(
                GameManager.instance.continueState.StateType);
        }
    }
}