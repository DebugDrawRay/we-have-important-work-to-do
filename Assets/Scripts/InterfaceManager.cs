using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FSS
{
    public class InterfaceManager : MonoBehaviour
    {
        [SerializeField] private RectTransform m_minimizedContainer;
        [SerializeField] private GameObject m_startMenu;

        public static InterfaceManager instance;

        private RectTransform m_lastClicked;
        private void Awake()
        {
            instance = this;
            m_startMenu.SetActive(false);
        }

        public void AddToMinimized(RectTransform item)
        {
            SelectMinimized(item);
            item.SetParent(m_minimizedContainer);
        }

        public void ToggleStartMenu(bool active)
        {
            m_startMenu.SetActive(active);
        }

        public void SelectMinimized(RectTransform mini)
        {
            if (m_lastClicked != null)
            {
                m_lastClicked.GetComponent<Toggle>().graphic.CrossFadeAlpha(0, 0, true);
            }
            m_lastClicked = mini;
        }
    }
}
