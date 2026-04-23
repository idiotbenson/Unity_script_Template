using UnityEngine;
using UnityEngine.Events;

public class Back : MonoBehaviour
{
    public UnityEvent BackEvent;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LegacyXRInputEvent>().onUpInteraction.unityEvent.AddListener(
            () => BackEvent.Invoke()
            );
    }


}
