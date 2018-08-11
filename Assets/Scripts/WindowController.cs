using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FSS
{
	public class WindowController : MonoBehaviour 
	{
		private RectTransform m_rt;
        private Vector3 m_cursorOffset;

		private void Awake()
		{
			m_rt = GetComponent<RectTransform>();
		}

		public void Minimize()
		{

		}
		public void Close()
		{

		}
        public void BeginDrag()
        {
            m_cursorOffset = transform.position - Input.mousePosition;
        }
		public void Drag()
		{
            transform.position = m_cursorOffset + Input.mousePosition;
        }
	}
}