
using UnityEngine;

public class RoleDetail : MonoBehaviour
{
    public GameObject[] RoleDetails;

    public void DisplayUI(bool state)
    {
        this.gameObject.SetActive(state);

    }

    public void DisplayRoleDetail(int index)
    {
        DisplayUI(true);

        foreach (GameObject roledetail in RoleDetails)
        {
            roledetail.SetActive(false);
        }

        RoleDetails[index].gameObject.SetActive(true);
    }
}
