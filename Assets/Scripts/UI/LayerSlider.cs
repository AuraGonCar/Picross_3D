using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LayerSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Slider currentSlider;

    [SerializeField] LayerSlider oppositeAxis;
    [SerializeField] PicrossHintManager parent;
    public bool isMax;

    private bool active;

    
    public void OnPointerDown(PointerEventData eventData)
    {
        parent.BeginLayerControl();
        parent.layerDetection.DisableAllLayerControlException(this.gameObject);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        parent.EndLayerControl();
        
    }
}
