
using UnityEngine;

public class FollowHead : MonoBehaviour
{
    [SerializeField] private Transform userTransform;
    [SerializeField] private Transform headTransform;

    public float xOffset = 0;
    public float yOffset = 0;
    public float zOffset = 0.5f;

    public bool DisplayWhenStart;

    // Start is called before the first frame update
    void Start()
    {
        if (userTransform == null)
        {
            var userGo = GameObject.Find("User");
            if (userGo != null)
            {
                userTransform = userGo.transform;
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

        if (DisplayWhenStart)
        {
            SetInFrontOfHead();
        }

    }

    public void SetInFrontOfHead()
    {
        if (headTransform == null || userTransform == null)
        {
            return;
        }

        transform.SetParent(headTransform);
        transform.localPosition = new Vector3(xOffset, yOffset, zOffset);
        transform.LookAt(headTransform);
        transform.SetParent(userTransform);

        //(Optional)Flip the UI one time to let player see
        transform.Rotate(new Vector3(0, 180, 0));

    }

    private void OnEnable()
    {
        SetInFrontOfHead();
        Debug.Log("Reset");
    }



}
