using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatedSprite : MonoBehaviour
{
    public Image m_image;
    private Sprite[] m_frames;
    private float m_fps;
    private bool m_loaded;

    public void Load(Sprite[] frames, float fps)
    {
        m_frames = frames;
        m_fps = fps;
        m_loaded = true;
    }
    private void Update()
    {
        if (m_loaded)
        {
            int index = Mathf.RoundToInt(Time.time * m_fps);
            index = index % m_frames.Length;
            m_image.sprite = m_frames[index];
        }
    }
}
