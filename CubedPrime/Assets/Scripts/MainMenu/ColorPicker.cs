using System;
using System.Collections;
using System.Collections.Generic;
using Discord;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public float currentHue;
    public float currentSat;
    public float currentVal;

    public RawImage hueImage;
    public RawImage satValImage;
    public RawImage outputImage;

    public Slider _hueSlider;

    private Texture2D hueTexture;
    private Texture2D svTexture;
    private Texture2D outputTexture;

    private void Start()
    {
        CreateHueImage();
        CreateSVImage();
        
        CreateOutputImage();
        UpdateOutputImage();
    }

    private void CreateHueImage()
    {
        hueTexture = new Texture2D(1, 16);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for (int i = 0; i < hueTexture.height; i++)
        {
            hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / hueTexture.height, 1, 1));
        }
        
        hueTexture.Apply();
        currentHue = 0;

        hueImage.texture = hueTexture;
    }

    private void CreateSVImage()
    {
        svTexture = new Texture2D(16, 16);
        svTexture.wrapMode = TextureWrapMode.Clamp;
        svTexture.name = "SatValueTexture";

        for (int i = 0; i < svTexture.height; i++)
        {
            for (int j = 0; j < svTexture.width; j++)
            {
                svTexture.SetPixel(j, i, Color.HSVToRGB(currentHue, (float)j / svTexture.width, (float)i / svTexture.height));
            }
        }
        
        svTexture.Apply();
        currentSat = 0;
        currentVal = 0;

        satValImage.texture = svTexture;
    }

    private void CreateOutputImage()
    {
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";

        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }
        
        outputTexture.Apply();
        outputImage.texture = outputTexture;
    }

    private void UpdateOutputImage()
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }
        
        outputTexture.Apply();
    }

    public void SetSV(float s, float v)
    {
        currentSat = s;
        currentVal = v;
        
        UpdateOutputImage();
    }

    public void UpdateSVImage()
    {
        currentHue = _hueSlider.value;

        for (int i = 0; i < svTexture.height; i++)
        {
            for (int j = 0; j < svTexture.width; j++)
            {
                svTexture.SetPixel(j, i, Color.HSVToRGB(currentHue, (float)j / svTexture.width, (float)i / svTexture.height));
            }
        }
        
        svTexture.Apply();
        UpdateOutputImage();
    }

    public void SaveColor()
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);
        PlayerPrefs.SetFloat("ColorR", currentColor.r);
        PlayerPrefs.SetFloat("ColorG", currentColor.g);
        PlayerPrefs.SetFloat("ColorB", currentColor.b);
        PlayerPrefs.SetFloat("ColorA", currentColor.a);
    }
}
