
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] GameObject user;

    Vector3 oriRot;
    public int offset_Y;

    public GameObject target;
    public bool lookAtUser = true;

    // Start is called before the first frame update
    void Start()
    {
        oriRot = transform.eulerAngles;

        if (user == null)
        {
            user = GameObject.Find("User");
        }

        SetTarget();


    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        transform.LookAt(target.transform.position);
        transform.eulerAngles = new Vector3(oriRot.x, transform.eulerAngles.y + offset_Y, oriRot.z);
    }

    void SetTarget()
    {

        if (lookAtUser)
        {
            target = user;
        }
        else
        {
            target = null;
        }


    }
}
