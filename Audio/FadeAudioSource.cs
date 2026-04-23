using System.Collections;
using UnityEngine;

public static class FadeAudioSource
{
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        return StartFade(audioSource, duration, targetVolume, false);
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume, bool useUnscaledTime)
    {
        if (audioSource == null)
        {
            yield break;
        }

        duration = Mathf.Max(0f, duration);
        targetVolume = Mathf.Clamp01(targetVolume);

        if (duration <= 0f)
        {
            audioSource.volume = targetVolume;
            yield break;
        }

        float currentTime = 0f;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            if (audioSource == null)
            {
                yield break;
            }

            currentTime += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / duration);
            audioSource.volume = Mathf.Lerp(start, targetVolume, t);
            yield return null;
        }

        // Ensure final value is exact after loop.
        if (audioSource != null)
        {
            audioSource.volume = targetVolume;
        }
    }
}