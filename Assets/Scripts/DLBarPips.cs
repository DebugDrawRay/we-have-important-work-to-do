using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLBarPips : MonoBehaviour {

    [SerializeField] private GameObject[] dlPips;
    [SerializeField] private float downloadTime;
    private float startTime;
    private float elapsedTime;
    private float elapsedPercentage;

    private void Awake () {
        startTime = Time.time;
	}
	
	private void Update () {
        elapsedTime = Time.time - startTime;
        elapsedPercentage = elapsedTime / downloadTime;
        UpdateDL();
	}

    private void UpdateDL()
    {
        float progress = elapsedPercentage * dlPips.Length;

        for (int i = 1; i < dlPips.Length; i++)
        {
            dlPips[i].SetActive(i + 1 <= progress);
        }

    }

}
