
using UnityEngine;

public class LookAtUser : MonoBehaviour
{

    [SerializeField] bool canFlip;
    [SerializeField] bool YaxisOnly;
    bool fliped;

    Quaternion rot;

    private void Start()
    {
        rot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        transform.LookAt(Helper.Camera.transform);
        if (YaxisOnly)
        {
            transform.rotation = Quaternion.Euler(new Vector3(rot.eulerAngles.x, transform.rotation.eulerAngles.y, rot.eulerAngles.z));
        }

    }

    void Flip()
    {
        if (canFlip == false)
        {
            return;
        }

        if (fliped == true)
        {
            return;
        }

        float tempX = transform.localScale.x;
        tempX *= -1;
        transform.localScale = new Vector3(tempX, transform.localScale.y, transform.localScale.z);
        fliped = true;
    }
}
