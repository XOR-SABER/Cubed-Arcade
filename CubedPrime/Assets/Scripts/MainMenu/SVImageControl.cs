using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SVImageControl : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Camera camera;
    public Image pickerImage;
    private RawImage SVimage;
    private ColorPicker colorPicker;
    private RectTransform _rectTransform;
    private RectTransform _pickerTransform;
    private Canvas _canvas;

    private void Awake()
    {
        SVimage = GetComponent<RawImage>();
        colorPicker = FindObjectOfType<ColorPicker>();
        _rectTransform = GetComponent<RectTransform>();

        _pickerTransform = pickerImage.GetComponent<RectTransform>();
        _pickerTransform.localPosition = new Vector3(-(_rectTransform.sizeDelta.x * 0.5f), -(_rectTransform.sizeDelta.y * 0.5f), 0f);
    }

    void UpdateColor(PointerEventData eventData)
    {
        //Vector3 pos = _rectTransform.InverseTransformPoint(eventData.position);
        Vector3 pos = eventData.position;
        Vector3 rectPos = camera.WorldToScreenPoint(_rectTransform.position);
        pos.x -= rectPos.x;
        pos.y -= rectPos.y;

        float deltaX = _rectTransform.sizeDelta.x * 0.5f;
        float deltaY = _rectTransform.sizeDelta.y * 0.5f;

        if (pos.x < -deltaX)
        {
            pos.x = -deltaX;
        }
        else if (pos.x > deltaX)
        {
            pos.x = deltaX;
        }

        if (pos.y < -deltaY)
        {
            pos.y = -deltaY;
        }
        else if (pos.y > deltaY)
        {
            pos.y = deltaY;
        }

        float x = pos.x + deltaX;
        float y = pos.y + deltaY;

        float xNorm = x / _rectTransform.sizeDelta.x;
        float yNorm = y / _rectTransform.sizeDelta.y;

        pos.z = 0;
        _pickerTransform.localPosition = pos;
        pickerImage.color = Color.HSVToRGB(0, 0, 1 - yNorm);

        colorPicker.SetSV(xNorm, yNorm);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }
}
