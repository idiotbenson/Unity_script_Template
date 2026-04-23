
using UnityEngine;

public class SetAnimator : MonoBehaviour
{
    [SerializeField] Transform userTransform;

    Vector3 previous;
    float velocity;

    Animator anim;



    void Start()
    {
        if (userTransform == null)
        {
            var userGo = GameObject.Find("User");
            if (userGo != null)
            {
                userTransform = userGo.transform;
            }
        }

        anim = GetComponent<Animator>();
        if (userTransform != null)
        {
            previous = userTransform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (userTransform == null || anim == null)
        {
            return;
        }

        if (Time.deltaTime <= 0f)
        {
            return;
        }

        velocity = ((userTransform.position - previous).magnitude) / Time.deltaTime;
        previous = userTransform.position;

        //Debug.Log("Velocity" + velocity);

        if (velocity >= 0.1f)
        {

            anim.SetBool("Run", true);
        }
        else
        {

            anim.SetBool("Run", false);
        }
    }
}
