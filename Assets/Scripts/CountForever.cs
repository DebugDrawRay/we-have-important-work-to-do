using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountForever : MonoBehaviour
{
    private int bytes;
    public SuperTextMesh byteText;
    private int randAdd;
    private bool overflowingBytes;

    // Use this for initialization
    private void Awake()
    {
        UpdateNum();
        bytes = 0;
        byteText.text = "Mal-Bytes Scanned: " + bytes.ToString();
        randAdd = Random.Range(0, 50);
        overflowingBytes = false;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateNum();
    }

    private void UpdateNum()
    {
        bytes = bytes + randAdd;
        if (!overflowingBytes)
        {
            if (bytes > 30000)
            {
                bytes = 0;
                byteText.text = "Mal-Bytes Scanned: a lot";
                overflowingBytes = true;
            }
            else
            {
                byteText.text = "Mal-Bytes Scanned: " + bytes.ToString();
            }
        }
    }

}