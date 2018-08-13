using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CheapStretching : MonoBehaviour {

    private float minLength;
    private float maxLength;
    private float currentLength;
    private float nextLength;
    private float randomTime;

    private void Awake ()
    {
        currentLength = gameObject.GetComponent<Image>().fillAmount;
        maxLength = gameObject.GetComponent<Image>().fillAmount;
    }

    private void Start ()
    {
        ChangeLength();
    }

	private void Update ()
    {
        randomTime = UnityEngine.Random.Range(0.05f, 0.5f);
        currentLength = gameObject.GetComponent<Image>().fillAmount;
    }

    public void ChangeLength ()
    {
        nextLength = UnityEngine.Random.Range(minLength, maxLength);
        gameObject.GetComponent<Image>().DOFillAmount(nextLength, randomTime).OnComplete(ChangeLength);
    }
}
