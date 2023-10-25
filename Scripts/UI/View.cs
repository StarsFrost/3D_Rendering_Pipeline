using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class View : MonoBehaviour
{
    public RawImage rawImage;
    public TMP_Dropdown availableObjects;
    public TMP_Dropdown availableAction;
    public TMP_Dropdown availableColor;
    public TMP_Dropdown createObject;
    public TMP_Text textcreatey;
    public Button transform;
    public Button createButton;
    public TMP_InputField tranx;
    public TMP_InputField trany;
    public TMP_InputField tranz;
    public TMP_InputField createx;
    public TMP_InputField createy;
    public TMP_InputField createz;
    public void updateObjects(List<string> currentObjects)
    {
        List<TMP_Dropdown.OptionData> data = new List<TMP_Dropdown.OptionData>();
        foreach (string str in currentObjects)
            data.Add(new TMP_Dropdown.OptionData(str));
        availableObjects.options = data;
    }

    public void updateTexture(Texture2D texture)
    {
        rawImage.texture = texture;
    }
}
