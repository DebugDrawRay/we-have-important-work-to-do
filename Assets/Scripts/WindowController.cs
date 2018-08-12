using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FSS
{
	public class WindowController : MonoBehaviour 
	{
        [SerializeField] private GameObject m_minimizeIcon;
        [SerializeField] private Image m_content;
        [SerializeField] private SuperTextMesh m_name;
        [SerializeField] private Image m_icon;
        private Toggle m_miniIcon;
		private RectTransform m_rt;
        private Vector3 m_cursorOffset;

        private GameManager.WindowCallback m_onClose;

        public static List<WindowController> AllWindows = new List<WindowController>();

        private void Awake()
        {
            m_rt = GetComponent<RectTransform>();
            m_miniIcon = Instantiate(m_minimizeIcon).GetComponent<Toggle>();
            AllWindows.Add(this);
        }

        private void Start()
        {
            InterfaceManager.instance.AddToMinimized(m_miniIcon.GetComponent<RectTransform>());
            m_miniIcon.onValueChanged.AddListener(ToggleMinimizedTaskbar);
            m_miniIcon.isOn = gameObject.activeSelf;
        }

        public void Initialize(Sprite graphic, string name, Sprite icon, GameManager.WindowCallback callback, bool startMinimized = false)
        {
            gameObject.SetActive(!startMinimized);
            m_miniIcon.isOn = !startMinimized;
            m_content.sprite = graphic;
            m_name.text = name;
            m_icon.sprite = icon;
            m_onClose += callback;
            transform.SetAsLastSibling();
        }
        public void ToggleMinimized()
		{
            gameObject.SetActive(!gameObject.activeSelf);
            m_miniIcon.isOn = gameObject.activeSelf;
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
            m_onClose(this);
            Destroy(gameObject);
            Destroy(m_miniIcon.gameObject);
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

	}
}