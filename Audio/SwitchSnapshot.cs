using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SwitchSnapshot : MonoBehaviour
{
    public enum SurfaceType
    {
        Metal,
        Wood,
        Brick
    }

    private enum EnvironmentType
    {
        Unknown,
        Indoor,
        Outdoor
    }

    private EnvironmentType currentEnvironment = EnvironmentType.Unknown;
    private float currentEnvironmentEchoBaseline;
    private bool hasEnvironmentEchoBaseline;

    [FormerlySerializedAs("audiomixer")]
    [SerializeField] private AudioMixer audioMixer;

    [FormerlySerializedAs("outsideSnapshot")]
    [SerializeField] private AudioMixerSnapshot outsideSnapshot;
    [FormerlySerializedAs("insideSnapshot")]
    [SerializeField] private AudioMixerSnapshot insideSnapshot;
    [SerializeField] private string echoParameter = "Delay_of_Echo";

    [SerializeField] private float metalEchoValue = 100f;
    [SerializeField] private float woodEchoDelta = -50f;
    [SerializeField] private float brickEchoValue = 50f;

    [FormerlySerializedAs("switchTime")]
    [SerializeField] private float switchTime = 0.2f;

    private void OnValidate()
    {
        switchTime = Mathf.Max(0f, switchTime);
    }

    // Keep typo-compatible API for existing event bindings.
    public void SwitchOutDoor()
    {
        SwitchOutdoor();
    }

    // Keep typo-compatible API for existing event bindings.
    public void SwitchInDoor()
    {
        SwitchIndoor();
    }

    public void SwitchOutdoor()
    {
        SwitchToSnapshot(outsideSnapshot, EnvironmentType.Outdoor, "Outside");
    }

    public void SwitchIndoor()
    {
        SwitchToSnapshot(insideSnapshot, EnvironmentType.Indoor, "Inside");
    }

    public void Metal()
    {
        ApplySurface(SurfaceType.Metal);
    }

    public void Wood()
    {
        ApplySurface(SurfaceType.Wood);
    }

    public void Brick()
    {
        ApplySurface(SurfaceType.Brick);
    }

    public void ApplySurface(SurfaceType surface)
    {
        if (!CanUpdateEcho())
        {
            return;
        }

        switch (surface)
        {
            case SurfaceType.Metal:
                SetEcho(metalEchoValue, "Metal");
                break;
            case SurfaceType.Wood:
                ApplyWoodEcho();
                break;
            case SurfaceType.Brick:
                SetEcho(brickEchoValue, "Brick");
                break;
            default:
                Debug.LogWarning($"Unsupported surface type: {surface}", this);
                break;
        }
    }

    // For Animator Event and Timeline string payloads.
    public void ApplySurface(string surfaceName)
    {
        if (string.IsNullOrWhiteSpace(surfaceName))
        {
            Debug.LogWarning("Surface name is empty.", this);
            return;
        }

        if (!System.Enum.TryParse(surfaceName, true, out SurfaceType surface))
        {
            Debug.LogWarning($"Unknown surface name: '{surfaceName}'.", this);
            return;
        }

        ApplySurface(surface);
    }

    // For UI Button int payloads (0:Metal, 1:Wood, 2:Brick).
    public void ApplySurface(int surfaceIndex)
    {
        if (!System.Enum.IsDefined(typeof(SurfaceType), surfaceIndex))
        {
            Debug.LogWarning($"Invalid surface index: {surfaceIndex}", this);
            return;
        }

        ApplySurface((SurfaceType)surfaceIndex);
    }

    private bool CanUpdateEcho()
    {
        if (audioMixer == null)
        {
            Debug.LogWarning("Audio mixer is not assigned.", this);
            return false;
        }

        if (string.IsNullOrWhiteSpace(echoParameter))
        {
            Debug.LogWarning("Echo parameter name is empty.", this);
            return false;
        }

        if (currentEnvironment == EnvironmentType.Unknown)
        {
            Debug.LogWarning("Environment has not been selected yet. Call SwitchInDoor or SwitchOutDoor first.", this);
            return false;
        }

        return true;
    }

    private void SwitchToSnapshot(AudioMixerSnapshot snapshot, EnvironmentType environment, string environmentLabel)
    {
        if (snapshot == null)
        {
            Debug.LogWarning($"{environmentLabel} snapshot is not assigned.", this);
            return;
        }

        snapshot.TransitionTo(switchTime);
        currentEnvironment = environment;
        CacheEnvironmentEchoBaseline();
        Debug.Log(environmentLabel);
    }

    private void SetEcho(float value, string debugLabel)
    {
        if (!audioMixer.SetFloat(echoParameter, value))
        {
            Debug.LogWarning($"Unable to set mixer parameter '{echoParameter}' to value {value}.", this);
            return;
        }

        Debug.Log(debugLabel);
    }

    private void ApplyWoodEcho()
    {
        if (hasEnvironmentEchoBaseline)
        {
            float newEchoValue = currentEnvironmentEchoBaseline + woodEchoDelta;
            SetEcho(newEchoValue, "Wood");
            return;
        }

        if (!audioMixer.GetFloat(echoParameter, out float value))
        {
            Debug.LogWarning($"Unable to read mixer parameter '{echoParameter}'.", this);
            return;
        }

        float newEchoValue = value + woodEchoDelta;
        SetEcho(newEchoValue, "Wood");
    }

    private void CacheEnvironmentEchoBaseline()
    {
        hasEnvironmentEchoBaseline = false;

        if (audioMixer == null || string.IsNullOrWhiteSpace(echoParameter))
        {
            return;
        }

        if (!audioMixer.GetFloat(echoParameter, out currentEnvironmentEchoBaseline))
        {
            Debug.LogWarning($"Unable to cache mixer parameter '{echoParameter}' as environment baseline.", this);
            return;
        }

        hasEnvironmentEchoBaseline = true;
    }
}
