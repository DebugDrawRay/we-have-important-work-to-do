using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagementHelper : MonoBehaviour
{
    public float timeToScene = 3;
    public int nextScene = 1;
    private float m_startTime;

    private void Awake()
    {
        m_startTime = Time.time;
    }

    private void Update()
    {
        if(m_startTime + timeToScene < Time.time)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
