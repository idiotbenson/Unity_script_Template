
using UnityEngine;
using UnityEngine.UI;

public class ClearButton : MonoBehaviour
{
    [SerializeField] private Button triggerButton;

    public bool ClearWeldingDirt;
    public bool ClearBrush;

    private void Start()
    {
        if (triggerButton == null)
        {
            triggerButton = GetComponent<Button>();
        }

        if (triggerButton == null)
        {
            return;
        }

        if (ClearWeldingDirt)
        {
            triggerButton.onClick.AddListener(clearAllWeldingDirt);
        }

        if (ClearBrush)
        {
            triggerButton.onClick.AddListener(clearAllBrush);
        }

    }

    public void clearAllWeldingDirt()
    {
        WeldingGamePlayControl.instance.clearAllWeldingDirt();
    }

    public void clearAllBrush()
    {
        Brush[] allBrush = FindObjectsOfType<Brush>();

        foreach (Brush brush in allBrush)
        {
            Destroy(brush.gameObject);
        }
    }
}
