
using UnityEngine;

public class ScreenCameraControl : MonoBehaviour
{
    [SerializeField]
    RenderTexture[] renderTexture;

    public void ChangeCameraRenderTexture()
    {
        NetworkedPlayer[] AllPlayers = GameObject.FindObjectsOfType<NetworkedPlayer>();

        for (int i = 0; i < AllPlayers.Length; i++)
        {
            switch (AllPlayers[i].GetJobName())
            {
                case "TeamLeader":
                    break;

                case "Illustrator":
                    AllPlayers[i].gameObject.GetComponentInChildren<Camera>().targetTexture = renderTexture[0];
                    break;

                case "Welder":
                    AllPlayers[i].gameObject.GetComponentInChildren<Camera>().targetTexture = renderTexture[1];
                    break;

                case "Sculptor":
                    AllPlayers[i].gameObject.GetComponentInChildren<Camera>().targetTexture = renderTexture[2];
                    break;

                case "Carpenter":
                    AllPlayers[i].gameObject.GetComponentInChildren<Camera>().targetTexture = renderTexture[3];
                    break;

                case "Painter":
                    AllPlayers[i].gameObject.GetComponentInChildren<Camera>().targetTexture = renderTexture[4];
                    break;

            }

        }

    }
}
