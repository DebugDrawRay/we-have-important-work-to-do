using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FSS
{
	public class WindowController : MonoBehaviour 
	{
        [SerializeField] private GameObject m_minimizeIcon;
        [SerializeField] private SuperTextMesh m_name;
        [SerializeField] private Image m_icon;
        [SerializeField] private AudioClip m_clickSound;
        [SerializeField] private AudioClip m_adSound;

        public bool programWindow;
        public bool unmanaged;
        private AdData.Function m_function;
        private Toggle m_miniIcon;
		private RectTransform m_rt;
        private Vector3 m_cursorOffset;

        private GameManager.WindowCallback m_onClose;

        public static List<WindowController> AllWindows = new List<WindowController>();

        private void Awake()
        {
            if (!unmanaged)
            {
                m_rt = GetComponent<RectTransform>();
                m_miniIcon = Instantiate(m_minimizeIcon).GetComponent<Toggle>();
                AllWindows.Add(this);
            }
        }

        private void Start()
        {
            if (!unmanaged)
            {
                InterfaceManager.instance.AddToMinimized(m_miniIcon.GetComponent<RectTransform>());
                m_miniIcon.onValueChanged.AddListener(ToggleMinimizedTaskbar);
                m_miniIcon.isOn = gameObject.activeSelf;
            }
        }

        public void Initialize(AdData.Function func, GameManager.WindowCallback callback, bool startMinimized = false)
        {
            m_function = func;

            gameObject.SetActive(!startMinimized);
            m_miniIcon.isOn = !startMinimized;

            m_onClose += callback;
            transform.SetAsLastSibling();
            if (programWindow)
            {
                ProgramManager.instance.Trigger(m_function, true);
            }
        }

        public void Initialize(AdData data, GameManager.WindowCallback callback, bool startMinimized = false)
        {
            m_name.text = data.name;
            m_icon.sprite = data.icon;
            GetComponent<AnimatedSprite>().Load(data.frames, data.fps);
            m_function = data.func;
            if (ProgramManager.instance.ResizeActive)
            {
                Resize(Settings.SmallWindowSize);
            }
            else
            {
                Resize(data.imageSize);
            }

            gameObject.SetActive(!startMinimized);
            m_miniIcon.isOn = !startMinimized;

            m_miniIcon.GetComponentInChildren<MinimizedButton>().Initialize(data.name, data.icon);

            m_onClose += callback;
            transform.SetAsLastSibling();
            if (programWindow)
            {
                ProgramManager.instance.Trigger(m_function, true);
            }
            else
            {
                if(data.sfx != null)
                {
                    AudioManager.PlaySfx(data.sfx);
                }
            }

        }
        public void ToggleMinimized()
		{
            m_miniIcon.isOn = false;
            gameObject.SetActive(false);
            m_miniIcon.GetComponent<Toggle>().graphic.CrossFadeAlpha(0, 0, false);
        }
        public void ToggleMinimizedTaskbar(bool active)
        {
            if(!m_miniIcon.isOn)
            {
                InterfaceManager.instance.SelectMinimized(m_miniIcon.GetComponent<RectTransform>());
            }
            gameObject.SetActive(true);
            m_miniIcon.isOn = true;
            transform.SetAsLastSibling();
        }

        public void Close()
		{
            if (programWindow)
            {
                ProgramManager.instance.Trigger(m_function, false);
            }
            m_onClose(this);
            Destroy(gameObject);
            Destroy(m_miniIcon.gameObject);
		}
        public void MouseDown()
        {
            AudioManager.PlaySfx(m_clickSound);
        }
        public void BeginDrag()
        {
            m_cursorOffset = transform.position - Input.mousePosition;
            transform.SetAsLastSibling();

        }
        public void Drag()
		{
            transform.position = m_cursorOffset + Input.mousePosition;
        }
        public void EndDrag()
        {
            RectTransform winRect = GetComponent<RectTransform>();
            Vector2 position = winRect.anchoredPosition;
                
            if (position.y + (winRect.rect.height / 2) > Screen.height / 2)
            {
                position.y = (Screen.height / 2) - (winRect.rect.height / 2);
            }
            if (position.y < -(Screen.height / 2))
            {
                position.y = (float)-(Screen.height / 2);
            }
            if (position.x > Screen.width / 2)
            {
                position.x = (Screen.width / 2) - (winRect.rect.width * .25f);
            }
            if (position.x < -(Screen.width / 2))
            {
                position.x = (float)-(Screen.width / 2);
            }
            winRect.anchoredPosition = position;
        }
        public void WindowFunction()
        {
            transform.SetAsLastSibling();
            if (m_function != AdData.Function.None && !programWindow)
            {
                AudioManager.PlaySfx(m_adSound);
                PenaltyController.instance.TriggerPenalty(m_function);
                Close();
            }
            else
            {
                AudioManager.PlaySfx(m_clickSound);
            }
        }
        public void Resize(Vector2 newSize)
        {
            newSize.y += 34;
            newSize.x += 12;
            GetComponent<RectTransform>().sizeDelta = newSize;
        }

	}
}