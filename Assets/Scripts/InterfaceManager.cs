using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void ToggleStartMenu()
    {
        m_startMenu.SetActive(!m_startMenu.activeSelf);
    }
}
