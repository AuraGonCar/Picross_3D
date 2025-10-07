using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PicrossLayerControl : MonoBehaviour
{
    [SerializeField] PicrossLayerDetectionManagement parent;
    [SerializeField] List<GameObject> layers;

    bool active = false;
    public void DetectControl()
    {
        parent.DisableAllLayerControl();

        foreach(GameObject g in layers)
        {
            g.SetActive(true);
        }
    }
}

