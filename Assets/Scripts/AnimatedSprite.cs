using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatedSprite : MonoBehaviour
{
    public Image m_image;
    public Sprite[] p_frames;
    public float p_fps;
    public bool loadFromPublic;
    private Sprite[] m_frames;
    private float m_fps;
    private bool m_loaded;

    private void Start()
    {
        if(loadFromPublic)
        {
            Load(p_frames, p_fps);
        }
    }
    public void Load(Sprite[] frames, float fps)
    {
        m_frames = frames;
        m_fps = fps;
        if (frames != null && frames[0] != null)
        {
            m_image.sprite = m_frames[0];
        }
        m_loaded = true;
    }
    private void Update()
    {
        if (m_loaded && m_fps != 0)
        {
            int index = Mathf.RoundToInt(Time.time * m_fps);
            index = index % m_frames.Length;
            m_image.sprite = m_frames[index];
        }
    }
}
