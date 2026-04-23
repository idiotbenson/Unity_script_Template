using UnityEngine;
using LegacyXR.vXR.LegacyGear;

public class LegacyXRInputEvent : MonoBehaviour
{
    public LegacyXR.vXR.Interaction onSelectInteraction = new LegacyXR.vXR.Interaction();
    public LegacyXR.vXR.Interaction onDeSelectInteraction = new LegacyXR.vXR.Interaction();
    public LegacyXR.vXR.Interaction onDownInteraction = new LegacyXR.vXR.Interaction();
    public LegacyXR.vXR.Interaction onUpInteraction = new LegacyXR.vXR.Interaction();
    public LegacyXR.vXR.Interaction onGrabInteraction = new LegacyXR.vXR.Interaction();
    public LegacyXR.vXR.Interaction onDropInteraction = new LegacyXR.vXR.Interaction();


    public bool LegacyGearButtonInput;
    public bool LegacyGearInterableInput;

    LegacyGear_Button LegacyGearButton;
    LegacyGear_Interactables LegacyGearInteractables;
    private void Awake()
    {
        init();
    }


    public void init()
    {
        SetUpInteractionType();
        HandleLegacyXRInput();
    }
    void SetUpInteractionType()
    {
        onSelectInteraction.interactionType = LegacyXR.vXR.InteractionType.Select;
        onDeSelectInteraction.interactionType = LegacyXR.vXR.InteractionType.Deselect;
        onDownInteraction.interactionType = LegacyXR.vXR.InteractionType.Down;
        onUpInteraction.interactionType = LegacyXR.vXR.InteractionType.Up;
        onGrabInteraction.interactionType = LegacyXR.vXR.InteractionType.Grab;
        onDropInteraction.interactionType = LegacyXR.vXR.InteractionType.Drop;
    }

    void HandleLegacyXRInput()
    {
        if (LegacyGearButtonInput)
        {
            LegacyGearButton = GetComponent<LegacyGear_Button>();

            LegacyGearButton.interactions.Add(onSelectInteraction);
            LegacyGearButton.interactions.Add(onDeSelectInteraction);
            LegacyGearButton.interactions.Add(onDownInteraction);
            LegacyGearButton.interactions.Add(onUpInteraction);
            LegacyGearButton.interactions.Add(onGrabInteraction);
            LegacyGearButton.interactions.Add(onDropInteraction);
        }
        else if (LegacyGearInterableInput)
        {
            LegacyGearInteractables = GetComponent<LegacyGear_Interactables>();

            LegacyGearInteractables.interactions.Add(onSelectInteraction);
            LegacyGearInteractables.interactions.Add(onDeSelectInteraction);
            LegacyGearInteractables.interactions.Add(onDownInteraction);
            LegacyGearInteractables.interactions.Add(onUpInteraction);
            LegacyGearInteractables.interactions.Add(onGrabInteraction);
            LegacyGearInteractables.interactions.Add(onDropInteraction);
        }

    }

}
