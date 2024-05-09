using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PointsAnimation : MonoBehaviour
{
    private TMP_Text _text;
    private string _initText;
    [FormerlySerializedAs("gameOverManager")] public GameOverInfoManager gameOverInfoManager;

    private void InitText()
    {
        _text = GetComponent<TMP_Text>();
        _initText = _text.text + ' ';
    }

    public void Type(int points, float scaleTime, AnimationCurve curve, float transitionTime)
    {
        StartCoroutine(TypePoints(points, scaleTime, curve, transitionTime));
    }

    IEnumerator TypePoints(int points, float scaleTime, AnimationCurve curve, float transitionTime)
    {
        InitText();
        yield return new WaitForSeconds(transitionTime);
        _text.text = _initText + "0";
        
        if (points != 0)
        {
            int currentPoints = 0;

            float t = 0f;

            while (t < 1f)
            {
                t += 0.01f;
                yield return new WaitForSeconds(scaleTime / 100);
            
                currentPoints = Mathf.RoundToInt(curve.Evaluate(t) * points);
                _text.text = _initText + currentPoints;
            }
        }
        
        gameOverInfoManager.NextText();
    }
}
