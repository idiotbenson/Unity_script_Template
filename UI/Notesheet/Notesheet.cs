
using UnityEngine;
using UnityEngine.UI;
using LegacyXR.vXR.LegacyGear;
public class Notesheet : MonoBehaviour
{
    bool isOpened;
    public Text text;
    public LegacyGear_Button closeBtn;

    public GameObject backGround;

    private void Start()
    {
        closeBtn.GetComponent<LegacyXRInputEvent>().onUpInteraction.unityEvent.AddListener(
        () =>
        {
            SetIsOpen(false, null);
        }
         );
    }

    public void DisplayNotesheet(NotesheetData data)
    {
        if (isOpened)
        {
            SetIsOpen(false, data);
        }
        else
        {
            SetIsOpen(true, data);
        }
    }

    void Setup(NotesheetData data)
    {
        text.text = data.text;
    }

    public void SetIsOpen(bool state, NotesheetData data)
    {
        if (state)
        {
            GetComponent<FollowHead>().SetInFrontOfHead();
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                GetComponent<Animator>().SetTrigger("Open");
            }


            Setup(data);
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Close");

        }

        backGround.SetActive(state);

        isOpened = state;
    }

    public bool GetIsOpen()
    {
        return isOpened;
    }

}
