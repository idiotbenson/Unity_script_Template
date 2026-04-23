
using UnityEngine;

public class ToolControllerManager : MonoBehaviour
{
    public static ToolControllerManager instance;

    public ToolController[] AllToolController;
    ToolController currentToolController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void SwitchToolController(ToolController targetController)
    {
        Debug.Log("Old controller:" + this.currentToolController);

        //Turn off old controller
        if (this.currentToolController != null)
        {
            this.currentToolController.toolReturner.gameObject.SetActive(false);
            this.currentToolController.toolDisplayer.gameObject.SetActive(true);
            this.currentToolController.gameObject.SetActive(false);

        }

        //Not switch to new controller if it is null
        if (targetController == null)
        {
            return;
        }

        //Turn on new controller and store it into current controller
        Debug.Log("Switched to new controller: " + targetController.name);
        this.currentToolController = targetController;
        this.currentToolController.gameObject.SetActive(true);
    }
}
