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
        [SerializeField] private Text m_name;
        [SerializeField] private Image m_icon;
        private Toggle m_miniIcon;
		private RectTransform m_rt;
        private Vector3 m_cursorOffset;

        private void Awake()
        {
            m_rt = GetComponent<RectTransform>();
            m_miniIcon = Instantiate(m_minimizeIcon).GetComponent<Toggle>();
        }

        private void Start()
        {
            InterfaceManager.instance.AddToMinimized(m_miniIcon.GetComponent<RectTransform>());
            m_miniIcon.onValueChanged.AddListener(ToggleMinimized);
            m_miniIcon.isOn = gameObject.activeInHierarchy;
        }

        public void Initialize(Sprite graphic, string name, Sprite icon, bool startMinimized = false)
        {
            gameObject.SetActive(!startMinimized);
            m_miniIcon.isOn = !startMinimized;
            m_content.sprite = graphic;
            m_name.text = name;
            m_icon.sprite = icon;
        }
        public void ToggleMinimized()
		{
            gameObject.SetActive(!gameObject.activeSelf);
            m_miniIcon.isOn = gameObject.activeSelf;
        }
        public void ToggleMinimized(bool active)
        {
            gameObject.SetActive(active);
        }
        public void Close()
		{
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