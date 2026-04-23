
using UnityEngine;


public class IconPanel : MonoBehaviour
{
    public Icon[] icons;


    public void DisplayCurrentIcon(int index)
    {
        foreach (Icon icon in icons)
        {
            icon.HightLightIcon(false);
        }

        icons[index].HightLightIcon(true);

    }

    public void EnableAllIcon()
    {
        foreach (Icon icon in icons)
        {
            icon.EnableButton();
        }
    }
}
