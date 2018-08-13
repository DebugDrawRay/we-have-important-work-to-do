using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private int m_musicSourceCount = 3;
    [SerializeField] private int m_sfxPoolStartSize = 5;
    [Range(0, 1f)]
    [SerializeField] private float m_musicVolume;
    [Range(0, 1f)]
    [SerializeField] private float m_sfxVolume;

    private static AudioSource[] m_musicSources;
    private static List<AudioSource> m_sfxPool = new List<AudioSource>();
    private static AudioManager instance;
    private void Awake()
    {
        instance = this;
        m_musicSources = new AudioSource[m_musicSourceCount];
        for (int i = 0; i < m_musicSources.Length; i++)
        {
            m_musicSources[i] = gameObject.AddComponent<AudioSource>();
            m_musicSources[i].volume = m_musicVolume;
            m_musicSources[i].playOnAwake = false;
            m_musicSources[i].loop = false;
            m_musicSources[i].spatialBlend = 0;
            m_musicSources[i].reverbZoneMix = 0;
        }
        for (int i = 0; i < m_sfxPoolStartSize; i++)
        {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            s.volume = m_sfxVolume;
            s.playOnAwake = false;
            s.loop = false;
            s.spatialBlend = 0;
            s.reverbZoneMix = 0;
            m_sfxPool.Add(s);
        }
    }

    public static AudioSource PlaySfx(AudioClip clip, float volumeOverride = -1)
    {
        foreach (AudioSource a in m_sfxPool)
        {
            if (!a.isPlaying)
            {
                a.clip = clip;
                a.volume = volumeOverride == -1 ? instance.m_sfxVolume : volumeOverride;
                a.Play();
                return a;
            }
        }
        AudioSource newSource = instance.gameObject.AddComponent<AudioSource>();
        newSource.clip = clip;
        newSource.volume = volumeOverride;
        newSource.volume = volumeOverride == -1 ? instance.m_sfxVolume : volumeOverride;
        newSource.Play();
        m_sfxPool.Add(newSource);
        return newSource;
    }
    public static void PlaySfxQueue(AudioClip[] clips, bool loopLast)
    {
        foreach (AudioSource a in m_sfxPool)
        {
            if (!a.isPlaying)
            {
                instance.StartQueue(clips, a, loopLast);
                return;
            }
        }
        AudioSource newSource = instance.gameObject.AddComponent<AudioSource>();
        m_sfxPool.Add(newSource);
        instance.StartQueue(clips, newSource, loopLast);

    }

    public static void PlayMusic(AudioClip clip, int index, bool looping)
    {
        if(index < m_musicSources.Length)
        {
            m_musicSources[index].clip = clip;
            m_musicSources[index].loop = looping;
            m_musicSources[index].Play();
        }
    }

    public void StartQueue(AudioClip[] clips, AudioSource source, bool loopLast)
    {
        StartCoroutine(PlayQueue(clips, source, loopLast));
    }
    public IEnumerator PlayQueue(AudioClip[] clips, AudioSource source, bool loopLast)
    {
        for(int i = 0; i < clips.Length; i++)
        {
            source.clip = clips[i];
            source.Play();
            source.loop = i == clips.Length - 1 && loopLast;
            yield return new WaitForSeconds(clips[i].length);
        }
    }

    public static void StopAll()
    {
        foreach (AudioSource a in m_sfxPool)
        {
            a.Stop();
        }
        foreach (AudioSource a in m_musicSources)
        {
            a.Stop();
        }
    }
}
