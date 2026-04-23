using System.Collections;
using UnityEngine;

public class HurryUpNotice : MonoBehaviour
{
    [SerializeField] float disappearTime;

    public AudioClip hurrySound;

    public void PopUp()
    {
        Debug.Log("HurryUp");

        gameObject.GetComponent<Animator>().SetTrigger("Open");
        StartCoroutine(endPopUp());

        Utilities.GenerateSound(hurrySound);
    }

    IEnumerator endPopUp()
    {
        yield return new WaitForSecondsRealtime(disappearTime);
        gameObject.GetComponent<Animator>().SetTrigger("Idle");
    }
}
