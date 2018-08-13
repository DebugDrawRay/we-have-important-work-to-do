using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FSS
{
    public class ProgramManager : MonoBehaviour
    {
        [SerializeField] private Button m_antiAdMenu;
        [SerializeField] private GameObject m_antiAdWindow;
        private bool m_antiAdPurchased;
        public bool AntiAdAvailable
        {
            get
            {
                return !m_antiAdPurchased;
            }
        }
        private float m_antiAdStart;
        private float m_antiAdDelayStart;
        private bool m_antiAdActive;
        private bool m_antiAdInDelay;

        [SerializeField] private Button m_resizeMenu;
        [SerializeField] private GameObject m_resizeWindow;
        private bool m_resizePurchased;
        public bool ResizeAvailable
        {
            get
            {
                return !m_resizePurchased;
            }
        }
        private float m_resizeStart;
        private float m_resizeDelayStart;
        private bool m_resizeActive;
        private bool m_resizeInDelay;

        [SerializeField] private Button m_predictionMenu;
        [SerializeField] private GameObject m_predictionWindow;
        private bool m_predictionPurchased;
        public bool PredictionAvailable
        {
            get
            {
                return !m_predictionPurchased;
            }
        }
        private float m_predictionStart;
        private float m_predictionDelayStart;
        private bool m_predictionActive;
        private bool m_predictionInDelay;

        [SerializeField] private Button m_consolidatorMenu;
        [SerializeField] private GameObject m_consolidatorWindow;
        private bool m_consolidatorPurchased;
        public bool ConsolidatorAvailable
        {
            get
            {
                return !m_consolidatorPurchased;
            }
        }
        private float m_consolidatorStart;
        private float m_consolidatorDelayStart;
        private bool m_consolidateActive;
        private bool m_consolidateInDelay;

        [SerializeField] private Button m_timeMenu;
        [SerializeField] private GameObject m_timeWindow;
        private bool m_timePurchased;
        public bool TimeAvailable
        {
            get
            {
                return !m_timePurchased;
            }
        }
        private float m_timeStart;
        private float m_timeDelayStart;
        private bool m_timeActive;
        private bool m_timeInDelay;

        [SerializeField] private GameObject m_purchaseWindow;

        [SerializeField] private GameObject m_predictionIcon;
        [SerializeField] private RectTransform m_elementContainer;
        [SerializeField] private GameObject m_crashContainer;
        [SerializeField] private SuperTextMesh m_crashText;
        private Coroutine m_crashCr;

        private RectTransform m_currentPrediction;

        private float m_lastAd;

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

            m_antiAdPurchased = false;
            m_resizePurchased = false;
            m_predictionPurchased = false;
            m_consolidatorPurchased = false;
            m_timePurchased = false;

            m_antiAdActive = false;
            m_resizeActive = false;
            m_predictionActive = false;
            m_consolidateActive = false;
            m_timeActive = false;

            m_antiAdInDelay = false;
            m_resizeInDelay = false;
            m_predictionInDelay = false;
            m_consolidateInDelay = false;
            m_timeInDelay = false;

            m_antiAdMenu.gameObject.SetActive(false);
            m_consolidatorMenu.gameObject.SetActive(false);
            m_resizeMenu.gameObject.SetActive(false);
            m_predictionMenu.gameObject.SetActive(false);
            m_timeMenu.gameObject.SetActive(false);

            m_crashContainer.SetActive(false);
        }
        private void Update()
        {
            if (m_manager.GameActive)
            {
                UpdatePrograms();
                UpdateDelay();
            }
            m_antiAdMenu.interactable = m_antiAdPurchased && !m_antiAdInDelay && !m_antiAdActive;
            m_consolidatorMenu.interactable = m_consolidatorPurchased && !m_consolidateInDelay && !m_consolidateActive;
            m_resizeMenu.interactable = m_resizePurchased && !m_resizeInDelay && !m_resizeActive;
            m_predictionMenu.interactable = m_predictionPurchased && !m_predictionInDelay && !m_predictionActive;
            m_timeMenu.interactable = m_timePurchased && !m_timeInDelay && !m_timeActive;

            m_antiAdMenu.gameObject.SetActive(m_antiAdPurchased);
            m_consolidatorMenu.gameObject.SetActive(m_consolidatorPurchased);
            m_resizeMenu.gameObject.SetActive(m_resizePurchased);
            m_predictionMenu.gameObject.SetActive(m_predictionPurchased);
            m_timeMenu.gameObject.SetActive(m_timePurchased);
        }
        private void UpdatePrograms()
        {
            if (m_antiAdActive)
            {
                if (m_lastAd + Settings.AntiAdCloseInterval * m_manager.CurrentPopUpInterval < Time.time)
                {
                    m_manager.CloseRandom();
                    m_lastAd = Time.time;
                }
                if (m_antiAdStart + Settings.AntiAdLength < Time.time)
                {
                    m_antiAdActive = false;
                    m_antiAdMenu.interactable = false;
                    m_manager.CloseWindow("adblock");
                    m_antiAdDelayStart = Time.time;
                    m_antiAdInDelay = true;
                    if (m_crashCr != null)
                    {
                        StopCoroutine(m_crashCr);
                    }
                    m_crashCr = StartCoroutine(CrashEvent(m_antiAdWindow.name));
                }
            }
            if (m_consolidateActive)
            {
                if (m_consolidatorStart + Settings.ConsolidatorLength < Time.time)
                {
                    m_consolidateActive = false;
                    m_consolidatorMenu.interactable = false;
                    m_manager.CloseWindow("consolidate");
                    m_consolidatorDelayStart = Time.time;
                    m_consolidateInDelay = true;
                    if (m_crashCr != null)
                    {
                        StopCoroutine(m_crashCr);
                    }
                    m_crashCr = StartCoroutine(CrashEvent(m_consolidatorWindow.name));
                }
            }
            if (m_resizeActive)
            {
                if (m_resizeStart + Settings.ResizeLength < Time.time)
                {
                    m_resizeActive = false;
                    m_resizeMenu.interactable = false;
                    m_manager.CloseWindow("resize");
                    m_resizeDelayStart = Time.time;
                    m_resizeInDelay = true;
                    if (m_crashCr != null)
                    {
                        StopCoroutine(m_crashCr);
                    }
                    m_crashCr = StartCoroutine(CrashEvent(m_resizeWindow.name));
                }
            }
            if (m_predictionActive)
            {
                if (m_predictionStart + Settings.PredictionLength < Time.time)
                {
                    m_predictionActive = false;
                    m_currentPrediction.gameObject.SetActive(false);
                    m_predictionMenu.interactable = false;
                    m_manager.CloseWindow("pre");
                    m_predictionDelayStart = Time.time;
                    m_predictionInDelay = true;
                    if (m_crashCr != null)
                    {
                        StopCoroutine(m_crashCr);
                    }
                    m_crashCr = StartCoroutine(CrashEvent(m_predictionWindow.name));
                }
            }
            if (m_timeActive)
            {
                if(m_timeStart + Settings.TimeLength < Time.time)
                {
                    m_timeActive = false;
                    m_timeMenu.interactable = false;
                    m_manager.CloseWindow("time");
                    m_timeDelayStart = Time.time;
                    m_timeInDelay = true;
                    if (m_crashCr != null)
                    {
                        StopCoroutine(m_crashCr);
                    }
                    m_crashCr = StartCoroutine(CrashEvent(m_timeWindow.name));
                }
            }
        }
        private void UpdateDelay()
        {
            if (m_antiAdInDelay)
            {
                if (m_antiAdDelayStart + Settings.AntiAdDelay < Time.time)
                {
                    m_antiAdInDelay = false;
                }
            }
            if (m_consolidateInDelay)
            {
                if (m_consolidatorDelayStart + Settings.ConsolidatorDelay < Time.time)
                {
                    m_consolidateInDelay = false;
                }
            }
            if (m_resizeInDelay)
            {
                if (m_resizeDelayStart + Settings.ResizeDelay < Time.time)
                {
                    m_resizeInDelay = false;
                }
            }
            if (m_predictionInDelay)
            {
                if (m_predictionDelayStart + Settings.PredictionDelay < Time.time)
                {
                    m_predictionInDelay = false;
                }
            }
            if (m_timeInDelay)
            {
                if (m_timeDelayStart + Settings.TimeDelay < Time.time)
                {
                    m_timeInDelay = false;
                }
            }
        }

        public void Launch(int program)
        {
            switch(program)
            {
                case 0:
                    if (!m_manager.CheckDuplicate(m_antiAdWindow) && !m_antiAdInDelay)
                    {
                        m_manager.AddWindow(m_antiAdWindow, AdData.Function.AntiAd);
                    }
                    break;
                case 1:
                    if (!m_manager.CheckDuplicate(m_resizeWindow) && !m_resizeInDelay)
                    {
                        m_manager.AddWindow(m_resizeWindow, AdData.Function.Resize);
                    }
                    break;
                case 2:
                    if (!m_manager.CheckDuplicate(m_predictionWindow) && !m_predictionInDelay)
                    {
                        m_manager.AddWindow(m_predictionWindow, AdData.Function.Prediction);
                    }
                    break;
                case 3:
                    if (!m_manager.CheckDuplicate(m_consolidatorWindow) && !m_consolidateInDelay)
                    {
                        m_manager.AddWindow(m_consolidatorWindow, AdData.Function.Consolidate);
                    }
                    break;
                case 4:
                    if (!m_manager.CheckDuplicate(m_timeWindow) && !m_timeInDelay)
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

        public bool Purchase(int program)
        {
            switch (program)
            {
                case 0:
                    if(Settings.AntiAdCost <= m_manager.CurrentCurrency && !m_antiAdPurchased)
                    {
                        m_manager.AddCurrency(-Settings.AntiAdCost);
                        m_antiAdMenu.interactable = true;
                        m_antiAdPurchased = true;
                        return true;
                    }
                    return false;
                case 1:
                    if (Settings.ResizeCost <= m_manager.CurrentCurrency && !m_resizePurchased)
                    {
                        m_manager.AddCurrency(-Settings.ResizeCost);
                        m_resizeMenu.interactable = true;
                        m_resizePurchased = true;
                        return true;
                    }
                    return false;
                case 2:
                    if (Settings.PredictionCost <= m_manager.CurrentCurrency && !m_predictionPurchased)
                    {
                        m_manager.AddCurrency(-Settings.PredictionCost);
                        m_predictionMenu.interactable = true;
                        m_predictionPurchased = true;
                        return true;
                    }
                    return false;
                case 3:
                    if (Settings.ConsolidatorCost <= m_manager.CurrentCurrency && !m_consolidatorPurchased)
                    {
                        m_manager.AddCurrency(-Settings.ConsolidatorCost);
                        m_consolidatorMenu.interactable = true;
                        m_consolidatorPurchased = true;
                        return true;
                    }
                    return false;
                case 4:
                    if (Settings.TimeCost <= m_manager.CurrentCurrency && !m_timePurchased)
                    {
                        m_manager.AddCurrency(-Settings.TimeCost);
                        m_timeMenu.interactable = true;
                        m_timePurchased = true;
                        return true;
                    }
                    return false;
                case 5:
                    if (Settings.RamCost <= m_manager.CurrentCurrency)
                    {
                        m_manager.AddCurrency(-Settings.TimeCost);
                        Trigger(AdData.Function.Ram);
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public void Trigger(AdData.Function func, bool active = false)
        {
            switch (func)
            {
                case AdData.Function.AntiAd:
                    if(active)
                    {
                        m_antiAdActive = true;
                        m_antiAdStart = Time.time;
                        m_lastAd = Time.time;
                    }
                    else
                    {
                        m_antiAdActive = false;
                        m_antiAdMenu.interactable = false;
                        m_antiAdDelayStart = Time.time;
                        m_antiAdInDelay = true;

                    }
                    break;
                case AdData.Function.Resize:
                    if(active)
                    {
                        m_resizeActive = true;
                        m_resizeStart = Time.time;
                    }
                    else
                    {
                        m_resizeActive = false;
                        m_resizeMenu.interactable = false;
                        m_resizeDelayStart = Time.time;
                        m_resizeInDelay = true;
                    }
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
                    else
                    {
                        m_predictionActive = false;
                        m_currentPrediction.gameObject.SetActive(false);
                        m_predictionMenu.interactable = false;
                        m_predictionDelayStart = Time.time;
                        m_predictionInDelay = true;
                    }
                    break;
                case AdData.Function.Consolidate:
                    if (active)
                    {
                        m_consolidateActive = true;
                        m_consolidatorStart = Time.time;
                    }
                    else
                    {
                        m_consolidateActive = false;
                        m_consolidatorMenu.interactable = false;
                        m_consolidatorDelayStart = Time.time;
                        m_consolidateInDelay = true;
                    }
                    break;
                case AdData.Function.Time:
                    if (active)
                    {
                        m_timeActive = true;
                        m_timeStart = Time.time;
                    }
                    else
                    {
                        m_timeActive = false;
                        m_timeMenu.interactable = false;
                        m_timeDelayStart = Time.time;
                        m_timeInDelay = true;
                    }
                    break;
            }
        }

        public IEnumerator CrashEvent(string name)
        {
            m_crashText.text = name + " has crashed - please wait before attemping to launch again";
            m_crashContainer.SetActive(true);
            yield return new WaitForSeconds(5f);
            m_crashContainer.SetActive(false);
        }
        public void Predict()
        {
            m_currentPrediction.anchoredPosition = Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height) / 2);
        }
    }
}
