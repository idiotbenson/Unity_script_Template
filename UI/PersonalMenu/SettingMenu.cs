
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject[] qualityButtons;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void Start()
    {

        for (int i = 0; i < qualityButtons.Length; i++)
        {
            int x = i;
            qualityButtons[i].GetComponent<LegacyXRInputEvent>().
               onUpInteraction.unityEvent.AddListener(
          () =>
          {
              SetQuality(x);
          }
           );
        }

    }
}
