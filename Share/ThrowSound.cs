using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class ThrowSound : MonoBehaviour
{

    public AudioClip throwSound;
    public float soundVolume = 1;
    public float minVelocity = 3;
    AudioSource AS;
    Rigidbody RB;

    void Start()
    {
        AS = GetComponent<AudioSource>();
        RB = GetComponent<Rigidbody>();
    }

    public void PlayThrowSound()
    {
        Invoke("StartPlaySound", 0.05f);

    }

    void StartPlaySound()
    {
        if (RB.velocity.magnitude >= minVelocity)
        {
            AS.volume = soundVolume;
            AS.PlayOneShot(throwSound);
        }
    }
}
