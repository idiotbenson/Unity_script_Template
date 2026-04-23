using System.Collections;
using UnityEngine;

public class TriggerFadeSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float targetVolume = 0.2f;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool useUnscaledTime;

    private Coroutine activeFadeCoroutine;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (playOnStart)
        {
            TriggerFade();
        }
    }

    private void OnDisable()
    {
        StopFade();
    }

    private void OnValidate()
    {
        fadeDuration = Mathf.Max(0f, fadeDuration);
        targetVolume = Mathf.Clamp01(targetVolume);
    }

    public void TriggerFade()
    {
        if (!isActiveAndEnabled)
        {
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("TriggerFadeSound requires an AudioSource.", this);
            return;
        }

        if (activeFadeCoroutine != null)
        {
            StopCoroutine(activeFadeCoroutine);
        }

        activeFadeCoroutine = StartCoroutine(FadeRoutine());
    }

    public void StopFade()
    {
        if (activeFadeCoroutine == null)
        {
            return;
        }

        StopCoroutine(activeFadeCoroutine);
        activeFadeCoroutine = null;
    }

    private IEnumerator FadeRoutine()
    {
        yield return FadeAudioSource.StartFade(audioSource, fadeDuration, targetVolume, useUnscaledTime);
        activeFadeCoroutine = null;
    }
}
