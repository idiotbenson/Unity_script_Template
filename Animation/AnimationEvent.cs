using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class AnimationEvent : MonoBehaviour
{
    [FormerlySerializedAs("AnimEvent")]
    [SerializeField] private UnityEvent animEvent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool verboseLog;

    private void Reset()
    {
        CacheAudioSourceIfMissing();
    }

    private void OnValidate()
    {
        CacheAudioSourceIfMissing();
    }

    private void Awake()
    {
        CacheAudioSourceIfMissing();
    }

    public void InvokeAnimEvent()
    {
        if (animEvent == null && verboseLog)
        {
            Debug.LogWarning("AnimationEvent has no UnityEvent bindings.", this);
        }

        animEvent?.Invoke();
        if (verboseLog)
        {
            Debug.Log("AnimationEvent.InvokeAnimEvent executed.", this);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            if (verboseLog)
            {
                Debug.LogWarning("PlaySound called with null AudioClip.", this);
            }

            return;
        }

        if (audioSource == null)
        {
            if (verboseLog)
            {
                Debug.LogWarning("AudioSource is missing; cannot play animation sound.", this);
            }

            return;
        }

        audioSource.PlayOneShot(clip);
        if (verboseLog)
        {
            Debug.Log($"PlaySound executed: {clip.name}", this);
        }
    }

    private void CacheAudioSourceIfMissing()
    {
        if (audioSource != null)
        {
            return;
        }

        TryGetComponent(out audioSource);
    }
}
