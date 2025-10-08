using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PicrossLayerControl : MonoBehaviour
{
    [SerializeField] PicrossHintManager parent;

    public bool active = false;

    public bool isMax;

    public bool layerX;
    public bool layerZ;

    public int originalValue;
    public int currentValue;

    private int maxValue;
    private int minValue;

    private void Start()
    {
        if (layerX)
        {
            minValue = parent.minXPosition;
            maxValue = parent.maxXPosition;

            if (isMax)
            {
                originalValue = parent.maxXPosition;
                currentValue = originalValue;
            }
            else
            {
                originalValue = parent.minXPosition;
                currentValue = originalValue;
            }
        }
        else if(layerZ)
        {
            minValue = parent.minZPosition;
            maxValue = parent.maxZPosition;


            if (isMax)
            {
                originalValue = parent.maxZPosition;
                currentValue = originalValue;
            }
            else
            {
                originalValue = parent.minZPosition;
                currentValue = originalValue;
            }
        }
        else
        {
            Debug.Log("No value selected for " + gameObject.name);
        }
    }


    public void SetActive()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    public bool LayerState()
    {
        return active;
    }

    public bool IsCurrentValueOriginal()
    {
        return currentValue == originalValue;
    }


    public void AddValue(int newValue)
    {
        currentValue += newValue;
        ClampValue();
    }

    public void RemoveValue(int newValue)
    {
        currentValue -= newValue;
        ClampValue();
    }

    private void ClampValue()
    {
        if (currentValue >= maxValue)
        {
            currentValue = maxValue;
        }

        if (currentValue <= minValue)
        {
            currentValue = minValue;
        }
    }


    
}

