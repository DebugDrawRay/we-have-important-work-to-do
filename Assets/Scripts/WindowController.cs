using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FSS
{
	public class WindowController : MonoBehaviour 
	{
        [SerializeField] private GameObject m_minimizeIcon;
        private Button miniIcon;
		private RectTransform m_rt;
        private Vector3 m_cursorOffset;

		private void Awake()
		{
			m_rt = GetComponent<RectTransform>();
            miniIcon = Instantiate(m_minimizeIcon).GetComponent<Button>();
            miniIcon.onClick.AddListener(Enlarge);
            InterfaceManager.instance.AddToMinimized(miniIcon.GetComponent<RectTransform>());
		}

		public void Minimize()
		{
            gameObject.SetActive(false);
            m_minimizeIcon.SetActive(true);
        }

        public void Enlarge()
        {
            gameObject.SetActive(true);
        }

        public void Close()
		{
            gameObject.SetActive(false);
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