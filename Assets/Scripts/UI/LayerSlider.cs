using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LayerSlider : MonoBehaviour
{
    public Slider currentSlider;

    [SerializeField] LayerSlider oppositeAxis;
    [SerializeField] PicrossHintManager parent;
    public bool isMax;

    private bool active;


}
