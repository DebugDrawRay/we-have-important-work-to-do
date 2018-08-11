using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private RectTransform m_minimizedContainer;

    public static InterfaceManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddToMinimized(RectTransform item)
    {
        item.SetParent(m_minimizedContainer);
    }
}
