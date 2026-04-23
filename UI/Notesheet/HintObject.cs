
using UnityEngine;


public class HintObject : MonoBehaviour
{
    public NotesheetData data;
    public Notesheet noteSheet;

    public AudioClip OpenSound;

    private void Start()
    {
        GetComponent<LegacyXRInputEvent>().
              onUpInteraction.unityEvent.AddListener(
         () =>
         {
             noteSheet.SetIsOpen(true, data);
             if (OpenSound != null)
             {
                 Utilities.GenerateSound(OpenSound);
             }

         }
          );
    }


}
