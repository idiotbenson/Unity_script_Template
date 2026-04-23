
using UnityEngine;
using UnityEngine.UI;


public class RoleButton : MonoBehaviour
{
    public GameObject[] RoleButtons;
    public RoleDetail roleDetail;

    private void Start()
    {
        for (int i = 0; i < RoleButtons.Length; i++)
        {
            int x = i;
            var button = RoleButtons[i].GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => SwitchRoleDetail(x));
            }
        }
    }

    public void DisplayUI(bool state)
    {
        this.gameObject.SetActive(state);
    }

    public void SwitchRoleDetail(int index)
    {
        DisplayUI(false);

        roleDetail.DisplayRoleDetail(index);
    }






}
