using UnityEngine.UI;
using UnityEngine;
using ProjectThief.States;
using System.Collections.Generic;
using System.Collections;
using System.Text;

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
        [SerializeField, Tooltip("Navigation area")]
        private GameObject _navi;
        [SerializeField, Tooltip("Menu background")]
        private GameObject _background;
        [SerializeField, Header("Fade In/Out")]
        private RawImage _fadeScreen;
        [SerializeField, Tooltip("Fade in/ out effect duration")]
        private float _duration = 2;
        [SerializeField, Tooltip("Main credits screen"), Header("Credits")]
        private GameObject _credits;
        [SerializeField, Tooltip("Part 1 credits screen")]
        private GameObject _creditsPart1;
        [SerializeField, Tooltip("Credits part 1 button")]
        private GameObject _moveToPart2;
        [SerializeField, Tooltip("Part2 credits screen")]
        private GameObject _creditsPart2;
        [SerializeField, Tooltip("Credits part 2 button")]
        private GameObject _moveToPart1;
        [SerializeField, Tooltip("Credits fade effect duration")]
        private float _creditsDuration;
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
        [SerializeField, Tooltip("New message highlight")]
        private RawImage _highlight;
        [SerializeField, Tooltip("Highlight effect duration")]
        private float _highlightDuration = 1f;
        [SerializeField, Tooltip("How many times highlight flashes")]
        private int _times = 2;
        #endregion

        #region Private variables
        private AudioManager _audioManager;
        private float _r, _g, _b;
        private float _hR, _hG, _hB;
        private float _start, _infoStart;
        private float _timePassed;
        private float _rInfo, _gInfo, _bInfo;
        private float _rText, _gText, _bText;
        private float _cred1R, _cred1G, _cred1B;
        private float _cred2R, _cred2G, _cred2B;
        private float _credB1R, _credB1G, _credB1B;
        private float _credB2R, _credB2G, _credB2B;
        private float _textTime;
        private float _highlightTime;
        private float _creditsTime;
        private List<string> _lines;
        private List<int> _lineLength;
        private int _linePos = 0;
        private int _flashes;
        private bool _lastTextShown;
        private bool _newGame;
        private bool _returnMenu;
        private bool _allCharsPrinted;
        private bool _coroutineRunning;
        private bool _highlightStarted;
        private bool _creditPage1;
        private bool _creditsFadeStart;
        private bool _creditsFade;
        private Coroutine _runningCoroutine;
        private RawImage _page1Img;
        private RawImage _page2Img;
        private Image _button1Img;
        private Image _button2Img;
        private Button _button1;
        private Button _button2;
        #endregion

        #region Awake & Update
        private void Awake()
        {
            _audioManager = GameManager.instance.audioManager;
            _newGame = false;
            _returnMenu = false;
            _lines = new List<string>();
            _lineLength = new List<int>();

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

                _hR = _highlight.color.r;
                _hG = _highlight.color.g;
                _hB = _highlight.color.b;

                _highlight.color = new Vector4(_hR, _hG, _hB, 0);
                _infoText.color = new Vector4(_rText, _gText, _bText, 0);
                _textBg.color = new Vector4(_rInfo, _bInfo, _gInfo, 0);
                GameManager.instance.infoBoxVisible = false;
                GameManager.instance.infoFadeInStart = false;
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

                _page1Img = _creditsPart1.GetComponent<RawImage>();
                _page2Img = _creditsPart2.GetComponent<RawImage>();
                _button1Img = _moveToPart1.GetComponent<Image>();
                _button2Img = _moveToPart2.GetComponent<Image>();
                _button1 = _moveToPart1.GetComponent<Button>();
                _button2 = _moveToPart2.GetComponent<Button>();

                _cred1R = _page1Img.color.r;
                _cred2R = _page2Img.color.r;
                _cred1G = _page1Img.color.g;
                _cred2G = _page2Img.color.g;
                _cred1B = _page1Img.color.b;
                _cred2B = _page2Img.color.b;

                _credB1R = _button1Img.color.r;
                _credB2R = _button2Img.color.r;
                _credB1G = _button1Img.color.g;
                _credB2G = _button2Img.color.g;
                _credB1B = _button1Img.color.b;
                _credB2B = _button2Img.color.b;
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

                // Credits fade effects
                if (_creditsFadeStart)
                {
                    _creditsTime = Time.time;
                    _creditsFade = true;
                    _creditsFadeStart = false;
                }

                if (_creditPage1 && _creditsFade)
                {
                    if (!_creditsPart2.activeInHierarchy)                    
                        _creditsPart2.SetActive(true);

                    CreditsFade2();
                }
                else if (!_creditPage1 && _creditsFade)
                {
                    if (!_creditsPart1.activeInHierarchy)                    
                        _creditsPart1.SetActive(true);
                    

                    CreditsFade1();
                }

                if (_creditsFade)
                {
                    if (!_creditPage1 && _page1Img.color.a >= 0.6f)
                    {
                        _button1.interactable = false;
                        _button2.interactable = true;
                        _button1Img.color = new Vector4(_credB1R, _credB1G, _credB1B, 0.25f);
                        _button2Img.color = new Vector4(_credB2R, _credB2G, _credB2B, 1);

                        if (!_creditPage1 && _page1Img.color.a == 1)
                        {
                            _creditPage1 = true;
                            _creditsFade = false;
                        }

                    }
                    else if (_creditPage1 && _page2Img.color.a >= 0.6f)
                    {
                        _button1.interactable = true;
                        _button2.interactable = false;
                        _button1Img.color = new Vector4(_credB1R, _credB1G, _credB1B, 1);
                        _button2Img.color = new Vector4(_credB2R, _credB2G, _credB2B, 0.25f);

                        if (_creditPage1 && _page2Img.color.a == 1)
                        {
                            _creditPage1 = false;
                            _creditsFade = false;
                        }
                    }
                }
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
                if (_newGame)
                    AudioManager.instance.PlayTrack(1);

                _newGame = false;
                GameManager.instance.previousState = GameStateController.CurrentState;
                GameStateController.PerformTransition(_nextState);
            }
            if (_returnMenu && _fadeScreen.color.a == 1)
            {
                _returnMenu = false;
                GameStateController.PerformTransition(_menuState);
                AudioManager.instance.PlayTrack(0);
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
                        _highlightTime = Time.time;
                        _highlight.color = new Vector4(_hR, _hG, _hB, 1);
                        _flashes = 0;
                        _highlightStarted = true;

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

                        _highlightTime = Time.time;
                        _highlight.color = new Vector4(_hR, _hG, _hB, 1);
                        _flashes = 0;
                        _highlightStarted = true;

                        CheckString();
                    }

                    if (_highlightStarted)
                    {
                        HighlightFadeOut();

                        if (_highlight.color.a == 0 && _flashes < _times)
                        {
                            _flashes++;
                            if (_flashes < _times)
                            {
                                _highlightTime = Time.time;
                                _highlight.color = new Vector4(_hR, _hG, _hB, 1);
                            }
                        }

                        if (_highlight.color.a == 0 && _flashes == _times)
                        {
                            _highlightStarted = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region Text Handling
        private void CheckString()
        {
            _lines.Clear();
            _lineLength.Clear();
            _lines = new List<string>();
            _lineLength = new List<int>();
            _linePos = 0;
            int charCount = 0;

            string line = GameManager.instance.infoText + _endchar;
            string text = "";

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != _endchar)
                {
                    text += line[i];
                    charCount++;
                }
                else
                {
                    _lines.Add(text);
                    text = "";
                    _lineLength.Add(charCount);
                    charCount = 0;
                }
            }

            if (_coroutineRunning)
            {
                StopCoroutine(_runningCoroutine);
                _coroutineRunning = false;
                _infoText.text = "";
            }

            _allCharsPrinted = false;
            EmptyString(_lineLength[_linePos]);
            _runningCoroutine = StartCoroutine(PrintChar(_lines[_linePos]));            

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

        private void EmptyString(int charCount)
        {
            string emptyString = "";

            for (int i = 0; i < charCount; i++)
            {
                emptyString += " ";
            }

            _infoText.text = emptyString;
        }

        private void UpdateText()
        {            
            _textTime += Time.deltaTime;
            if (_textTime >= _textDelay)
            {
                _textTime = 0;
                _allCharsPrinted = false;
                EmptyString(_lineLength[_linePos]);
                _runningCoroutine = StartCoroutine(PrintChar(_lines[_linePos]));

                if ((_lines.Count - 1) > _linePos)
                    _linePos++;
                else
                {
                    _linePos = 0;
                    _lastTextShown = true;
                }                
            }
            if (!_coroutineRunning)
                _textTime = 0;
        }

        private IEnumerator PrintChar(string line)
        {
            _coroutineRunning = true;
            StringBuilder displayText = new StringBuilder(_infoText.text);

            for (int i = 0; i < line.Length; i++)
            {
                displayText[i] = line[i];
                _infoText.text = displayText.ToString();

                if (i == line.Length - 1)
                {
                    _allCharsPrinted = true;
                    yield break;
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

        private void HighlightFadeOut()
        {
            float progress = Time.time - _highlightTime;
            _highlight.color = Color.Lerp(_highlight.color, new Vector4(_hR, _hG, _hB, 0), progress / _highlightDuration);
        }

        private void CreditsFade1()
        {
            float progress = Time.time - _creditsTime;
            _page1Img.color = Color.Lerp(_page1Img.color, new Vector4(_cred1R, _cred1G, _cred1B, 1), progress / _creditsDuration);
            _page2Img.color = Color.Lerp(_page2Img.color, new Vector4(_cred2R, _cred2G, _cred2B, 0), progress / _creditsDuration);
        }

        private void CreditsFade2()
        {
            float progress = Time.time - _creditsTime;
            _page1Img.color = Color.Lerp(_page1Img.color, new Vector4(_cred1R, _cred1G, _cred1B, 0), progress / _creditsDuration);
            _page2Img.color = Color.Lerp(_page2Img.color, new Vector4(_cred2R, _cred2G, _cred2B, 1), progress / _creditsDuration);
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
            GameManager.instance.canContinue = false;
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

        public void Credits()
        {
            _navi.SetActive(false);
            _background.SetActive(false);
            _credits.SetActive(true);
            _page1Img.color = new Vector4(_cred1R, _cred1G, _cred1B, 1);
            _button1.interactable = false;
            _button2.interactable = true;
            _button2Img.color = new Vector4(_credB2R, _credB2G, _credB2B, 1);
            _button1Img.color = new Vector4(_credB1R, _credB1G, _credB1B, 0.25f);
            _creditsPart2.SetActive(false);
            _creditPage1 = true;            
        }

        public void CreditsBack()
        {
            _page1Img.color = new Vector4(_cred1R, _cred1G, _cred1B, 0);
            _page2Img.color = new Vector4(_cred2R, _cred2G, _cred2B, 0);
            _button1Img.color = new Vector4(_credB1R, _credB1G, _credB1B, 0);
            _button2Img.color = new Vector4(_credB2R, _credB2G, _credB2B, 0);
            _button1.interactable = false;
            _button2.interactable = false;
            _creditsFadeStart = false;
            _creditsFade = false;
            _credits.SetActive(false);            
            _background.SetActive(true);
            _navi.SetActive(true);
        }

        public void MoveToCreditsPart2()
        {
            _creditsFadeStart = true;
        }

        public void MoveToCreditsPart1()
        {
            _creditsFadeStart = true;
        }
        #endregion
    }
}