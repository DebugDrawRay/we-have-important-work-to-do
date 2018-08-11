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

        private void Awake()
        {
            instance = this;
            m_startMenu.SetActive(false);
        }

        public void AddToMinimized(RectTransform item)
        {
            item.SetParent(m_minimizedContainer);
            item.GetComponent<Toggle>().group = m_minimizedContainer.GetComponent<ToggleGroup>();
        }

        public void ToggleStartMenu(bool active)
        {
            m_startMenu.SetActive(active);
        }
    }
}
