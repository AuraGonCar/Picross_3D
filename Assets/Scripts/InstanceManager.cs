using UnityEngine;
using Unity.Cinemachine;

public class InstanceManager : MonoBehaviour
{
    [SerializeField] GameObject currentPuzzle;
    public PicrossHintManager currentPuzzleHints;
    [SerializeField] CinemachineCamera vCam;
    public PlayerControls controls;
    void Start()
    {
        GameObject test = Instantiate(currentPuzzle, Vector3.zero, Quaternion.identity);
        test.transform.parent = null;

        if (test.TryGetComponent<PicrossHintManager>(out PicrossHintManager o))
            currentPuzzleHints = o;

        currentPuzzleHints.parent = this;

        vCam.Follow = test.transform;
    }



}
