using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DesktopPet : MonoBehaviour
{
    [SerializeField] private string[] m_lines;
    [SerializeField] private float m_lineInterval = 2f;
    [SerializeField] private Text m_lineContainer;
    private float m_startTime;

    private void Awake()
    {
        m_startTime = Time.time;
    }

    private void Update()
    {
        float elapsed = Time.time - m_startTime;
        if (Mathf.RoundToInt(elapsed) % m_lineInterval == 0)
        {
            int ran = Random.Range(0, m_lines.Length);
            m_lineContainer.text = m_lines[ran];
        }
    }

    public void ChangePosition()
    {
        Vector2 pos = UnityEngine.Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height) / 2);
        GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
