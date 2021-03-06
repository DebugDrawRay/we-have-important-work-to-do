﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace FSS
{
    public class DesktopPet : MonoBehaviour
    {
        [SerializeField] private bool m_dialogueActive;
        [SerializeField] private string[] m_lines;
        [SerializeField] private float m_lineInterval = 2f;
        [SerializeField] private Text m_lineContainer;
        [SerializeField] private float m_lifeTime = 15f;
        [SerializeField] private AudioClip m_audioLoop;

        private float m_timeStart;

        private PenaltyController.PetEvent m_onDestroy;
        private float m_lastPosTime;
        private float m_lastLineTime;

        private AudioSource m_currentSource;
        private void Awake()
        {
            m_timeStart = Time.time;
            m_lastPosTime = Time.time;
            m_lastLineTime = Time.time;
        }

        public void Initialize(PenaltyController.PetEvent callback)
        {
            m_onDestroy = callback;
            Vector2 pos = UnityEngine.Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height) / 2);
            GetComponent<RectTransform>().anchoredPosition = pos;
            m_currentSource = AudioManager.PlaySfx(m_audioLoop, .25f);
        }

        private void Update()
        {
            if (m_lastLineTime + m_lineInterval < Time.time && m_dialogueActive)
            {
                int ran = Random.Range(0, m_lines.Length);
                m_lineContainer.text = m_lines[ran];
                m_lastLineTime = Time.time;
            }
            if (m_lastPosTime + Settings.PetPositionRate < Time.time)
            {
                ChangePosition();
                m_lastPosTime = Time.time;
            }
            if (m_timeStart + m_lifeTime < Time.time)
            {
                m_currentSource.Stop();
                m_onDestroy(this);
            }
        }

        public void ChangePosition()
        {
            Vector2 pos = UnityEngine.Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height) / 2);
            GetComponent<RectTransform>().DOAnchorPos(pos, Settings.PetPositionRate/2).SetEase(Ease.Linear);
        }
    }
}
