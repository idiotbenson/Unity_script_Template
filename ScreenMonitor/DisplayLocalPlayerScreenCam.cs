
using Photon.Pun;
using UnityEngine;


public class DisplayLocalPlayerScreenCam : MonoBehaviourPunCallbacks
{
    [SerializeField] int layerToMask;
    [SerializeField] RenderTexture defaultTexture;
    bool FollowLocalPlayer;

    // Start is called before the first frame update
    void Start()
    {
        #region Hide the local player model
        int cullingMask = GetComponent<Camera>().cullingMask;
        GetComponent<Camera>().cullingMask = cullingMask & ~(1 << layerToMask);
        #endregion

        ///Set the default render texture
        GetComponent<Camera>().targetTexture = defaultTexture;

        #region Set camera to follow and enable following
        if (photonView.IsMine)
        {
            FollowLocalPlayer = true;
        }

        if (FollowLocalPlayer)
        {
            // Reserved for future local-player binding if needed.
        }
    }
    #endregion



    // Update is called once per frame
    void Update()
    {
        if (FollowLocalPlayer == true)
        {
            transform.rotation = Helper.Camera.transform.rotation;
        }
    }
}
