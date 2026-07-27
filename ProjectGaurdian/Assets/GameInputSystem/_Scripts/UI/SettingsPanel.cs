using UnityEngine;

public class SettingsPanel : UIScreen, ISettingsPanel
{
    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void ApplySettings()
    {
        Debug.Log("Apply Settings");
    }

    public void LoadSettings()
    {
        Debug.Log("Load Settings");
    }
}