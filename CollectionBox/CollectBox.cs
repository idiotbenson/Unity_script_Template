
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CollectBox : MonoBehaviourPunCallbacks
{
    public Prominence prominence;

    public GameObject[] transportationPoints;
    public GameObject jobPrefab;
    public GameObject brushPrefab;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TransportableObject"))
        {
            bool isGrabbed = false;
            if (other.TryGetComponent(out GrabState grabState))
            {
                isGrabbed = grabState.IsGrabbed;
            }

            if (isGrabbed)
            {
                prominence.IncreaseHoleRadius();
            }

            if (!isGrabbed)
            {
                ////transfer this gameobject to other worker
                //other.transform.position = transportationPoints[0].transform.position;
                //other.transform.rotation = transportationPoints[0].transform.rotation;

                if (NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().GetJobName() != "Welder")
                {
                    Destroy(other.gameObject);
                    return;
                }


                //send rpc to spawn new object in other worker
                int textureIndex = other.gameObject.GetComponent<TransportableObject>().GetTextureIndex();
                int numberOfBrushes = other.gameObject.GetComponent<TransportableObject>().GetNumberOfBrushes();
                List<Vector3> AllBrushesPos = other.gameObject.GetComponent<TransportableObject>().GetAllBrushesPos();
                List<Quaternion> AllBrushesRot = other.gameObject.GetComponent<TransportableObject>().GetAllBrushesRot();
                List<Vector3> AllBrushesScale = other.gameObject.GetComponent<TransportableObject>().GetAllBrushesScale();


                PhotonView newjobPrefabPhotonView = PhotonNetwork.Instantiate(jobPrefab.name, transportationPoints[0].transform.position, transportationPoints[0].transform.rotation)
                                        .GetComponent<PhotonView>();

                //newjobPrefabPhotonView.transform.gameObject.GetComponent<TransportableObject>().photonView.RPC("SetPosRPC", RpcTarget.All, transportationPoints[0].transform.position);
                newjobPrefabPhotonView.transform.gameObject.GetComponent<TransportableObject>().photonView.RPC("SetTextureRPC", RpcTarget.All, textureIndex);



                for (int i = 0; i < numberOfBrushes; i++)
                {
                    PhotonView newBrushPhotonView = PhotonNetwork.Instantiate(brushPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<PhotonView>();
                    newBrushPhotonView.transform.gameObject.GetComponent<Brush>().photonView.RPC("SetParentRPC", RpcTarget.All, newjobPrefabPhotonView.ViewID);
                    //newBrushPhotonView.transform.SetParent(jobPrefabCache.GetComponent<TransportableObject>().textureSurfaceToChange.transform);
                    newBrushPhotonView.transform.gameObject.GetComponent<Brush>().photonView.RPC("SetLocalPosRPC", RpcTarget.All, AllBrushesPos[i]);
                    newBrushPhotonView.transform.gameObject.GetComponent<Brush>().photonView.RPC("SetLocalRotRPC", RpcTarget.All, AllBrushesRot[i]);
                    newBrushPhotonView.transform.gameObject.GetComponent<Brush>().photonView.RPC("SetLocalScaleRPC", RpcTarget.All, AllBrushesScale[i]);


                }


                Destroy(other.gameObject);

            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TransportableObject>() != null)
        {
            prominence.DecreaseHoleRadius();
        }
    }


}
