using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAdDatabase", menuName = "Important/New Ad Database")]
public class AdDatabase : ScriptableObject
{
    public AdData[] entries;

    public AdData RequestRandom()
    {
        int ran = Random.Range(0, entries.Length);
        return entries[ran];
    }
}

[System.Serializable]
public class AdData
{
    public string name;
    public Sprite[] frames;
    public Sprite icon;
    public float fps;
    public Vector2 imageSize
    {
        get
        {
            return frames[0].rect.size;
        }
    }
    public Function func;
    public enum Function
    {
        None,
        Cursor,
        Emoticons,
        DesktopPet,
        BackgroundChange,
        PopUpSpeed
    }
}