using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicrossLayerDetectionManagement : MonoBehaviour
{
    [SerializeField] PicrossHintManager parent;
    [SerializeField] List<GameObject> allLayerControl;
    [SerializeField] List<GameObject> allLayerDetection;

    bool layerDetection = true;
    public void DisableAllLayerControl()
    {
        foreach(GameObject g in allLayerControl)
        {
            g.SetActive(false);
        }
    }

    public void DisableAllLayerControlException(GameObject ex)
    {
        foreach (GameObject g in allLayerControl)
        {
            if (g == ex)
                continue;

            g.SetActive(false);
        }
    }
    public void DisableAllLayerDetection()
    {
        if (!layerDetection)
            return;
        foreach (GameObject g in allLayerDetection)
        {
            g.SetActive(false);
        }

        layerDetection = false;
    }

    public void EnableAllLayerDetection()
    {
        if (layerDetection)
            return;

        foreach (GameObject g in allLayerDetection)
        {
            g.SetActive(true);
        }

        layerDetection = true;
    }
}
