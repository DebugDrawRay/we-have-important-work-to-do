using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
namespace FSS
{
    public class TypewriterEffect : MonoBehaviour
    {
        [TextArea] public string m_textToWrite;
        [SerializeField] private float m_writeSpeed;
        [SerializeField] private float m_eventDelay;
        [SerializeField] private bool m_storeOriginalText = true;
        [SerializeField] private bool m_playOnStart = false;
        [SerializeField] private UnityEvent m_onComplete;

        private string m_originalText;
        private Text text;
        private Tween type;
        private void Awake()
        {
            text = GetComponent<Text>();
            m_originalText = text.text;
        }
        private void Start()
        {
            if (m_playOnStart)
            {
                Write();
            }
        }

        public void Write()
        {
            if (text != null)
            {
                if (type != null && type.IsActive())
                {
                    type.Kill();
                }
                text.text = "";
                type = text.DOText(m_textToWrite, m_writeSpeed).OnComplete(InvokeOnComplete).SetEase(Ease.Linear);
            }
        }
        public void Revert(bool typewrite)
        {
            if (text != null)
            {
                if (type != null && type.IsActive())
                {
                    type.Kill();
                }
                text.text = "";
                if (typewrite)
                {
                    text.DOText(m_originalText, m_writeSpeed).SetEase(Ease.Linear);
                }
                else
                {
                    text.text = m_originalText;
                }
            }
        }

        private void InvokeOnComplete()
        {
            StartCoroutine(InvokeCoroutine());
        }

        private IEnumerator InvokeCoroutine()
        {
            yield return new WaitForSeconds(m_eventDelay);
            m_onComplete.Invoke();
        }
    }
}
