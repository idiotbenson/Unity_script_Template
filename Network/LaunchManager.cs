
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    [SerializeField] byte maxPlayersPerRoom = 4;
    bool isConnecting;
    public InputField playerName;
    public Text feedbackText;
    [SerializeField] string gameVersion = "1";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            //Debug.Log("HasUserNameRecord!");
            //Debug.Log(PlayerPrefs.GetString("PlayerName"));
            playerName.text = PlayerPrefs.GetString("PlayerName");

        }

#if UNITY_EDITOR

#endif

    }



    public void ConnectNetwork()
    {
        feedbackText.text = "";
        isConnecting = true;

        PhotonNetwork.NickName = playerName.text;
        if (PhotonNetwork.IsConnected)
        {
            feedbackText.text += "\nJoining Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            feedbackText.text += "\nConnecting...";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }


    }

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }

    public void ConnectSingle()
    {
        SceneManager.LoadScene("MainGame");
    }


    /***Network Callbacks***/
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            feedbackText.text += "\nOnConnectedToMaster...";
            PhotonNetwork.JoinRandomRoom();
        }


    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        feedbackText.text += "\nFailed to join random room...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        feedbackText.text += "\nDisconnected because" + cause;
        isConnecting = false;
    }

    public override void OnJoinedRoom()
    {
        feedbackText.text += "\nJoined Room with" + PhotonNetwork.CurrentRoom.PlayerCount + "players.";
        PhotonNetwork.LoadLevel("MainGame");
    }
}



