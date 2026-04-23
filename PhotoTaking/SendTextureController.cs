using Photon.Pun;
using System.Collections;
using UnityEngine;


public class SendTextureController : MonoBehaviourPunCallbacks
{
    public SnappingArea snappingArea;

    public GameObject leaderScreen;
    public MessageBox messageBox;

    //public List<GameObject> capturedPict = new List<GameObject>();
    public Texture2D capTexture;
    public Texture2D receivedTexture;

    public byte[] receivedBytes;
    public byte[] serializedBytes;


    void Update()
    {
        //capturedPict.Clear();

        ////Find all captured image
        //GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        //for (int i = 0; i < allGameObjects.Length; i++)
        //{
        //    if (allGameObjects[i].tag == "Captured")
        //    {                  
        //        //add to first in list
        //        capturedPict.Add(allGameObjects[i]);
        //    }                        
        //}


        ////cache the first capture image 's texture in the list
        //capTexture = (Texture2D)capturedPict[0].GetComponent<Renderer>().material.mainTexture;


    }




    public void SendTexture()
    {
        if (snappingArea.snappedPhoto == null)
        {
            Debug.Log("No photo, return");
            return;
        }

        messageBox.SetMessage("Sent a photo");

        capTexture = (Texture2D)snappingArea.snappedPhoto.GetComponent<Renderer>().material.mainTexture;


        //get pixels of the cap texture
        StartCoroutine(GetRenderTexturePixel(capTexture));

        //set rpc to change texture in leader screen
        photonView.RPC(nameof(SendTextures), RpcTarget.All, serializedBytes);
    }

    public void ClearTexture()
    {
        Destroy(snappingArea.snappedPhoto);
    }

    IEnumerator GetRenderTexturePixel(Texture2D textureToSerialize)
    {
        serializedBytes = textureToSerialize.EncodeToPNG();
        yield return new WaitForEndOfFrame();
    }


    [PunRPC]
    void SendTextures(byte[] receivedByte)
    {
        receivedTexture = null;
        receivedTexture = new Texture2D(1, 1);
        receivedTexture.LoadImage(receivedByte);
        receivedTexture.Apply();

        leaderScreen.GetComponent<MeshRenderer>().material.mainTexture = receivedTexture;

        //else if (GetComponent<Image>())
        //{
        //    GetComponent<Image>().sprite = (Sprite)receivedTexture;
        //}

    }


}
