using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class SettingsMenu : UIScreen
{
    [SerializeField] private SettingsTab _tabGroup;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();

        Close();
    }




    public override void Open()
    {
        _canvas.enabled = true;
    }

    public override void Close()
    {
        _canvas.enabled = false;
    }
}