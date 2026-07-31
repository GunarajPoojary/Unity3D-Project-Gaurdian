using UnityEngine;

public class AudioSettingsPanel : SettingsPanel
{
    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}