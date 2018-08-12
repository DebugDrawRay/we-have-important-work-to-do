using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MinimizedButton : MonoBehaviour
{
    [SerializeField] private Text m_text;
    [SerializeField] private Image m_icon;

    public void Initialize(string text, Sprite icon)
    {
        m_text.text = text;
        m_icon.sprite = icon;
    }

}
