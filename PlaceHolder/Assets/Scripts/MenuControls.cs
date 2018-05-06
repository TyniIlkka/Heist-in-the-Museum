using UnityEngine.UI;
using UnityEngine;
using ProjectThief.States;
using System.Collections.Generic;
using System.Collections;

namespace ProjectThief
{
    public class MenuControls : MonoBehaviour
    {
        #region SerializeField variables
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
        [SerializeField, Header("Info screen")]
        private GameObject _infoBg;
        [SerializeField, Header("Info text")]
        private RawImage _textBg;
        [SerializeField]
        private Text _infoText;
        [SerializeField, Tooltip("Text hide delay")]
        private float _delay = 2f;
        [SerializeField, Tooltip("Info fade effect duration")]
        private float _infoDuration = 2f;
        [SerializeField, Tooltip("Delay between text")]
        private float _textDelay = 0.5f;
        [SerializeField, Tooltip("Line end char")]
        private char _endchar = '#';
        [SerializeField, Tooltip("Delay between characters")]
        private float _charDelay = 0.025f;
        [SerializeField, Tooltip("Info text is permanent")]
        private bool _permanent;
        #endregion

        #region Private variables
        private AudioManager _audioManager;
        private float _r, _g, _b;
        private float _start, _infoStart;
        private bool _newGame;
        private bool _returnMenu;
        private float _timePassed;
        private float _rInfo, _gInfo, _bInfo;
        private float _rText, _gText, _bText;
        private List<string> _lines;
        private bool _lastTextShown;
        private float _textTime;
        private int _linePos = 0;
        private bool _allCharsPrinted;
        #endregion

        #region Awake & Update
        private void Awake()
        {
            _audioManager = GameManager.instance.audioManager;
            _newGame = false;
            _returnMenu = false;
            _lines = new List<string>();

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

            // Fade in out effect
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
            
            // Transition
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

            // Info Text
            if (GameStateController.CurrentState.SceneName != "MainMenu")
            {
                if (GameManager.instance.infoFadeInStart && _fadeScreen.color.a == 0)
                {
                    _infoStart = Time.time;
                    GameManager.instance.infoFadeInStart = false;

                    if (GameManager.instance.infoFadeIn)
                    {
                        _textBg.color = new Vector4(_rInfo, _gInfo, _bInfo, 0);
                        GameManager.instance.infoBoxVisible = true;
                        CheckString();
                    }
                    else
                        _textBg.color = new Vector4(_rInfo, _gInfo, _bInfo, 1);
                }

                if (_textBg != null)
                {
                    if (GameManager.instance.infoBoxVisible &&
                        _textBg.color.a == 1 && _lastTextShown && _allCharsPrinted && !_permanent)
                        InfoTimer();

                    if (GameManager.instance.infoBoxVisible && _textBg.color.a != 1)
                        FadeInInfo();

                    if (!GameManager.instance.infoBoxVisible && _textBg.color.a != 0)
                        FadeOutInfo();

                    if (!_lastTextShown && _allCharsPrinted)
                        UpdateText();

                    if (GameManager.instance.newText)
                    {
                        GameManager.instance.newText = false;
                        CheckString();
                    }
                }
            }
        }
        #endregion

        #region Text Handling
        private void CheckString()
        {
            Debug.Log("checking string");
            _lines.Clear();
            _lines = new List<string>();
            string line = GameManager.instance.infoText + _endchar;
            string text = "";

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != _endchar)
                {
                    text += line[i];
                }
                else
                {
                    _lines.Add(text);
                    text = "";
                }
            }

            _allCharsPrinted = false;
            StartCoroutine(PrintChar(_lines[_linePos]));            

            if ((_lines.Count - 1) > _linePos)
            {
                _lastTextShown = false;
                _linePos++;
            }
            else
            {
                _linePos = 0;
                _lastTextShown = true;
            }
        }

        private void UpdateText()
        {
            _textTime += Time.deltaTime;
            if (_textTime >= _textDelay)
            {
                _textTime = 0;
                _allCharsPrinted = false;
                StartCoroutine(PrintChar(_lines[_linePos]));

                if ((_lines.Count - 1) > _linePos)
                    _linePos++;
                else
                {
                    _linePos = 0;
                    _lastTextShown = true;
                }
                
            }
        }

        private IEnumerator PrintChar(string line)
        {
            _infoText.text = string.Empty;            

            for (int i = 0; i < line.Length; i++)
            {
                _infoText.text += line[i];
                
                if (i == line.Length - 1)
                {
                    _allCharsPrinted = true;
                }

                yield return new WaitForSeconds(_charDelay);
            }
        }      

        private void InfoTimer()
        {            
            _timePassed += Time.deltaTime;
            if (_timePassed >= _delay)
            {
                _timePassed = 0;
                GameManager.instance.infoBoxVisible = false;
                _infoStart = Time.time;
            }
            if (GameManager.instance.resetInfoTimer)
            {
                _timePassed = 0;
                _infoText.text = GameManager.instance.infoText;
                GameManager.instance.resetInfoTimer = false;
            }
        }
        #endregion

        #region Fade In/Out effect
        private void FadeInInfo()
        {
            float progress = Time.time - _infoStart;
            _infoText.color = Color.Lerp(_infoText.color, new Vector4(_rText, _gText, _bText, 1), progress / _infoDuration);
            _textBg.color = Color.Lerp(_textBg.color, new Vector4(_rInfo, _gInfo, _bInfo, 1), progress / _infoDuration);
        }

        private void FadeOutInfo()
        {
            float progress = Time.time - _infoStart;
            _infoText.color = Color.Lerp(_infoText.color, new Vector4(_rText, _gText, _bText, 0), progress / _infoDuration);
            _textBg.color = Color.Lerp(_textBg.color, new Vector4(_rInfo, _gInfo, _bInfo, 0), progress / _infoDuration);
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
        #endregion

        #region Buttons & Sliders
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
        #endregion
    }
}