
using UnityEngine;
using UnityEngine.UI;

public class HintPanel : MonoBehaviour
{
    public Text textfield;
    public HintText hintData;

    void Start()
    {
        textfield.text = hintData.text;
    }
}


[System.Serializable]
public class HintText
{
    public string text;
}
