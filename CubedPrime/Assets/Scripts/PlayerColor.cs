using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    public SpriteRenderer headColor;
    public SpriteRenderer baseColor;
    void Start()
    {
        float r = PlayerPrefs.GetFloat("ColorR", 1);
        float g = PlayerPrefs.GetFloat("ColorG", 1);
        float b = PlayerPrefs.GetFloat("ColorB", 1);
        float a = PlayerPrefs.GetFloat("ColorA", 1);

        Color color = new Color(r, g, b, a);
        headColor.color = color;
        baseColor.color = color;
    }
    
}
