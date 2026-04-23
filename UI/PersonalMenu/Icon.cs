
using UnityEngine;
using UnityEngine.UI;
using LegacyXR.vXR.LegacyGear;

public class Icon : MonoBehaviour
{
    Color originalColor;

    public int personalMenuIndex;
    public PersonalMenu personalMenu;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = GetComponent<Image>().color;

        GetComponent<LegacyXRInputEvent>().onUpInteraction.unityEvent.AddListener(
            () => ChangePersonalMenuIndex(personalMenuIndex)
            );


    }


    public void EnableButton()
    {
        GetComponent<LegacyGear_Button>().interactive = LegacyXR.vXR.Interactive.Functional;
    }

    void ChangePersonalMenuIndex(int index)
    {
        personalMenu.SetCurrentMainpageIndex(index);
    }

    public void HightLightIcon(bool state)
    {
        GetComponent<UIEvent>().DisplayOutLiner(state);

        if (state == true)
        {
            GetComponent<LegacyGear_Button>().interactive = LegacyXR.vXR.Interactive.Disable;
        }
        else
        {
            GetComponent<LegacyGear_Button>().interactive = LegacyXR.vXR.Interactive.Functional;
        }


    }
}
