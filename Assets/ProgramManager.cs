using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSS
{
    public class ProgramManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_antiAdMenu;
        [SerializeField] private GameObject m_antiAdWindow;

        [SerializeField] private GameObject m_resizeMenu;
        [SerializeField] private GameObject m_resizeWindow;

        [SerializeField] private GameObject m_predictionMenu;
        [SerializeField] private GameObject m_predictionWindow;

        [SerializeField] private GameObject m_consolidatorMenu;
        [SerializeField] private GameObject m_consolidatorWindow;

        [SerializeField] private GameObject m_timeMenu;
        [SerializeField] private GameObject m_timeWindow;

        [SerializeField] private GameObject m_purchaseWindow;

        [SerializeField] private GameObject m_predictionIcon;
        [SerializeField] private RectTransform m_elementContainer;
        private RectTransform m_currentPrediction;

        private bool m_antiAdActive;
        private bool m_resizeActive;
        private bool m_predictionActive;
        private bool m_consolidateActive;
        private bool m_timeActive;

        private float m_lastAd;
        private float m_predictionStart;
        private float m_timeStart;

        private GameManager m_manager;
        public static ProgramManager instance;

        public bool ConsolidateActive
        {
            get
            {
                return m_consolidateActive;
            }
        }
        public bool ResizeActive
        {
            get
            {
                return m_resizeActive;
            }
        }
        public bool PredictionActive
        {
            get
            {
                return m_predictionActive;
            }
        }
        public Vector2 PredictionPos
        {
            get
            {
                return m_currentPrediction.anchoredPosition;
            }
        }

        public float TotalRamUse
        {
            get
            {
                float total = 0;
                total += m_antiAdActive ? Settings.AntiAdRamCost : 0;
                total += m_resizeActive ? Settings.ResizeRamCost : 0;
                total += m_predictionActive ? Settings.PredictionRamCost : 0;
                total += m_consolidateActive ? Settings.ConsolidatorRamCost : 0;
                total += m_timeActive ? Settings.TimeRamCost : 0;
                return total;
            }
        }

        public float SpeedMulti
        {
            get
            {
                return m_timeActive ? Settings.TimeSpeedMulti : 1;
            }
        }
        private void Awake()
        {
            m_manager = GetComponent<GameManager>();
            instance = this;
            m_currentPrediction = Instantiate(m_predictionIcon, m_elementContainer).GetComponent<RectTransform>();
            Initialize();
        }
        public void Initialize()
        {
            m_currentPrediction.gameObject.SetActive(false);
            m_antiAdMenu.SetActive(false);
            m_resizeMenu.SetActive(false);
            m_predictionMenu.SetActive(false);
            m_consolidatorMenu.SetActive(false);
            m_timeMenu.SetActive(false);
            m_antiAdActive = false;
            m_resizeActive = false;
            m_predictionActive = false;
            m_consolidateActive = false;
            m_timeActive = false;

            m_lastAd = Time.time;
        }
        private void Update()
        {
            if(m_manager.GameActive)
            {
                UpdatePrograms();
            }
        }
        private void UpdatePrograms()
        {
            if (m_antiAdActive)
            {
                if(m_lastAd + Settings.AntiAdCloseInterval < Time.time)
                {
                    m_manager.CloseRandom();
                    m_lastAd = Time.time;
                }
            }
            if (m_predictionActive)
            {
                if (m_predictionStart + Settings.TimeLength < Time.time)
                {
                    m_predictionActive = false;
                    m_currentPrediction.gameObject.SetActive(false);
                    m_predictionMenu.SetActive(false);
                    m_manager.CloseWindow("pre");
                }
            }
            if (m_timeActive)
            {
                if(m_timeStart + Settings.TimeLength < Time.time)
                {
                    m_timeActive = false;
                    m_timeMenu.SetActive(false);
                }
            }
        }

        public void Launch(int program)
        {
            switch(program)
            {
                case 0:
                    if (!m_manager.CheckDuplicate(m_antiAdWindow))
                    {
                        m_manager.AddWindow(m_antiAdWindow, AdData.Function.AntiAd);
                    }
                    break;
                case 1:
                    if (!m_manager.CheckDuplicate(m_resizeWindow))
                    {
                        m_manager.AddWindow(m_resizeWindow, AdData.Function.Resize);
                    }
                    break;
                case 2:
                    if (!m_manager.CheckDuplicate(m_predictionWindow))
                    {
                        m_manager.AddWindow(m_predictionWindow, AdData.Function.Prediction);
                    }
                    break;
                case 3:
                    if (!m_manager.CheckDuplicate(m_consolidatorWindow))
                    {
                        m_manager.AddWindow(m_consolidatorWindow, AdData.Function.Consolidate);
                    }
                    break;
                case 4:
                    if (!m_manager.CheckDuplicate(m_timeWindow))
                    {
                        m_manager.AddWindow(m_timeWindow, AdData.Function.Time);
                    }
                    break;
                case 5:
                    if (!m_manager.CheckDuplicate(m_purchaseWindow))
                    {
                        m_manager.AddWindow(m_purchaseWindow, AdData.Function.None);
                    }
                    break;
            }
        }

        public void Purchase(int program)
        {
            switch (program)
            {
                case 0:
                    if(Settings.AntiAdCost <= m_manager.CurrentCurrency && !m_antiAdMenu.activeSelf)
                    {
                        m_manager.AddCurrency(-Settings.AntiAdCost);
                        m_antiAdMenu.SetActive(true);
                    }
                    break;
                case 1:
                    if (Settings.ResizeCost <= m_manager.CurrentCurrency && !m_resizeMenu.activeSelf)
                    {
                        m_manager.AddCurrency(-Settings.ResizeCost);
                        m_resizeMenu.SetActive(true);
                    }
                    break;
                case 2:
                    if (Settings.PredictionCost <= m_manager.CurrentCurrency && !m_predictionMenu.activeSelf)
                    {
                        m_manager.AddCurrency(-Settings.PredictionCost);
                        m_predictionMenu.SetActive(true);
                    }
                    break;
                case 3:
                    if (Settings.ConsolidatorCost <= m_manager.CurrentCurrency && !m_consolidatorMenu.activeSelf)
                    {
                        m_manager.AddCurrency(-Settings.ConsolidatorCost);
                        m_consolidatorMenu.SetActive(true);
                    }
                    break;
                case 4:
                    if (Settings.TimeCost <= m_manager.CurrentCurrency && !m_timeMenu.activeSelf)
                    {
                        m_manager.AddCurrency(-Settings.TimeCost);
                        m_timeMenu.SetActive(true);
                    }
                    break;
                case 5:
                    if (Settings.RamCost <= m_manager.CurrentCurrency)
                    {
                        m_manager.AddCurrency(-Settings.TimeCost);
                        Trigger(AdData.Function.Ram);
                    }
                    break;
            }
        }

        public void Trigger(AdData.Function func, bool active = false)
        {
            switch (func)
            {
                case AdData.Function.AntiAd:
                    m_antiAdActive = active;
                    break;
                case AdData.Function.Resize:
                    m_resizeActive = active;
                    break;
                case AdData.Function.Ram:
                    m_manager.AddRam(Settings.PurchasableRam);
                    break;
                case AdData.Function.Prediction:
                    if (active)
                    {
                        m_predictionActive = true;
                        m_currentPrediction.gameObject.SetActive(true);
                        m_predictionStart = Time.time;
                    }
                    break;
                case AdData.Function.Consolidate:
                    m_consolidateActive = active;
                    break;
                case AdData.Function.Time:
                    if (active)
                    {
                        m_timeActive = true;
                        m_timeStart = Time.time;
                    }
                    break;
            }
        }
        public void Predict()
        {
            m_currentPrediction.anchoredPosition = Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height) / 2);
        }
    }
}
