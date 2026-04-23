
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public GameObject[] InfoMenus;

    public void DisplayCurrentInfoMenu(int index)
    {
        foreach (GameObject obj in InfoMenus)
        {
            obj.gameObject.SetActive(false);
        }

        InfoMenus[index].gameObject.SetActive(true);
    }
}
