using UnityEngine;

public class SettingsMenu : UIScreen
{
    [SerializeField] private SettingsTab _tabGroup;



    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}