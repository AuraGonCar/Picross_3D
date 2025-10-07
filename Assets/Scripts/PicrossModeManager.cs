using UnityEngine;
using UnityEngine.UI;
public class PicrossModeManager : MonoBehaviour
{
    [SerializeField] private PicrossMode currentMode;
    [SerializeField] private PlayerControls controls;

    [SerializeField] private Image markButton;
    [SerializeField] private Image destroyButton;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color markColor;
    [SerializeField] private Color destroyColor;


    private void ChangeModes(PicrossMode modeChange)
    {
        if (modeChange == currentMode)
        {
            currentMode = PicrossMode.None;
            destroyButton.color = defaultColor;
            markButton.color = defaultColor;
        }
        else
            currentMode = modeChange;

        controls.ChangeControls(currentMode);
    }

    public void ChangeModeMark()
    {
        destroyButton.color = defaultColor;
        markButton.color = markColor;

        ChangeModes(PicrossMode.Mark);
    }

    public void ChangeModeDestroy()
    {
        destroyButton.color = destroyColor;
        markButton.color = defaultColor;
        ChangeModes(PicrossMode.Destroy);
        

    }
}
