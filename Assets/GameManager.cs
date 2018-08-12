using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSS
{
    public class GameManager : MonoBehaviour
    {
        [Header("Ram")]
        [SerializeField] private GameObject[] m_ramPips;
        [SerializeField] private SuperTextMesh m_ramText;
        private float m_currentRam;
        private float m_currentUseage
        {
            get
            {
                return Settings.WindowCost * m_currentWindows.Count;
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
        private int m_currentCurrency;
        
        [Header("Time")]
        [SerializeField] private SuperTextMesh m_clock;
        private DateTime m_currentClockTime;
        private float m_gameStartTime;
        private float m_lastClockUpdate;
        private float m_totalMinutes;
        private float m_gameLength;

        [Header("Management")]
        [SerializeField] private TypewriterEffect m_welcomePrompt;
        [SerializeField] private GameObject m_managementGroup;
        [SerializeField] private GameObject m_loginScreen;
        [SerializeField] private GameObject m_sequenceScreen;
        [SerializeField] private float m_stateDelay = 3f;
        [SerializeField] private float m_gameStartDelay = 5f;

        [Header("Boot")]
        [SerializeField] private GameObject m_bootGroup;
        [SerializeField] private GameObject m_bootGraphics;
        [SerializeField] private GameObject m_bootLogo;
        [SerializeField] private GameObject m_bootInformation;
        [SerializeField] private GameObject m_bootWarning;

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
            m_managementGroup.SetActive(false);
            m_loginScreen.SetActive(false);
            m_sequenceScreen.SetActive(false);
            m_bootGroup.SetActive(false);
            m_bootGraphics.SetActive(false);
            m_bootLogo.SetActive(false);
            m_bootInformation.SetActive(false);
            m_bootWarning.SetActive(false);
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

        private void UpdateRam()
        {
            float progress = (m_currentUseage / m_currentRam) * m_ramPips.Length;

            for(int i = 0; i < m_ramPips.Length; i++)
            {
                m_ramPips[i].SetActive(i + 1 <= progress);
            }

            m_ramText.text = m_currentUseage.ToString() + " / " + m_currentRam + Settings.RamUnit;
        }
        private void UpdatePopUps()
        {
            if(m_lastWindowTime + m_popUpInterval <= Time.time)
            {
                AddPopUp();
                m_lastWindowTime = Time.time;
            }
        }

        private void AddPopUp()
        {
            WindowController window = Instantiate(m_window, m_windowContainer).GetComponent<WindowController>();
            Vector2 pos = UnityEngine.Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height) / 2);
            RectTransform winRect = window.GetComponent<RectTransform>();
            if (pos.y + (winRect.rect.height / 2) > Screen.height / 2)
            {
                pos.y = (Screen.height/2) - (winRect.rect.height / 2);
            }
            winRect.anchoredPosition = pos;
            m_currentWindows.Add(window);
            window.Initialize(null, "Test Window", null, CloseWindow);
        }
        public void CloseWindow(WindowController window)
        {
            if(m_currentWindows.Contains(window))
            {
                m_currentWindows.Remove(window);
                AddCurrency(Settings.CurrencyOnClose);
            }
        }

        public void AddCurrency(int amount)
        {
            m_currentCurrency += amount;
            m_currencyText.text = m_currentCurrency.ToString();
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
            }
        }

        public void UpdateName(string name)
        {
            m_playerName = name;
            m_welcomePrompt.m_textToWrite = "> Good morning " + name +".";
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
    }
}