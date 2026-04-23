
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameCanvas : MonoBehaviourPunCallbacks
{

    [SerializeField] List<Text> PlayerNameFields = new List<Text>();
    [SerializeField] Color joinedNameFieldColor;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(ListAllPlayerName), 1f);
    }




    void ListAllPlayerName()
    {
        Debug.Log("List all the player");
        Player[] player = PhotonNetwork.PlayerList;

        //reset all to "Waiting..." first
        for (int i = 0; i < PlayerNameFields.Count; i++)
        {
            PlayerNameFields[i].text = "Waiting...";
            PlayerNameFields[i].color = Color.white;
            //PlayerNameFields[i].fontSize = 120;
        }

        for (int i = 0; i < player.Length; i++)
        {
            //Set each field to each player name
            PlayerNameFields[i].text = player[i].NickName;
            PlayerNameFields[i].color = joinedNameFieldColor;
            //PlayerNameFields[i].fontSize = 150;
        }



    }

    /**Network callbacks**/

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("PlayerEnterRoom ,list all the names");
        ListAllPlayerName();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("PlayerLeftRoom ,list all the names");
        ListAllPlayerName();
    }


}
