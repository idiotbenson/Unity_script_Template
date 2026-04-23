using SpaceGraphicsToolkit;
using System.Collections;
using UnityEngine;

public class Prominence : MonoBehaviour
{
    SgtProminence sgtProminence;
    bool Shining = false;

    // Start is called before the first frame update
    void Start()
    {
        sgtProminence = GetComponent<SgtProminence>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, 1f);
    }

    public void IncreaseHoleRadius()
    {
        if (Shining == true)
        {
            return;
        }

        StartCoroutine(IncreaseRadius());
        Shining = true;

    }

    public void DecreaseHoleRadius()
    {
        if (Shining == false)
        {
            return;
        }
        StartCoroutine(DecreaseRadius());
        Shining = false;

    }

    IEnumerator IncreaseRadius()
    {


        for (float i = 0; i < 15; i += 1f)
        {
            sgtProminence.RadiusMax += 0.1f;
            Mathf.Clamp(sgtProminence.RadiusMax, 1.5f, 3f);
            yield return new WaitForSeconds(0.01f);
        }



    }

    IEnumerator DecreaseRadius()
    {


        for (float i = 0; i < 15; i += 1f)
        {
            sgtProminence.RadiusMax -= 0.1f;
            Mathf.Clamp(sgtProminence.RadiusMax, 1.5f, 3f);
            yield return new WaitForSeconds(0.01f);
        }

    }
}
