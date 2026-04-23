using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviourPunCallbacks
{
    public Text message;
    public Animator anim;

    [PunRPC]
    public void SetMessage(string messageText)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            return;
        }

        anim.SetTrigger("Open");

        GetComponent<FollowHead>().SetInFrontOfHead();
        message.text = messageText;

        StartCoroutine(WaitDisappear());
    }

    IEnumerator WaitDisappear()
    {
        yield return Helper.GetWait(5);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Close"))
        {
            yield break;
        }

        anim.SetTrigger("Close");

    }

}
