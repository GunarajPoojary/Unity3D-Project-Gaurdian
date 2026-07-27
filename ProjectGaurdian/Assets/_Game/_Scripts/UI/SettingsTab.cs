using UnityEngine.UI;

public class SettingsTab : TabGroup<Button, SettingsPanel>
{
    private SettingsPanel _currentPanel;

    private void Awake()
    {
        if (_tabs != null && _tabs.Length > 0)
        {
            _currentPanel = _tabs[0].tabPanel; // Set first panel as default
            _currentPanel.Open();
        }
    }

    protected override void HandleSelectTab(SettingsPanel tabPanel)
    {
        base.HandleSelectTab(tabPanel);

        _currentPanel.Close(); // Close previous panel

        _currentPanel = tabPanel; // Store the new panel

        _currentPanel.Open(); // And open the new panel
    }
}