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
        [SerializeField] private GameObject m_alienPet;

        private List<Emoticon> m_currentEmotes = new List<Emoticon>();
        private List<DesktopPet> m_currentPets = new List<DesktopPet>();

        private bool m_changeCursor;
        private bool m_emoticons;
        private bool m_slowdown;
        private bool m_pet;
        private bool m_speedUp;

        private float m_cursorStart;
        private float m_emoticonsStart;
        private float m_slowdownStart;
        private float m_petStart;
        private float m_speedUpStart;

        private Sprite m_previousBg;

        private float m_lastEmoteTime;
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
        private GameManager m_manager;
        private void Awake()
        {
            instance = this;
            m_manager = GetComponent<GameManager>();

        }

        public void Reinitialize()
        {
            DestroyAllEmotes();
            DestroyAllPets();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        public void TriggerPenalty(AdData.Function func)
        {
            int ran = 0;
            switch (func)
            {
                case AdData.Function.BackgroundChange:
                    ran = Random.Range(0, m_backgrounds.Length);
                    m_background.sprite = m_backgrounds[ran];
                    break;
                case AdData.Function.Cursor:
                    m_cursorStart = Time.time;
                    ran = Random.Range(0, m_cursors.Length);
                    Cursor.SetCursor(m_cursors[ran], m_cursorHotspots[ran], CursorMode.ForceSoftware);
                    m_changeCursor = true;
                    break;
                case AdData.Function.DesktopPet:
                    m_petStart = Time.time;
                    DesktopPet pet = Instantiate(m_desktopPet, m_elementContainer).GetComponent<DesktopPet>();
                    pet.Initialize(DestroyPet);
                    m_currentPets.Add(pet);
                    m_pet = true;
                    break;
                case AdData.Function.AlienPet:
                    m_petStart = Time.time;
                    DesktopPet alien = Instantiate(m_alienPet, m_elementContainer).GetComponent<DesktopPet>();
                    alien.Initialize(DestroyPet);
                    m_currentPets.Add(alien);
                    m_pet = true;
                    break;
                case AdData.Function.DownloadSlowdown:
                    m_slowdownStart = Time.time;
                    m_slowdown = true;
                    break;
                case AdData.Function.Emoticons:
                    m_emoticonsStart = Time.time;
                    m_lastEmoteTime = Time.time;
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
            if(m_manager.GameActive)
            {
                UpdatePenalties();

            }
        }
        private void UpdatePenalties()
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
                    if (m_lastEmoteTime + Settings.EmoteRate < Time.time)
                    {
                        Emoticon e = Instantiate(m_emote, m_elementContainer).GetComponent<Emoticon>();
                        e.Initialize(DestroyEmote);
                        m_lastEmoteTime = Time.time;
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

        public delegate void PetEvent(DesktopPet pet);
        public void DestroyPet(DesktopPet pet)
        {
            m_currentPets.Remove(pet);
            Destroy(pet.gameObject);
        }
        public void DestroyAllPets()
        {
            foreach (DesktopPet e in m_currentPets)
            {
                Destroy(e.gameObject);
            }
            m_currentPets = new List<DesktopPet>();
        }
    }
}
