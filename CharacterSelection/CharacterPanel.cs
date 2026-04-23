
using UnityEngine;
using UnityEngine.UI;
public class CharacterPanel : MonoBehaviour
{
    public JobData jobData;
    [SerializeField] Image Outliner;
    [SerializeField] Text PlayerNameField;
    [SerializeField] Text JobName;
    [SerializeField] Text JobDescription;
    [SerializeField] Image JobImage;
    [SerializeField] Button selectButton;

    public bool DisplayInfoWhenHover;

    private void Start()
    {
        if (selectButton == null)
        {
            selectButton = GetComponent<Button>();
        }

        if (selectButton != null)
        {
            selectButton.onClick.AddListener(PressedEvent);
        }


        InitCharacterInfo();
        SetPlayerNameField("");
        SetOutliner(false);
    }

    public void SetPlayerNameField(string playerName)
    {
        PlayerNameField.text = playerName;
    }

    public string GetPlayerNameField()
    {
        return this.PlayerNameField.text;
    }

    public void SetOutliner(bool active)
    {
        //Outliner.enabled = active;
        Outliner.gameObject.SetActive(active);
    }

    public void SetSelectionInteractable(bool active)
    {
        if (selectButton != null)
        {
            selectButton.interactable = active;
        }

    }

    public void HighlightPanel(bool outlinerActive, bool buttonActive, string playerName)
    {
        SetSelectionInteractable(buttonActive);
        SetOutliner(outlinerActive);

        SetPlayerNameField(playerName);

    }

    public void InitCharacterInfo()
    {
        JobName.text = jobData.name;
        JobDescription.text = jobData.jobDescription;
        JobImage.sprite = jobData.jobImage;
    }

    public void DisplayJobDesciption(bool active)
    {
        JobDescription.transform.parent.gameObject.SetActive(active);
    }


    void HoverEvent()
    {
        //Debug.Log("DescriptionActive");
        if (DisplayInfoWhenHover)
        {
            DisplayJobDesciption(true);

        }

    }

    void UnHoverEvent()
    {
        //Debug.Log("DescriptionDeActive");
        if (DisplayInfoWhenHover)
        {
            DisplayJobDesciption(false);
        }

    }

    void PressedEvent()
    {
        //Update all the hightlight selected panel on every player screen
        CharacterSelectionManager.instance.BeginHightlightCharacterPanel(jobData.jobIndex);

        //Update player skin on every player screen
        //NetworkedPlayer.LocalPlayerInstance.GetComponent<PlayerClothing>().BeginChangeClothes(jobData.jobIndex);

        //Update player skin on every player screen
        NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().BeginChangeBody(jobData.jobIndex);

        //Update all the jobnames on every network player
        NetworkedPlayer.LocalPlayerInstance.GetComponent<NetworkedPlayer>().BeginSetJobName(jobData.name);

        if (jobData.avatarModel != null)
        {
            AvatarMenuController.Instance.SpawnAvatarModel(jobData.avatarModel, jobData.jobName);
        }


    }


}
