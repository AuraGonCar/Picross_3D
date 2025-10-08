using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicrossLayerDetectionManagement : MonoBehaviour
{

    [SerializeField] List<GameObject> allLayerDetection;

    bool active = true;
    public void DisableAllLayerDetection()
    {
        if (!active)
            return;
        foreach (GameObject g in allLayerDetection)
        {
            g.SetActive(false);
        }

        active = false;

    }

    public void EnableAllLayerDetection()
    {
        if (active)
            return;
        foreach (GameObject g in allLayerDetection)
        {
            g.SetActive(true);
        }

        active = true;
    }
}
