
using Photon.Pun;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviourPunCallbacks
{
    public static CharacterSelectionManager instance;
    [SerializeField] CharacterPanel[] characterPanel;

    //[SerializeField] Color originalColor;
    public int currentCharacterPanelIndex = 0;
    //[SerializeField] int previousIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //if (PlayerPrefs.HasKey("PlayerCharacter"))
        //{
        //    currentCharacter = PlayerPrefs.GetInt("PlayerCharacter");
        //}

        //Store original color
        //originalColor = character[currentCharacter].GetComponent<UIEffect>().OutLiner.GetComponent<Image>().color;


        //Set value store in playprefs character to display
        //foreach (GameObject obj in characterPanel)
        //{
        //    obj.GetComponent<UIEvent>().OutLiner.SetActive(false);
        //}

        //character[currentCharacter].GetComponent<UIEffect>().OutLiner.SetActive(true);
        // Legacy button handling removed.
    }

    public void BeginHightlightCharacterPanel(int index)
    {
        //update all the clothes on every network player
        photonView.RPC(nameof(HightlightCharacterPanelRPC), RpcTarget.All, index, PhotonNetwork.NickName);

        //update all the jobnames on every network player
        //NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().BeginSetJobName(characterPanel[index].name);

    }



    [PunRPC]
    void HightlightCharacterPanelRPC(int number, string playerName)
    {
        PhotonNetwork.SendAllOutgoingCommands();

        this.currentCharacterPanelIndex = number;
        /**Save to playprefs and Set Color**/
        //PlayerPrefs.SetInt("PlayerCharacter", currentCharacter);
        foreach (CharacterPanel panel in characterPanel)
        {
            if (panel.GetComponent<CharacterPanel>().GetPlayerNameField() == playerName)
            {
                panel.HighlightPanel(false, true, "");
            }
        }
        characterPanel[currentCharacterPanelIndex].GetComponent<CharacterPanel>().HighlightPanel(true, false, playerName);

    }
}
