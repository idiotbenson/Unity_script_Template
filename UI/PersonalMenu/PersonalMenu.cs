
using UnityEngine;
using LegacyXR.vXR.LegacyGear;

public class PersonalMenu : MonoBehaviour
{
    bool isOpened;
    int currentMainPageIndex;

    public RoleButton roleButton;
    public RoleDetail roleDetail;
    public InfoPanel InfoPanel;
    public IconPanel IconPanel;

    public void SetIsOpen(bool state)
    {
        if (state)
        {
            GetComponent<FollowHead>().SetInFrontOfHead();
            GetComponent<Animator>().SetTrigger("Open");

            roleButton.DisplayUI(true);
            roleDetail.DisplayUI(false);
            IconPanel.EnableAllIcon();
            SetCurrentMainpageIndex(0);

        }
        else
        {
            GetComponent<Animator>().SetTrigger("Close");
        }

        isOpened = state;
    }

    public bool GetIsOpen()
    {
        return isOpened;
    }


    public int GetCurrentMainPageIndex()
    {
        return currentMainPageIndex;
    }

    public void SetCurrentMainpageIndex(int index)
    {
        currentMainPageIndex = index;
        SetMainPage(index);
    }

    void SetMainPage(int index)
    {
        currentMainPageIndex = index;
        InfoPanel.DisplayCurrentInfoMenu(currentMainPageIndex);
        IconPanel.DisplayCurrentIcon(currentMainPageIndex);
    }


    private void Update()
    {
        if (LegacyGear.Cmd.Received("OpenPersonalMenu"))
        {
            if (isOpened)
            {
                SetIsOpen(false);
            }
            else
            {
                SetIsOpen(true);
            }


        }

    }
}
