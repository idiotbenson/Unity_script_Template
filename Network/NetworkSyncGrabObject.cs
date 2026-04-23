using Photon.Pun;
using UnityEngine;
public class NetworkSyncGrabObject : MonoBehaviourPunCallbacks
{
    public bool picked = false;
    LegacyXRInputEvent vInputEvent;
    void Start()
    {
        vInputEvent = GetComponent<LegacyXRInputEvent>();

        vInputEvent.onGrabInteraction.unityEvent.AddListener(() =>
        {
            onGrabEvent();
        });



        vInputEvent.onDropInteraction.unityEvent.AddListener(() =>
        {
            onDropEvent();
        });


    }

    public void onGrabEvent()
    {
        if (picked == false)
        {
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            picked = true;
            Debug.Log("Grabbing");
            if (TryGetComponent(out ArchObject archObject))
            {
                photonView.RPC("SetRigidBodyRPC", RpcTarget.All, true, false);
            }

            if (TryGetComponent(out HighlightedObject hightLightedObject))
            {
                hightLightedObject.GetComponent<PhotonView>().RPC("SetHightlightedRPC", RpcTarget.All, true);
            }

            GetComponent<PhotonView>().RPC("SetLayerRPC", RpcTarget.Others, 0);
            GetComponent<ArchObject>().PlayParticle(true);
        }

    }

    public void onDropEvent()
    {
        picked = false;
        Debug.Log("Drop");
        if (GetComponent<PhotonView>().Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
        {
            if (TryGetComponent(out ArchObject archObject))
            {
                archObject.SetRigidBodyRPC(false, true);
            }



            if (TryGetComponent(out HighlightedObject hightLightedObject))
            {
                if (archObject.occupying == false)
                {
                    hightLightedObject.GetComponent<PhotonView>().RPC("SetHightlightedRPC", RpcTarget.All, false);
                    GetComponent<PhotonView>().RPC("SetLayerRPC", RpcTarget.Others, 28);
                    GetComponent<ArchObject>().PlayParticle(false);
                }

            }
        }



    }



}
