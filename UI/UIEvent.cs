using UnityEngine;
using UnityEngine.UI;
using LegacyXR.vXR.LegacyGear;


[RequireComponent(typeof(LegacyXRInputEvent))]
public class UIEvent : MonoBehaviour
{

    LegacyGear_Button LegacyGearButton;
    LegacyXRInputEvent vInputEvent;

    [SerializeField] Vector3 OriginalScale = new Vector3(1, 1, 1);
    [SerializeField] Color OriginalColor;
    Vector3 OriginalPos;


    [SerializeField] Vector3 ScalingFactor = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] Color SelectedColor = Color.yellow;
    [SerializeField] Vector3 SelectedPos;
    [SerializeField] Color PressingColor = Color.green;


    [Header("Optional")]
    [SerializeField] AudioClip clip;
    public GameObject OutLiner;
    public bool DisplayOutlinerWhenSelect;


    private void Start()
    {
        LegacyGearButton = GetComponent<LegacyGear_Button>();

        vInputEvent = GetComponent<LegacyXRInputEvent>();

        //OriginalScale = LegacyGearButton.gameObject.GetComponent<RectTransform>().localScale;
        OriginalScale = new Vector3(1, 1, 1);

        if (LegacyGearButton.gameObject.GetComponent<Image>() != null)
        {
            OriginalColor = LegacyGearButton.gameObject.GetComponent<Image>().color;
        }


        AddEffect();
    }

    void AddEffect()
    {
        //Select
        //Debug.LogError("Adding UI effect Listener");        
        vInputEvent.onSelectInteraction.unityEvent.AddListener(() =>
        {
            OnSelectionEvent();
        });


        //Deselect     
        vInputEvent.onDeSelectInteraction.unityEvent.AddListener(() =>
        {
            OnDeSelectionEvent();
        });


        //Down       
        vInputEvent.onDownInteraction.unityEvent.AddListener(() =>
        {
            OnDownEvent();
        });


        //Up
        vInputEvent.onUpInteraction.unityEvent.AddListener(() =>
        {
            OnUpEvent();
        });



    }

    void OnSelectionEvent()
    {
        LegacyGearButton.select.scaling = ScalingFactor;
        LegacyGearButton.select.color = SelectedColor;
        LegacyGearButton.select.movement = SelectedPos;

        if (DisplayOutlinerWhenSelect)
        {
            DisplayOutLiner(true);
        }

    }


    void OnDeSelectionEvent()
    {
        LegacyGearButton.select.scaling = OriginalScale;
        LegacyGearButton.select.color = OriginalColor;
        LegacyGearButton.select.movement = OriginalPos;

        if (DisplayOutlinerWhenSelect)
        {
            DisplayOutLiner(false);
        }
    }

    void OnDownEvent()
    {
        LegacyGearButton.click.scaling = ScalingFactor;
        LegacyGearButton.click.color = PressingColor;

        if (clip != null)
        {
            Utilities.GenerateSound(clip);

        }

    }

    void OnUpEvent()
    {

    }

    public void DisplayOutLiner(bool state)
    {
        if (OutLiner == null)
        {
            return;
        }
        OutLiner.SetActive(state);
    }
}
