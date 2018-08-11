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
        private Button m_miniIcon;
		private RectTransform m_rt;
        private Vector3 m_cursorOffset;

		private void Awake()
		{
			m_rt = GetComponent<RectTransform>();
            m_miniIcon = Instantiate(m_minimizeIcon).GetComponent<Button>();
            InterfaceManager.instance.AddToMinimized(m_miniIcon.GetComponent<RectTransform>());
		}

        private void Start()
        {
            m_miniIcon.onClick.AddListener(ToggleMinimized);
        }

        public void Initialize(Sprite graphic, bool startMinimized = false)
        {
            gameObject.SetActive(!startMinimized);
            m_content.sprite = graphic;
        }
        public void ToggleMinimized()
		{
            gameObject.SetActive(!gameObject.activeSelf);
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