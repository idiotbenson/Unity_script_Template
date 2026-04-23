
using UnityEngine;

public class FollowLegacyGearUser : MonoBehaviour
{
    [SerializeField] Transform actualTransform;
    [SerializeField] Transform headTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (actualTransform == null)
        {
            var actualGo = GameObject.Find("Actual");
            if (actualGo != null)
            {
                actualTransform = actualGo.transform;
            }
        }

        if (headTransform == null)
        {
            var headGo = GameObject.Find("Head");
            if (headGo != null)
            {
                headTransform = headGo.transform;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (headTransform == null || actualTransform == null)
        {
            return;
        }

        transform.position = new Vector3(headTransform.position.x, transform.position.y, headTransform.position.z);
        transform.rotation = actualTransform.rotation;
    }
}
