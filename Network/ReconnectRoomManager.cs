using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ReconnectRoomManager : MonoBehaviourPunCallbacks
{
    //public override void OnJoinedRoom()
    //{
    //    PhotonNetwork.LoadLevel("MainGame");
    //    Debug.Log("Joinned Room!");
    //}

    bool isConnecting;
    public ChangeSceneTrigger changeSceneTrigger;
    public string SceneName;

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("\nDisconnected because" + cause);
        Debug.Log("Ready to reconnect..");
        isConnecting = false;

        changeSceneTrigger.ChangeScene(SceneName);
        //if (isConnecting == false)
        //{
        //    PhotonNetwork.ConnectUsingSettings();        
        //}

    }

    public override void OnConnected()
    {
        Debug.Log("Connected");
        isConnecting = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        //PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room");

    }



}
