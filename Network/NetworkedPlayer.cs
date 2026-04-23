
using Photon.Pun;
using UnityEngine;
using LegacyXR.vXR.LegacyGear;


public class NetworkedPlayer : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;

    [Header("Properties")]
    public NameUI playerNamePrefab;
    public Transform playerRoot;
    public Renderer playerMesh;
    public GameObject[] playerBodies;
    int currentBodyIndex;
    [SerializeField] string playerJobName;

    [Header("SoundControl")]
    public SoundIcon playerSoundIcon;
    GameObject playerSound;
    [SerializeField] private bool listenKeyboardMicToggle = true;
    [SerializeField] private bool useSharedInputConfig = true;
    [SerializeField] private KeyCode toggleMicKey = KeyCode.M;


    private void Awake()
    {
        Init();
    }

    void Start()
    {

        //PhotonVoiceView = GetComponent<PhotonVoiceView>();
        //NetworkedPlayer.LocalPlayerInstance.GetComponent<SoundControl>().Recorder.IsRecording = false;

    }

    void Init()
    {
        SpawnPlayerNameUI();
        SpawnPlayerSoundIcon();

        BeginChangeBody(0);
    }

    void SpawnPlayerNameUI()
    {
        if (photonView.IsMine == false && playerNamePrefab != null && playerRoot != null && playerMesh != null)
        {
            //Instantiate name
            GameObject playerName = Instantiate(playerNamePrefab.gameObject);
            playerName.GetComponent<NameUI>().SetTarget(playerRoot);
            playerName.GetComponent<NameUI>().playerName.text = photonView.Owner.NickName;
            playerName.GetComponent<NameUI>().SetPlayerRend(playerMesh);


        }
    }

    void SpawnPlayerSoundIcon()
    {
        if (playerSoundIcon == null || playerRoot == null || playerMesh == null)
        {
            return;
        }

        playerSound = Instantiate(playerSoundIcon.gameObject);
        playerSound.GetComponent<SoundIcon>().SetTarget(playerRoot);
        playerSound.GetComponent<SoundIcon>().SetPlayerRend(playerMesh);
    }

    #region Set player job name
    public void BeginSetJobName(string jobName)
    {
        photonView.RPC(nameof(SetJobNameRPC), RpcTarget.All, jobName);
    }

    [PunRPC]
    public void SetJobNameRPC(string jobName)
    {
        this.playerJobName = jobName;
    }

    public string GetJobName()
    {
        return this.playerJobName;
    }
    #endregion

    #region Display sound icon
    public void BeginSetSoundIcon(bool enableRecording)
    {
        photonView.RPC(nameof(SetSoundIconRPC), RpcTarget.All, enableRecording);
    }


    [PunRPC]
    public void SetSoundIconRPC(bool enableRecording)
    {
        if (playerSound == null)
        {
            return;
        }

        playerSound.GetComponent<SoundIcon>().soundIconImage.SetActive(enableRecording);
        Debug.Log("Set" + enableRecording);
    }
    #endregion

    #region Change body
    public void BeginChangeBody(int index)
    {
        //currentIndex = index;
        photonView.RPC(nameof(ChangeBodyRPC), RpcTarget.All, index);

        Debug.Log("Send Clothes Command");

    }

    [PunRPC]
    void ChangeBodyRPC(int index)
    {
        foreach (GameObject body in playerBodies)
        {
            body.gameObject.SetActive(false);
        }

        playerBodies[index].gameObject.SetActive(true);
        currentBodyIndex = index;

        Debug.Log("Update currentBodyIndex" + currentBodyIndex);
    }
    #endregion

    private void Update()
    {
        KeyCode activeKey = toggleMicKey;
        if (useSharedInputConfig && InputConfig.TryGet(out InputConfig inputConfig))
        {
            activeKey = inputConfig.micToggleKey;
        }

        if (listenKeyboardMicToggle && Input.GetKeyDown(activeKey))
        {
            TurnMicState();
        }
    }

    public void TurnMicState()
    {
        if (photonView.IsMine && LocalPlayerInstance != null && PhotonVoiceManager.Instance != null)
        {
            //NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().Recorder.RecordingEnabled = !Recorder.RecordingEnabled;
            //NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().BeginSetSoundIcon(
            //    !NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().playerSoundIcon.enabled);

            PhotonVoiceManager.Instance.EnableTransmit();
            NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().BeginSetSoundIcon(PhotonVoiceManager.Instance.GetTransmitState());

            PhotonVoiceManager.Instance.SetLocalMicState();
        }
    }
}
