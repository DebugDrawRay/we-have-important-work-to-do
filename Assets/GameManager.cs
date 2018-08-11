using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSS
{
    public class GameManager : MonoBehaviour
    {
        [Header("Ram")]
        [SerializeField] private GameObject[] m_ramPips;
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

        public delegate void WindowCallback(WindowController windows);

        private void Awake()
        {
            m_currentRam = Settings.StartRam;
            m_popUpInterval = Settings.StartPopupInterval;
        }

        private void Update()
        {
            UpdatePopUps();
            UpdateRam();
        }

        private void UpdateRam()
        {
            float progress = (m_currentUseage / m_currentRam) * m_ramPips.Length;

            for(int i = 0; i < m_ramPips.Length; i++)
            {
                m_ramPips[i].SetActive(i + 1 <= progress);
            }
        }
        private void UpdatePopUps()
        {
            if(m_lastWindowTime + m_popUpInterval <= Time.time)
            {
                WindowController window = Instantiate(m_window, m_windowContainer).GetComponent<WindowController>();
                Vector2 pos = Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height)/2);
                window.GetComponent<RectTransform>().anchoredPosition = pos;
                m_currentWindows.Add(window);
                window.Initialize(null, "Test Window", null, CloseWindow);
                m_lastWindowTime = Time.time;
            }
        }

        public void CloseWindow(WindowController window)
        {
            if(m_currentWindows.Contains(window))
            {
                m_currentWindows.Remove(window);
            }
        }
    }
}