
using UnityEngine;

public class HandUI : MonoBehaviour
{
    GameObject Hand;
    GameObject Controller;

    void Start()
    {
        Hand = GameObject.Find("Hand");
        Controller = GameObject.Find("Controller");



    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Hand.transform.position;
        //transform.position = Controller.transform.position;

#if UNITY_EDITOR

        transform.position = new Vector3(Hand.transform.position.x, Hand.transform.position.y + 0.15f,
                                          Hand.transform.position.z);
#endif

        //transform.rotation = Quaternion.Euler(new Vector3(Hand.transform.rotation.eulerAngles.x,
        // Hand.transform.rotation.eulerAngles.y, Hand.transform.rotation.eulerAngles.z));

        transform.rotation = Quaternion.Euler(new Vector3(Controller.transform.rotation.eulerAngles.x,
                                               Controller.transform.rotation.eulerAngles.y, Controller.transform.rotation.eulerAngles.z));



    }
}
