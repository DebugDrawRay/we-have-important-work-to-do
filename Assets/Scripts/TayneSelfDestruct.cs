using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TayneSelfDestruct : MonoBehaviour {

    public Image m_image;
    public Sprite[] p_frames;
    public float p_fps;
    public bool loadFromPublic;
    private Sprite[] m_frames;
    private float m_fps;
    private bool m_loaded;

    [SerializeField] private float duration;
    private float startTime;
    private float elapsedTime;

    private void Start()
    {
        if (loadFromPublic)
        {
            Load(p_frames, p_fps);
        }
    }

    private void OnEnable ()
    {
        elapsedTime = 0;
        startTime = Time.time;
	}

    public void Load(Sprite[] frames, float fps)
    {
        m_frames = frames;
        m_fps = fps;
        if (m_frames != null && m_frames.Length > 0)
        {
            m_image.sprite = m_frames[0];
        }
        m_loaded = true;
    }

    private void Update()
    {
        elapsedTime = Time.time - startTime;

        if (elapsedTime > duration)
        {
            gameObject.SetActive(false);
        }

        if (m_loaded && m_fps != 0)
        {
            int index = Mathf.RoundToInt(elapsedTime * m_fps);
            index = index % m_frames.Length;
            m_image.sprite = m_frames[index];
        }
    }
}
