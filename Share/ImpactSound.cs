using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class ImpactSound : MonoBehaviour
{

    public AudioClip[] impactSound;
    public bool SpatialSound3D = true;
    public bool volumnChange = true;
    public float volumnRatio = 5;
    AudioSource AS;

    void Start()
    {
        AS = GetComponent<AudioSource>();
        if (SpatialSound3D)
            AS.spatialBlend = 1;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= 0.05f)
        {
            AS.clip = impactSound[Random.Range(0, impactSound.Length)];
            if (volumnChange)
                AS.volume = Mathf.Min(collision.relativeVelocity.magnitude / volumnRatio, 1);
            AS.Play();
        }
    }


}
