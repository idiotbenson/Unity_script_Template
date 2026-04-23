
using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{

    [SerializeField]
    float spawnDistance;

    bool fliped;

    void Start()
    {


        //Cam = Camera.main;
    }


    void Update()
    {
        SetPosAndRot(this.gameObject);
    }

    void SetPosAndRot(GameObject obj)
    {
        //Calculate position to spawn
        Vector3 playerPos = Helper.Camera.transform.position;
        Vector3 playerDirection = Helper.Camera.transform.forward;
        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        //Set position and rotation
        obj.transform.position = spawnPos;
        obj.transform.LookAt(Helper.Camera.transform);

        //(Optional)Flip the UI one time to let player see
        if (fliped == false)
        {
            float tempPosX = obj.transform.localScale.x;
            tempPosX *= -1;
            obj.transform.localScale = new Vector3(tempPosX, obj.transform.localScale.y, obj.transform.localScale.z);

            fliped = true;
        }




    }
}
