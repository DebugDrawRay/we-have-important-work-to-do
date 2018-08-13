using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;
using DG.Tweening;
namespace FSS
{
    public class GameManager : MonoBehaviour
    {
        [Header("Ram")]
        [SerializeField] private GameObject[] m_ramPips;
        [SerializeField] private SuperTextMesh m_ramText;
        private float m_currentRam;
        private float m_additionalRam;
        private float TotalRam
        {
            get
            {
                return m_currentRam + m_additionalRam;
            }
        }
        private float m_currentUseage
        {
            get
            {
                int validWindowCount = 0;
                foreach(WindowController w in m_currentWindows)
                {
                    if(!w.programWindow)
                    {
                        validWindowCount++;
                    }
                }
                return (Settings.WindowCost * validWindowCount) + ProgramManager.instance.TotalRamUse;
            }
        }

        [Header("Popups")]
        [SerializeField]private GameObject m_window;
        [SerializeField]private RectTransform m_windowContainer;
        private float m_popUpInterval;
        private float m_lastWindowTime;
        private List<WindowController> m_currentWindows = new List<WindowController>();

        [Header("Currency")]
        [SerializeField] private SuperTextMesh m_currencyText;
        public int CurrentCurrency
        {
            get
            {
                return m_currentCurrency;
            }
        }
        private int m_currentCurrency;
        
        [Header("Time")]
        [SerializeField] private SuperTextMesh m_clock;
        private DateTime m_currentClockTime;
        private float m_gameStartTime;
        private float m_lastClockUpdate;
        private float m_totalMinutes;
        private float m_gameLength;

        [Header("Management")]
        [SerializeField] private GameObject m_managementGroup;
        [SerializeField] private GameObject m_loginScreen;
        [SerializeField] private float m_stateDelay = 3f;
        [SerializeField] private float m_gameStartDelay = 5f;

        [Header("Sequence")]
        [SerializeField] private GameObject m_sequenceScreen;
        [SerializeField] private TypewriterEffect m_welcomePrompt;
        [SerializeField] private TypewriterEffect m_selectPrompt;
        [SerializeField] private TypewriterEffect m_cursorPrompt;
        [SerializeField] private GameObject m_easySelect;
        [SerializeField] private GameObject m_hardSelect;
        [SerializeField] private GameObject m_endlessSelect;

        [Header("Boot")]
        [SerializeField] private GameObject m_bootGroup;
        [SerializeField] private GameObject m_bootGraphics;
        [SerializeField] private GameObject m_bootLogo;
        [SerializeField] private GameObject m_bootInformation;
        [SerializeField] private GameObject m_bootWarning;

        [Header("Game Complete")]
        [SerializeField] private GameObject m_blueScreen;
        [SerializeField] private GameObject m_shutdown;
        [SerializeField] private CanvasGroup m_interfaceCanvas;
        [SerializeField] private DigitalGlitch m_glitch;
        [SerializeField] private float m_glitchSpeed;

        [Header("Ads")]
        [SerializeField] private AdDatabase m_adb;

        public bool GameActive
        {
            get
            {
                return m_currentState == GameState.InGame && m_gameStarted;
            }
        }
        private bool m_gameStarted;
        private bool m_endless;
        private string m_playerName;
        private GameState m_currentState;

        public enum GameState
        {
            Boot,
            Login,
            Sequence,
            InGame,
            Complete
        }
        public delegate void WindowCallback(WindowController windows);
        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            m_managementGroup.SetActive(false);
            m_loginScreen.SetActive(false);
            m_sequenceScreen.SetActive(false);

            m_bootGroup.SetActive(false);
            m_bootGraphics.SetActive(false);
            m_bootLogo.SetActive(false);
            m_bootInformation.SetActive(false);
            m_bootWarning.SetActive(false);

            m_welcomePrompt.Revert(false);
            m_selectPrompt.Revert(false);
            m_cursorPrompt.Revert(false);
            m_easySelect.SetActive(false);
            m_hardSelect.SetActive(false);
            m_endlessSelect.SetActive(false);

            m_blueScreen.SetActive(false);
            m_shutdown.SetActive(false);
            m_glitch.intensity = 0;

            m_interfaceCanvas.interactable = true;
            m_interfaceCanvas.blocksRaycasts = true;

            m_additionalRam = 0;
            m_currentCurrency = 0;

            m_currentState = GameState.Boot;
            m_gameStarted = false;
        }

        private void Start()
        {
            EnterState();
        }

        private void Update()
        {
            if(m_currentState == GameState.InGame && m_gameStarted)
            {
                UpdatePopUps();
                UpdateRam();
                UpdateTime();
            }
            if(m_blueScreen.activeSelf && Input.anyKey)
            {
                RestartGame();
            }
        }

        public void SetupGame(int mode)
        {
            m_currentClockTime = Settings.StartClockTime;
            switch (mode)
            {
                case 0:
                    m_totalMinutes = (Settings.EasyClockEnd.ToUniversalTime().Hour - Settings.StartClockTime.ToUniversalTime().Hour) * 60;
                    m_gameLength = Settings.EasyModeTime;
                    break;
                case 1:
                    m_totalMinutes = (Settings.HardClockEnd.ToUniversalTime().Hour - Settings.StartClockTime.ToUniversalTime().Hour) * 60;
                    m_gameLength = Settings.HardModeTime;
                    break;
                case 2:
                    m_currentClockTime = DateTime.Now;
                    m_endless = true;
                    break;
            }
            m_clock.text = m_currentClockTime.ToShortTimeString();
            m_currentRam = Settings.StartRam;
            UpdateRam();
            m_currencyText.text = "0";
            m_popUpInterval = Settings.StartPopupInterval;

            TransitionToNextState(3);
        }
        public void StartGame()
        {
            m_gameStartTime = Time.time;
            m_gameStarted = true;
        }
        public void GameComplete(bool victory)
        {
            m_interfaceCanvas.interactable = false;
            m_interfaceCanvas.blocksRaycasts = false;
            m_currentState = GameState.Complete;
            if (victory)
            {
                m_shutdown.SetActive(true);
            }
            else
            {
                GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                GetComponent<Canvas>().worldCamera = Camera.main;
                Tween glitch = DOTween.To(() => m_glitch.intensity, x => m_glitch.intensity = x, .5f, m_glitchSpeed);
                glitch.OnComplete(DisplayBlueScreen);
            }
        }
        public void DisplayBlueScreen()
        {
            m_blueScreen.SetActive(true);
            GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            m_glitch.intensity = 0;
        }

        private void UpdateRam()
        {
            float progress = (m_currentUseage / TotalRam) * m_ramPips.Length;

            for(int i = 0; i < m_ramPips.Length; i++)
            {
                m_ramPips[i].SetActive(i + 1 <= progress);
            }

            m_ramText.text = m_currentUseage.ToString() + " / " + TotalRam + Settings.RamUnit;
            if(m_currentUseage > m_currentRam)
            {
                GameComplete(false);
            }
        }
        private void UpdatePopUps()
        {
            float interval = (m_popUpInterval / PenaltyController.instance.SpeedMulti) * ProgramManager.instance.SpeedMulti;
            if (m_lastWindowTime + interval  <= Time.time)
            {
                AddPopUp();
                m_lastWindowTime = Time.time;
            }
        }

        private void AddPopUp()
        {
            WindowController window = Instantiate(m_window, m_windowContainer).GetComponent<WindowController>();
            AdData data = m_adb.RequestRandom();
            window.Initialize(data, CloseWindow);
            if(ProgramManager.instance.ConsolidateActive)
            {
                RectTransform winRect = window.GetComponent<RectTransform>();
                winRect.anchoredPosition = Settings.ConsolodatePosition;
                if (ProgramManager.instance.PredictionActive)
                {
                    ProgramManager.instance.Predict();
                }
            }
            else
            {
                if (ProgramManager.instance.PredictionActive)
                {
                    Vector2 pos = ProgramManager.instance.PredictionPos;
                    RectTransform winRect = window.GetComponent<RectTransform>();
                    if (pos.y + (winRect.rect.height / 2) > Screen.height / 2)
                    {
                        pos.y = (Screen.height / 2) - (winRect.rect.height / 2);
                    }
                    winRect.anchoredPosition = pos;
                    ProgramManager.instance.Predict();
                }
                else
                {
                    Vector2 pos = UnityEngine.Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height) / 2);
                    RectTransform winRect = window.GetComponent<RectTransform>();
                    if (pos.y + (winRect.rect.height / 2) > Screen.height / 2)
                    {
                        pos.y = (Screen.height / 2) - (winRect.rect.height / 2);
                    }
                    winRect.anchoredPosition = pos;
                }
            }
            m_currentWindows.Add(window);
        }
        public void AddWindow(GameObject windowGo, AdData.Function func)
        {
            WindowController window = Instantiate(windowGo, m_windowContainer).GetComponent<WindowController>();
            window.Initialize(func, CloseWindow);
            Vector2 pos = UnityEngine.Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height) / 2);
            RectTransform winRect = window.GetComponent<RectTransform>();
            if (pos.y + (winRect.rect.height / 2) > Screen.height / 2)
            {
                pos.y = (Screen.height / 2) - (winRect.rect.height / 2);
            }
            winRect.anchoredPosition = pos;
            m_currentWindows.Add(window);
        }
        public bool CheckDuplicate(GameObject window)
        {
            foreach(WindowController w in m_currentWindows)
            {
                if(w.gameObject.CompareTag(window.tag))
                {
                    return true;
                }
            }
            return false;
        }
        public void CloseWindow(String tag)
        {
            foreach (WindowController w in m_currentWindows)
            {
                if (!w.CompareTag(tag))
                {
                    w.Close();
                    return;
                }
            }
        }
        public void CloseWindow(WindowController window)
        {
            if(m_currentWindows.Contains(window))
            {
                if(!window.programWindow)
                {
                    AddCurrency(Settings.CurrencyOnClose);
                }
                m_currentWindows.Remove(window);

            }
        }

        public void AddCurrency(int amount)
        {
            m_currentCurrency += amount;
            m_currencyText.text = m_currentCurrency.ToString();
        }

        public void AddRam(float amount)
        {
            m_additionalRam += amount;
        }


        private void UpdateTime()
        {
            float elapsed = Time.time - m_gameStartTime;
            if (m_endless)
            {
                m_clock.text = DateTime.Now.ToShortTimeString();
            }
            else
            {
                if (elapsed < Settings.EasyModeTime)
                {
                    float progress = (elapsed / m_gameLength) * m_totalMinutes;
                    m_currentClockTime = Settings.StartClockTime.AddMinutes(Mathf.RoundToInt(progress));
                    m_clock.text = m_currentClockTime.ToShortTimeString();
                }
                else
                {
                    GameComplete(true);
                }
            }
        }

        public void UpdateName(string name)
        {
            m_playerName = name;
            m_welcomePrompt.m_textToWrite = "> Good morning " + name +".";
        }
        private int m_maxIterations = 10;
        public void CloseRandom(int i = 0)
        {
            i++;
            int ran = UnityEngine.Random.Range(0, m_currentWindows.Count);
            if (!m_currentWindows[ran].programWindow)
            {
                m_currentWindows[ran].Close();
            }
            else
            {
                if (i < 10)
                {
                    CloseRandom(i);
                }
            }
        }
        public void TransitionToNextState(int state)
        {
            ExitState();
            m_currentState = (GameState)state;
            StartCoroutine(StateDelay(m_stateDelay));
        }
        public void ExitState()
        {
            switch (m_currentState)
            {
                case GameState.Boot:
                    m_bootGroup.SetActive(false);
                    break;
                case GameState.Login:
                    m_loginScreen.SetActive(false);
                    break;
                case GameState.Sequence:
                    m_sequenceScreen.SetActive(false);
                    break;
                case GameState.InGame:
                    break;
                case GameState.Complete:
                    break;
            }
        }
        public IEnumerator StateDelay(float time)
        {
            yield return new WaitForSeconds(time);
            EnterState();
        }
        public IEnumerator GameStartDelay(float time)
        {
            yield return new WaitForSeconds(time);
            StartGame();
        }
        public IEnumerator BootSequence()
        {
            m_bootGroup.SetActive(true);
            m_bootGraphics.SetActive(true);
            yield return new WaitForSeconds(m_stateDelay);
            m_bootLogo.SetActive(true);
            yield return new WaitForSeconds(m_stateDelay);
            m_bootInformation.SetActive(true);
            m_bootWarning.SetActive(true);
            yield return new WaitForSeconds(m_stateDelay);
            m_bootGraphics.SetActive(false);
            yield return new WaitForSeconds(m_stateDelay);
            TransitionToNextState(1);
        }
        public void EnterState()
        {
            switch(m_currentState)
            {
                case GameState.Boot:
                    m_managementGroup.SetActive(true);
                    StartCoroutine(BootSequence());
                    break;
                case GameState.Login:
                    m_loginScreen.SetActive(true);
                    break;
                case GameState.Sequence:
                    m_sequenceScreen.SetActive(true);
                    m_welcomePrompt.Write();
                    break;
                case GameState.InGame:
                    m_managementGroup.SetActive(false);
                    StartCoroutine(GameStartDelay(m_gameStartDelay));
                    break;
                case GameState.Complete:
                    break;
            }
        }

        public void RestartGame()
        {
            DestroyAllWindows();
            InterfaceManager.instance.ToggleStartMenu(false);
            GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            Initialize();
            PenaltyController.instance.Reinitialize();
            ProgramManager.instance.Initialize();
            EnterState();
        }
        public void DestroyAllWindows()
        {
            for (int i =  m_currentWindows.Count - 1; i >= 0; i--)
            {
                if (m_currentWindows[i] != null)
                {
                    m_currentWindows[i].Close();
                }
            }
            m_currentWindows = new List<WindowController>();
        }
    }
}