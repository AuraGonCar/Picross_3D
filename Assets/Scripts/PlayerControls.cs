using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private InputSystem_Actions actions;

    private InputAction press;
    private InputAction mark;
    private InputAction destroy;
    
    private Vector2 mousePosition;

    [SerializeField] CinemachineInputAxisController cinemachineControls;
    [SerializeField] InstanceManager parent;


    private PicrossMode currentMode;
    

    private void Awake()
    {
        actions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        press = actions.PicrossActions.Press;

        press.Enable();
        press.performed += CameraDrag;
        press.canceled += CameraDrag;

        mark = actions.PicrossActions.Mark;
        mark.performed += MarkPiece;

        destroy = actions.PicrossActions.Destroy;
        destroy.performed += DestroyPiece;
    }

    private void OnDisable()
    {
        DisableActions();
    }

    public void ChangeControls(PicrossMode change)
    {       
        DisableActions();
        currentMode = change;
        switch (currentMode)
        {
            case PicrossMode.None:
                press.Enable();
                parent.currentPuzzleHints.layerDetection.EnableAllLayerDetection();
                break;
            case PicrossMode.Mark:
                mark.Enable();
                parent.currentPuzzleHints.layerDetection.DisableAllLayerDetection();
                parent.currentPuzzleHints.layerDetection.DisableAllLayerControl();
                break;
            case PicrossMode.Destroy:
                parent.currentPuzzleHints.layerDetection.DisableAllLayerDetection();
                parent.currentPuzzleHints.layerDetection.DisableAllLayerControl();
                destroy.Enable();
                break;
            case PicrossMode.Layer:
                parent.currentPuzzleHints.layerDetection.DisableAllLayerDetection();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (currentMode != PicrossMode.None)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.collider.TryGetComponent<PicrossLayerControl>(out PicrossLayerControl p))
            {
                p.DetectControl();
            }
        }
    }
    private void CameraDrag(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        //    cinemachineControls.enabled = true;


        //}
        //else if (context.canceled)
        //{
        //    cinemachineControls.enabled = false;

        //}

        if (context.performed)
        {
            cinemachineControls.Controllers[0].Input.Gain = 2;
            cinemachineControls.Controllers[1].Input.Gain = -2;

        }
        else if (context.canceled)
        {
            foreach (var c in cinemachineControls.Controllers)
            {
                c.Input.Gain = 0;
            }
        }

    }

    private void MarkPiece(InputAction.CallbackContext context)
    {
        Debug.Log("Mark");
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent<PicrossPiece>(out PicrossPiece p))
            {
                p.MarkPiece();
            }
        }
    }

    private void DestroyPiece(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.TryGetComponent<PicrossPiece>(out PicrossPiece p))
            {
                p.DestroyPiece();
            }
        }
    }

    private void DisableActions()
    {
        press.Disable();
        mark.Disable();
        destroy.Disable();
    }
}
