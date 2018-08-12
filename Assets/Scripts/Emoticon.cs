using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FSS
{
    public class Emoticon : MonoBehaviour
    {
        [SerializeField] private Image m_mainImage;
        [SerializeField] private Sprite[] m_icons;
        [SerializeField] private float m_lifeTimeMin = 1;
        [SerializeField] private float m_lifeTimeMax = 2;

        private float m_lifeTime;
        private float m_timeStart;
        private bool m_initialized;
        private PenaltyController.EmoteEvent m_onDestroy;

        public void Initialize(PenaltyController.EmoteEvent callback)
        {
            m_onDestroy = callback;
            m_timeStart = Time.time;
            m_lifeTime = Random.Range(m_lifeTimeMin, m_lifeTimeMax);
            int ran = Random.Range(0, m_icons.Length);
            m_mainImage.sprite = m_icons[ran];
            Vector2 pos = UnityEngine.Random.insideUnitCircle * (new Vector2(Screen.width, Screen.height) / 2);
            GetComponent<RectTransform>().anchoredPosition = pos;
            m_initialized = true;
        }

        private void Update()
        {
            if(m_initialized)
            {
                if(m_timeStart + m_lifeTime < Time.time)
                {
                    m_onDestroy(this);
                }
            }
        }
    }
}
