using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using LegacyXR.vXR.LegacyGear;

public class GameMonitor : MonoBehaviourPunCallbacks
{
    LegacyGear_User LegacyGearUser;
    [SerializeField] GameObject UserControl;
    [SerializeField] ScreenCameraControl cameraControl;

    /*UI*/
    [SerializeField] GameObject StartGameCanvas;
    [SerializeField] GameObject WaitToStartCanvas;
    [SerializeField] GameObject LobbyCanvas;
    public GameObject[] countDownItems;
    public GameObject[] WayPoints;

    [SerializeField] GameObject IntroMediaPlayer;
    [SerializeField] GameObject ChooseCharacterCanvas;
    [SerializeField] GameObject StartWorkCanvas;

    public GameObject[] playerCharacterPrefabs;
    public Transform[] LobbySpawnPos;
    public Transform[] JobSpawnPos;

    GameObject pcharacter;

    /*Status*/
    bool gaming;

    /*CharacterData*/
    [SerializeField] int playerCharacterNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        LegacyGearUser = FindObjectOfType<LegacyGear_User>();

        //Initialization for single gameplay
        foreach (GameObject item in countDownItems)
        {
            item.SetActive(false);
        }


        StartGameCanvas.SetActive(false);   //turn off start game button
        WaitToStartCanvas.SetActive(false); //turn off wait game button
        LobbyCanvas.SetActive(false);

        IntroMediaPlayer.SetActive(false);
        ChooseCharacterCanvas.SetActive(false);
        StartWorkCanvas.SetActive(false);
        foreach (GameObject waypoint in WayPoints)
        {
            waypoint.SetActive(false);
        }

        //StartGameBtn.SetActive(false);    //set false if single player
        //ChooseCustomeCanvas.SetActive(false);  //set false if single player

        //if (PlayerPrefs.HasKey("PlayerCharacter"))
        //{
        //    playerCharacterNumber = PlayerPrefs.GetInt("PlayerCharacter");
        //}


        //Initialization for multiplayer gameplay
        if (PhotonNetwork.IsConnected)
        {

            if (NetworkedPlayer.LocalPlayerInstance == null)  //if we are first player to come
            {
                NetworkPlayerInitialization();
            }

            if (PhotonNetwork.IsMasterClient)   //first client connected and join the room
            {
                MasterClientNetworkObjectInitialization();

            }
            else
            {
                NormalClientNetworkObjectInitialization();
            }
        }
        //else
        //{
        //pcharacter = Instantiate(playerCharacterPrefabs[playerCharacterNumber]);
        //pcharacter.transform.position = startPos;
        //pcharacter.transform.rotation = startRot;

        /*Spawn AI car if single*/

        //    StartGame();  //automatically start game if single player
        //}

    }

    public void BeginGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(StartGameRPC), RpcTarget.All, null);
        }
    }

    [PunRPC]
    void StartGameRPC()
    {
        Debug.Log("StartGame");

        StartCoroutine(PlayCountDown());   //display countdown


        //GameObject.FindObjectOfType<MediaPlayer>().gameObject.SetActive(false);  //turn off media player
        //ChooseCharacterCanvas.gameObject.SetActive(false);   //turn off chooseCharacterCanvas
    }

    public void Beginwork()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(StartWorkRPC), RpcTarget.All, null);
        }
    }

    [PunRPC]
    void StartWorkRPC()
    {
        Debug.Log("StartWork");

        IntroMediaPlayer.SetActive(false);
        ChooseCharacterCanvas.SetActive(false);
        StartWorkCanvas.SetActive(false);
        foreach (GameObject waypoint in WayPoints)
        {
            waypoint.SetActive(false);
        }

        switch (NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().GetJobName())
        {
            case "TeamLeader":
                //Set rotation and position(fade effect)
                UserControl.GetComponent<UserRotation>().SetTargetPoint(JobSpawnPos[0].transform.position);
                LegacyGearUser.GetComponent<LegacyGear_User>().Transform(JobSpawnPos[0].transform, LegacyXR.vXR.TransitionMethod.Fade, false);

                //Start monitor screen
                cameraControl.ChangeCameraRenderTexture();
                break;

            case "Illustrator":
                //Set rotation and position(fade effect)
                UserControl.GetComponent<UserRotation>().SetTargetPoint(JobSpawnPos[1].transform.position);
                LegacyGearUser.GetComponent<LegacyGear_User>().Transform(JobSpawnPos[1].transform, LegacyXR.vXR.TransitionMethod.Fade, false);

                cameraControl.ChangeCameraRenderTexture();
                break;

            case "Welder":
                //Set rotation and position(fade effect)
                UserControl.GetComponent<UserRotation>().SetTargetPoint(JobSpawnPos[2].transform.position);
                LegacyGearUser.GetComponent<LegacyGear_User>().Transform(JobSpawnPos[2].transform, LegacyXR.vXR.TransitionMethod.Fade, false);

                cameraControl.ChangeCameraRenderTexture();
                break;

            case "Sculptor":
                //Set rotation and position(fade effect)
                UserControl.GetComponent<UserRotation>().SetTargetPoint(JobSpawnPos[3].transform.position);
                LegacyGearUser.GetComponent<LegacyGear_User>().Transform(JobSpawnPos[3].transform, LegacyXR.vXR.TransitionMethod.Fade, false);

                cameraControl.ChangeCameraRenderTexture();
                break;

            case "Carpenter":
                //Set rotation and position(fade effect)
                UserControl.GetComponent<UserRotation>().SetTargetPoint(JobSpawnPos[4].transform.position);
                LegacyGearUser.GetComponent<LegacyGear_User>().Transform(JobSpawnPos[4].transform, LegacyXR.vXR.TransitionMethod.Fade, false);

                cameraControl.ChangeCameraRenderTexture();
                break;

            case "Painter":
                //Set rotation and position(fade effect)
                UserControl.GetComponent<UserRotation>().SetTargetPoint(JobSpawnPos[5].transform.position);
                LegacyGearUser.GetComponent<LegacyGear_User>().Transform(JobSpawnPos[5].transform, LegacyXR.vXR.TransitionMethod.Fade, false);

                cameraControl.ChangeCameraRenderTexture();
                break;
        }






        //GameObject.FindObjectOfType<MediaPlayer>().gameObject.SetActive(false);  //turn off media player
        //ChooseCharacterCanvas.gameObject.SetActive(false);   //turn off chooseCharacterCanvas
    }

    IEnumerator PlayCountDown()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject item in countDownItems)
        {
            item.SetActive(true);
            yield return new WaitForSeconds(1);
            item.SetActive(false);
        }

        StartGameCanvas.SetActive(false);   //turn off start game button
        WaitToStartCanvas.SetActive(false); //turn off wait game button
        LobbyCanvas.SetActive(false);

        IntroMediaPlayer.SetActive(true);
        ChooseCharacterCanvas.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            StartWorkCanvas.SetActive(true);
        }

        foreach (GameObject waypoint in WayPoints)
        {
            if (waypoint != null)
            {
                waypoint.SetActive(true);
            }

        }

        gaming = true;
    }

    void NetworkPlayerInitialization()
    {
        //set start spawn position
        int randomStartPos = Random.Range(0, LobbySpawnPos.Length);
        Vector3 startPos = LobbySpawnPos[randomStartPos].position;
        Quaternion startRot = LobbySpawnPos[randomStartPos].rotation;

        startPos = LobbySpawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].position;
        startRot = LobbySpawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation;

        //Debug.Log(playerCharacterNumber);
        pcharacter = PhotonNetwork.Instantiate(playerCharacterPrefabs[playerCharacterNumber].name, startPos, startRot, 0);

        //set LegacyGear user position equal to new spawned object
        LegacyGearUser.GetComponent<RectTransform>().position = pcharacter.transform.position;
        //enable follow LegacyGearUser script
        pcharacter.GetComponentInChildren<FollowLegacyGearUser>().enabled = true;

        //enable all animators in all character clothes
        SetAnimator[] anims = pcharacter.GetComponentsInChildren<SetAnimator>(true);
        foreach (SetAnimator anim in anims)
        {
            anim.enabled = true;
        }


        //set it to be invisible by local player
        Transform[] children = pcharacter.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in children)
        {
            //change model to be invisible
            if (t.gameObject.layer == LayerMask.NameToLayer("PlayerModel"))
            {
                t.gameObject.layer = LayerMask.NameToLayer("LocalPlayerSkin");
            }

        }

        //set culling mask
        Camera[] allCamera = GameObject.FindObjectsOfType<Camera>();


        foreach (Camera camera in allCamera)
        {
            if (camera.tag == "MainCamera")
            {
                //Debug.Log("This is" + camera.gameObject.name + "that has maincamera tag");
                camera.cullingMask &= ~(1 << 10);
            }
        }


        NetworkedPlayer.LocalPlayerInstance = pcharacter;
    }

    void MasterClientNetworkObjectInitialization()
    {
        StartGameCanvas.SetActive(true);
        WaitToStartCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);


    }

    void NormalClientNetworkObjectInitialization()
    {
        StartGameCanvas.SetActive(false);
        WaitToStartCanvas.SetActive(true);
        LobbyCanvas.SetActive(true);


    }

    /**Network callbacks**/
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)   //first client connected and join the room
        {
            MasterClientNetworkObjectInitialization();

        }
        else
        {
            NormalClientNetworkObjectInitialization();
        }
    }



}
