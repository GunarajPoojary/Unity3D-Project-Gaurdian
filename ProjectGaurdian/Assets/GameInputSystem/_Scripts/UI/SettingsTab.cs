using UnityEngine.UI;

public class SettingsTab : TabGroup<Button, SettingsPanel>
{
    private SettingsPanel _cureentPanel;

    private void Awake()
    {
        if (_tabs != null && _tabs.Length > 0)
        {
            _cureentPanel = _tabs[0].tabPanel;
            _cureentPanel.Open();
        }
    }

    protected override void HandleSelectTab(SettingsPanel tabPanel)
    {
        base.HandleSelectTab(tabPanel);

        _cureentPanel.Close();

        _cureentPanel = tabPanel;

        _cureentPanel.Open();
    }
}