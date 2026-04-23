
using UnityEngine;
using UnityEngine.UI;

public class NameUI : MonoBehaviour
{
    [SerializeField] float Height;

    public Text playerName;
    Transform target;
    public CanvasGroup canvasGroup;
    Renderer playerRend;

    // Start is called before the first frame update
    void Start()
    {

        //this.transform.SetParent(GameObject.Find("PlayerNameCanvas").GetComponent<Transform>(), false);
        //playerName = this.GetComponent<Text>();
        //canvasGroup = this.GetComponent<CanvasGroup>();


        Flip();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRend == null)
        {
            Destroy(gameObject);
            return;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Helper.Camera);
        bool playerInView = GeometryUtility.TestPlanesAABB(planes, playerRend.bounds);
        canvasGroup.alpha = playerInView ? 1 : 0;
        //this.transform.position = Cam.WorldToScreenPoint(target.position + Vector3.up * 1.2f);
        this.transform.position = target.position + Vector3.up * Height;

        transform.LookAt(Helper.Camera.transform);
    }

    public void SetTarget(Transform t)
    {
        this.target = t;
    }

    public void SetPlayerRend(Renderer rend)
    {
        this.playerRend = rend;
    }

    void Flip()
    {
        float tempX = transform.localScale.x;
        tempX *= -1;
        transform.localScale = new Vector3(tempX, transform.localScale.y, transform.localScale.z);
    }
}
