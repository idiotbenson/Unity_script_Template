
using UnityEngine;

[RequireComponent(typeof(LegacyXRInputEvent))]
public class ChangeTexture : MonoBehaviour
{
    LegacyXRInputEvent vInputEvent;

    public GameObject textureSurfaceToChange;
    public int textureIndex;



    // Start is called before the first frame update
    void Start()
    {
        vInputEvent = GetComponent<LegacyXRInputEvent>();

        vInputEvent.onUpInteraction.unityEvent.AddListener(() => { changingTexture(); });
    }

    public void changingTexture()
    {

        if (textureSurfaceToChange.GetComponent<TransportableObject>() != null)
        {
            textureSurfaceToChange.GetComponent<TransportableObject>().SetTextureIndex(this.textureIndex);
        }
    }
}
