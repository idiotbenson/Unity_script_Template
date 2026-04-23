using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Votanic.vXR.vCast;

public class PlayAnimations : MonoBehaviour
{
    [FormerlySerializedAs("PlayAnimObject")]
    [SerializeField] private GameObject playAnimObject;
    [SerializeField] private string animName;
    [SerializeField] private float animSpeed = 1f;
    [FormerlySerializedAs("AnimDelay")]
    [SerializeField] private bool animDelay;
    [SerializeField] private bool fade;
    [FormerlySerializedAs("AnimDelayTime")]
    [SerializeField] private float animDelayTime;
    [SerializeField] private vCast_Fader fader;
    [SerializeField] private Animation targetAnimation;
    [SerializeField] private AnimationEvent animationEventHandler;
    [SerializeField] private bool verboseLog;

    private Coroutine delayedPlayCoroutine;

    private void Reset()
    {
        CacheAnimationIfMissing();
        CacheAnimationEventIfMissing();
    }

    private void OnValidate()
    {
        if (animDelayTime < 0f)
        {
            animDelayTime = 0f;
        }

        CacheAnimationIfMissing();
        CacheAnimationEventIfMissing();
    }

    private void Awake()
    {
        CacheAnimationIfMissing();
        CacheAnimationEventIfMissing();

        if (fader == null)
        {
            GameObject faderObject = GameObject.Find("CameraFader");
            if (faderObject != null)
            {
                fader = faderObject.GetComponent<vCast_Fader>();
            }
        }
    }

    public void TriggerAnimation()
    {
        if (playAnimObject == null)
        {
            Debug.LogWarning("PlayAnimations requires PlayAnimObject.", this);
            LogFlow("Trigger aborted: PlayAnimObject is null.");
            return;
        }

        CacheAnimationIfMissing();
        if (targetAnimation == null)
        {
            Debug.LogWarning("PlayAnimObject does not contain an Animation component.", playAnimObject);
            LogFlow("Trigger aborted: Animation component not found on PlayAnimObject.");
            return;
        }

        if (string.IsNullOrWhiteSpace(animName))
        {
            Debug.LogWarning("Animation name is empty. Please set Anim Name in inspector.", this);
            LogFlow("Trigger aborted: animation name is empty.");
            return;
        }

        AnimationState targetState = targetAnimation[animName];
        if (targetState == null)
        {
            Debug.LogWarning($"Animation clip '{animName}' was not found.", playAnimObject);
            LogFlow($"Trigger aborted: clip '{animName}' not found.");
            return;
        }

        if (fade && fader != null)
        {
            fader.isFading = true;
            fader.isRendering = true;
        }

        targetState.speed = animSpeed;
        LogFlow($"Validated clip '{animName}'. speed={animSpeed}, delay={animDelay}, delayTime={animDelayTime}");

        if (animDelay)
        {
            if (delayedPlayCoroutine != null)
            {
                StopCoroutine(delayedPlayCoroutine);
                LogFlow("Existing delayed coroutine stopped before starting new one.");
            }

            delayedPlayCoroutine = StartCoroutine(WaitBeforeAnimation());
            LogFlow("Delayed play coroutine started.");
        }
        else
        {
            TryPlayAndInvoke();
        }
    }

    private IEnumerator WaitBeforeAnimation()
    {
        yield return new WaitForSeconds(animDelayTime);
        LogFlow("Delay finished. Attempting to play animation now.");

        if (targetAnimation == null)
        {
            LogFlow("Delayed play cancelled: Animation component became null.");
            delayedPlayCoroutine = null;
            yield break;
        }

        TryPlayAndInvoke();
        delayedPlayCoroutine = null;
    }

    private void CacheAnimationIfMissing()
    {
        if (targetAnimation != null)
        {
            return;
        }

        if (playAnimObject != null)
        {
            targetAnimation = playAnimObject.GetComponent<Animation>();
        }
    }

    private void CacheAnimationEventIfMissing()
    {
        if (animationEventHandler != null)
        {
            return;
        }

        TryGetComponent(out animationEventHandler);
    }

    private void TryPlayAndInvoke()
    {
        bool isPlayed = targetAnimation.Play(animName);
        if (!isPlayed)
        {
            LogFlow($"Play failed for clip '{animName}'.");
            return;
        }

        LogFlow($"Play success for clip '{animName}'.");
        if (animationEventHandler != null)
        {
            animationEventHandler.InvokeAnimEvent();
            LogFlow("AnimationEvent.InvokeAnimEvent() called.");
            return;
        }

        LogFlow("Animation played, but no AnimationEvent component on this GameObject.");
    }

    private void LogFlow(string message)
    {
        if (!verboseLog)
        {
            return;
        }

        Debug.Log($"[PlayAnimations] {message}", this);
    }
}
