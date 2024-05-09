using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class highScore : MonoBehaviour
{
    public TMP_Text highScoreText;
    void Start()
    {
        highScoreText.text += " " + PlayerPrefs.GetInt("highScore", 0);
    }
}
