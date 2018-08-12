using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FSS
{
    public class PenaltyController : MonoBehaviour
    {
        [SerializeField] private RectTransform m_elementContainer;
        [SerializeField] private Image m_background;

        [SerializeField] private GameObject m_emote;
        [SerializeField] private Texture2D[] m_cursors;
        [SerializeField] private Vector2[] m_cursorHotspots;
        [SerializeField] private Sprite[] m_backgrounds;
        [SerializeField] private GameObject m_desktopPet;

        private List<Emoticon> m_currentEmotes = new List<Emoticon>();
        private DesktopPet m_currentPet;

        private bool m_changeCursor;
        private bool m_emoticons;
        private bool m_slowdown;
        private bool m_pet;
        private bool m_backgroundChange;
        private bool m_speedUp;

        private float m_cursorStart;
        private float m_emoticonsStart;
        private float m_slowdownStart;
        private float m_petStart;
        private float m_backgroundStart;
        private float m_speedUpStart;

        private Sprite m_previousBg;

        public float SpeedMulti
        {
            get
            {
                if(m_speedUp)
                {
                    return Settings.PenaltySpeedFactor;
                }
                else
                {
                    return 1;
                }
            }
        }
        public static PenaltyController instance;

        private void Awake()
        {
            instance = this;
        }

        public void Reinitialize()
        {
            DestroyAllEmotes();
            Destroy(m_currentPet.gameObject);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        public void TriggerPenalty(AdData.Function func)
        {
            int ran = 0;
            switch (func)
            {
                case AdData.Function.BackgroundChange:
                    m_backgroundStart = Time.time;
                    m_previousBg = m_background.sprite;
                    ran = Random.Range(0, m_backgrounds.Length);
                    m_background.sprite = m_backgrounds[ran];
                    m_backgroundChange = true;
                    break;
                case AdData.Function.Cursor:
                    m_cursorStart = Time.time;
                    ran = Random.Range(0, m_cursors.Length);
                    Cursor.SetCursor(m_cursors[ran], m_cursorHotspots[ran], CursorMode.ForceSoftware);
                    m_changeCursor = true;
                    break;
                case AdData.Function.DesktopPet:
                    m_petStart = Time.time;
                    m_currentPet = Instantiate(m_desktopPet, m_elementContainer).GetComponent<DesktopPet>();
                    m_pet = true;
                    break;
                case AdData.Function.DownloadSlowdown:
                    m_slowdownStart = Time.time;
                    m_slowdown = true;
                    break;
                case AdData.Function.Emoticons:
                    m_emoticonsStart = Time.time;
                    m_emoticons = true;
                    break;
                case AdData.Function.PopUpSpeed:
                    m_speedUpStart = Time.time;
                    m_speedUp = true;
                    break;
            }
        }

        private void Update()
        {
            if (m_changeCursor)
            {
                if (m_cursorStart + Settings.PenaltyTime < Time.time)
                {
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    m_changeCursor = false;
                }
            }
            if (m_emoticons)
            {
                if (m_emoticonsStart + Settings.PenaltyTime >= Time.time)
                {
                    float elapsed = Time.time - m_cursorStart;
                    if (Mathf.RoundToInt(elapsed) % Settings.EmoteRate == 0)
                    {
                        Emoticon e = Instantiate(m_emote, m_elementContainer).GetComponent<Emoticon>();
                        e.Initialize(DestroyEmote);
                    }
                }
                else
                {
                    DestroyAllEmotes();
                    m_emoticons = false;
                }
            }
            if (m_slowdown)
            {

            }
            if (m_pet)
            {
                if (m_petStart + Settings.PenaltyTime >= Time.time)
                {
                    float elapsed = Time.time - m_cursorStart;
                    if (Mathf.RoundToInt(elapsed) % Settings.PetPositionRate == 0)
                    {
                        m_currentPet.ChangePosition();
                    }
                }
                else
                {
                    Destroy(m_currentPet.gameObject);
                    m_pet = false;
                }
            }
            if (m_backgroundChange)
            {
                if (m_backgroundStart + Settings.PenaltyTime < Time.time)
                {
                    m_background.sprite = m_previousBg;
                    m_backgroundChange = false;
                }
            }
            if (m_speedUp)
            {
                if (m_speedUpStart + Settings.PenaltyTime < Time.time)
                {
                    m_speedUp = false;
                }
            }

        }

        public delegate void EmoteEvent(Emoticon emote);
        public void DestroyEmote(Emoticon emote)
        {
            m_currentEmotes.Remove(emote);
            Destroy(emote.gameObject);
        }
        public void DestroyAllEmotes()
        {
            foreach(Emoticon e in m_currentEmotes)
            {
                Destroy(e.gameObject);
            }
            m_currentEmotes = new List<Emoticon>();
        }
    }
}
