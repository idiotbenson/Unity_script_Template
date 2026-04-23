
using UnityEngine;

public class HintRaycaster : MonoBehaviour
{
    GameObject head;

    [Header("Raycast Detection")]
    [SerializeField] LayerMask interactableObjectLayer;
    [SerializeField][Min(1)] float hitRange;
    RaycastHit hit;

    [Header("Interactable hint")]
    [SerializeField] GameObject interactableUI;


    private void Start()
    {
        head = GameObject.Find("Head");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(head.transform.position, head.transform.forward * hitRange);

        if (hit.collider != null)
        {
            hit.collider.transform.parent.GetComponent<OutlineableObj>()?.TurnOffOutline();
            interactableUI.SetActive(false);
        }

        if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, hitRange, interactableObjectLayer))
        {
            hit.collider.transform.parent.GetComponent<OutlineableObj>()?.TurnOnOutline();
            interactableUI.SetActive(true);
        }




    }
}
