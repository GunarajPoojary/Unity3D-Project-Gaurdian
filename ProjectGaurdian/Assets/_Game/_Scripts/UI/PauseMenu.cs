using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : UIScreen
{
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _settingsMenuCloseButton;

    [SerializeField] private GameObject _root;
    [SerializeField] private SettingsMenu _settingsMenu;






    public override void Open()
    {
        gameObject.SetActive(true);
        _resumeButton.onClick.AddListener(HandleResumeClicked);
        _settingsButton.onClick.AddListener(HandleSettingsClicked);
        _settingsMenuCloseButton.onClick.AddListener(HandleSettingsMenuCloseClicked);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
        _resumeButton.onClick.AddListener(HandleResumeClicked);
        _settingsButton.onClick.AddListener(HandleSettingsClicked);
        _settingsMenuCloseButton.onClick.AddListener(HandleSettingsMenuCloseClicked);
    }

    private void HandleResumeClicked()
    {
        GameManager.Instance.ResumeGame();
        Close();
    }

    private void HandleSettingsClicked()
    {
        _root.SetActive(false);
        _settingsMenu.Open();
    }

    private void HandleSettingsMenuCloseClicked()
    {
        _settingsMenu.Close();
        _root.SetActive(true);
    }

}