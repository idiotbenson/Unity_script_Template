
using Photon.Pun;
using UnityEngine;

public class MediaNetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField] private MonoBehaviour repairIntroPlayer;

    public void BeginPlayMedia()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PlayMedia", RpcTarget.All, null);
        }
    }

    [PunRPC]
    void PlayMedia()
    {
        if (repairIntroPlayer == null)
        {
            return;
        }

        // Works with any component that implements PlayPause().
        repairIntroPlayer.SendMessage("PlayPause", SendMessageOptions.DontRequireReceiver);
    }
}
