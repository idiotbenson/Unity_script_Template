
using Photon.Pun;
using UnityEngine;

public class NetworkObjectSpawner : MonoBehaviourPunCallbacks
{
    public PrimitiveType typeToSpawn;
    public Material mat;

    GameObject newObject;


    public void SpawnObject()
    {
        var cube = GameObject.CreatePrimitive(typeToSpawn);
        cube.GetComponent<MeshRenderer>().material = mat;
        //cube.GetComponent<MeshRenderer>().material.mainTexture = Color.red;
        newObject = cube;

        //photonView.RPC(nameof(SpawnNetworkObject), RpcTarget.All, null);
        GameObject spawnObject = PhotonView.Instantiate(newObject, Vector3.one, Quaternion.identity);
        spawnObject.AddComponent<PhotonView>();
    }

    [PunRPC]
    public void SpawnNetworkObject()
    {
        GameObject spawnObject = Instantiate(newObject, Vector3.one, Quaternion.identity);
    }
}
