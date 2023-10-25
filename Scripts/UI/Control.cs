using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    View view;
    Dictionary<string, int> objectsID;
    List<Vector4> positionBuffer;

    void Start()
    {
        objectsID = new Dictionary<string, int>();
        positionBuffer = new List<Vector4>();
        view = GetComponent<View>();
        view.transform.onClick.AddListener(Transform);
        view.createButton.onClick.AddListener(GatherData);
        view.createObject.onValueChanged.AddListener(delegate { DisplayCreate(view.createObject); }) ;
        Model.Data.Init(800, 600);
        Model.Data.handler += UpdateUI;
        Model.Data.UpdateImage();
    }

    private void UpdateUI(Model m)
    {
        view.updateTexture(m.texture);
        view.updateObjects(m.scene.ObjectNames);
    }

    void GatherData()
    {
        positionBuffer.Add(new Vector4(float.Parse(view.createx.text), float.Parse(view.createy.text), float.Parse(view.createz.text)));
        switch (view.createObject.value)
        {
            case 0:
                if (positionBuffer.Count == 2)
                {
                    Create(new Vector4[2] { positionBuffer[0], positionBuffer[1]});
                    positionBuffer.Clear();
                }
                break;
            case 1:
                if (positionBuffer.Count == 3)
                {
                    Create(new Vector4[3] { positionBuffer[0], positionBuffer[1], positionBuffer[2] });
                    positionBuffer.Clear();
                }
                break;
            case 2:
                if (positionBuffer.Count == 1)
                {
                    Create(new Vector4[1] { positionBuffer[0] });
                    positionBuffer.Clear();
                }
                break;
            case 3:
                if (positionBuffer.Count == 2)
                {
                    Create(new Vector4[2] { positionBuffer[0], positionBuffer[1] });
                    positionBuffer.Clear();
                }
                break;
            default:
                break;
        }
    }

    void Create(Vector4[] data)
    {
        Color color = Color.cyan;
        switch (view.availableColor.value) 
        {
            case 0:
                color = Color.red;
                break;
            case 1:
                color = Color.green;
                break;
            case 2:
                color = Color.blue;
                break;
            case 3:
                color = Color.yellow;
                break;
            case 4:
                color = Color.white; 
                break;
        }

        IFactory factory = new CubeFactory();
        string type = "";
        switch (view.createObject.value)
        {
            case 0:
                factory = new CubeFactory();
                type = "Cube";
                break;
            case 1:
                factory = new TriangleFactory();
                type = "Triangle";
                break;
            case 2:
                factory = new CircleFactory();
                type = "Circle";
                break;
            case 3: 
                factory = new LineFactory();
                type = "Line";
                break;
            default:
                break;
        }
        GenerateObject(type, data, color, factory);
    
    }

    void GenerateObject(string type, Vector4[] data, Color color, IFactory f)
    {
        int id;
        id = objectsID.TryGetValue(type, out id) ? id : 0;
        id += 1;
        objectsID[type] = id;
        Object obj = f.CreateObject(data, color);
        Model.Data.AddObject(type + id.ToString(), obj);
    }
    void Transform()
    {
        int value = view.availableAction.value;
        string key = view.availableObjects.captionText.text;
        if (value == 3)
        {
            Model.Data.RemoveObject(key);
            return;
        }
        Model.Data.Transform(key, value, float.Parse(view.tranx.text), float.Parse(view.trany.text), float.Parse(view.tranz.text));
    }

    void DisplayCreate(TMP_Dropdown t)
    {
        view.textcreatey.gameObject.SetActive(true);
        view.textcreatey.text = "z";
        view.createz.gameObject.SetActive(true);
        if (view.createObject.value == 3) 
        {
            view.textcreatey.gameObject.SetActive(false);
            view.createz.gameObject.SetActive(false);
        }
        if (view.createObject.value == 2)
        {
            view.textcreatey.text = "radius";
        }
    }
}
