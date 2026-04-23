using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AlarmButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button triggerButton;

    [SerializeField] HurryUpNotice hurryUpNotice;
    [SerializeField] float waitTime;
    [SerializeField] string jobNameToNotice;

    public AudioClip ringSound;

    // Start is called before the first frame update
    void Start()
    {
        if (triggerButton == null)
        {
            triggerButton = GetComponent<Button>();
        }

        if (triggerButton != null)
        {
            triggerButton.onClick.AddListener(OnUpEvent);
        }
    }


    void OnUpEvent()
    {
        Debug.Log("PopUp");
        Utilities.GenerateSound(ringSound);

        //only team leader can press
        if (NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().GetJobName() == "TeamLeader")
        {
            NetworkedPlayer[] allPlayers = GameObject.FindObjectsOfType<NetworkedPlayer>();
            foreach (NetworkedPlayer player in allPlayers)
            {
                if (player.GetJobName() == jobNameToNotice)
                {
                    Player owner = player.GetComponent<PhotonView>().Owner;
                    photonView.RPC(nameof(AlarmPlayer), owner, null);
                }
            }

        }

    }

    [PunRPC]
    void AlarmPlayer()
    {
        hurryUpNotice.PopUp();

        if (triggerButton != null)
        {
            triggerButton.interactable = false;
        }
        StartCoroutine(canAlarmAgain());
    }


    IEnumerator canAlarmAgain()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        Color tempColor = Color.white;
        tempColor.a = 0.25f;
        GetComponent<Image>().color = tempColor;

        yield return new WaitForSecondsRealtime(waitTime);
        if (triggerButton != null)
        {
            triggerButton.interactable = true;
        }
    }

}
