using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GraphicsSettingsPanel : SettingsPanel
{

    private const string PP_RESOLUTION_INDEX = "video_resolutionIndex";
    private const string PP_QUALITY_INDEX = "video_qualityIndex";
    private const string PP_FULLSCREEN = "video_fullscreen";



    [Header("UI References")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    // [SerializeField] private Toggle _fullscreenToggle;


    [Header("UI References")]
    // [SerializeField] private Button _okButton;
    [SerializeField] private GameObject _panelRoot;

    private Resolution[] _availableResolutions;
    private List<Resolution> _filteredResolutions;

    private void Awake()
    {
        BuildResolutionChoices();
        // BuildQualityList();

        LoadSavedSettings();
    }
    



    public override void Open()
    {
        // _okButton.onClick.AddListener(() => _panelRoot.SetActive(false));

        // _fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        _resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        // _qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        // _okButton.onClick.RemoveListener(() => _panelRoot.SetActive(false));

        // _fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
        _resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
        // _qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);

        gameObject.SetActive(false);
    }



    private void BuildResolutionChoices()
    {
        // See for Resolution https://docs.unity3d.com/ScriptReference/Resolution.html
        _availableResolutions = Screen.resolutions;

        _filteredResolutions = _availableResolutions
            .GroupBy(r => new { r.width, r.height })
            .Select(g => g.OrderByDescending(r => r.refreshRateRatio.value).First())
            .OrderBy(r => r.width * r.height)
            .ToList();

        _resolutionDropdown.ClearOptions();
        var options = _filteredResolutions
            .Select(r => $"{r.width} x {r.height}")
            .ToList();
        _resolutionDropdown.AddOptions(options);

        int currentIndex = _filteredResolutions.FindIndex(
            r => r.width == Screen.currentResolution.width &&
                 r.height == Screen.currentResolution.height);
        if (currentIndex < 0) currentIndex = _filteredResolutions.Count - 1;
        _resolutionDropdown.SetValueWithoutNotify(currentIndex);
    }

    private void OnResolutionChanged(int index)
    {
        Resolution res = _filteredResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt(PP_RESOLUTION_INDEX, index);
    }


    // private void BuildQualityList()
    // {
    //     _qualityDropdown.ClearOptions();
    //     _qualityDropdown.AddOptions(QualitySettings.names.ToList());
    //     _qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
    // }

    private void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index, applyExpensiveChanges: true);
        PlayerPrefs.SetInt(PP_QUALITY_INDEX, index);
    }


    private void OnFullscreenChanged(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;
        PlayerPrefs.SetInt(PP_FULLSCREEN, isFullscreen ? 1 : 0);
    }


    private void LoadSavedSettings()
    {
        if (PlayerPrefs.HasKey(PP_RESOLUTION_INDEX))
        {
            int idx = PlayerPrefs.GetInt(PP_RESOLUTION_INDEX);
            _resolutionDropdown.SetValueWithoutNotify(idx);
            Resolution res = _filteredResolutions[idx];
            Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
        }

        if (PlayerPrefs.HasKey(PP_QUALITY_INDEX))
        {
            int idx = PlayerPrefs.GetInt(PP_QUALITY_INDEX);
            // _qualityDropdown.SetValueWithoutNotify(idx);
            QualitySettings.SetQualityLevel(idx, true);
        }

        bool fullscreen = PlayerPrefs.GetInt(PP_FULLSCREEN, Screen.fullScreen ? 1 : 0) == 1;
        // _fullscreenToggle.SetIsOnWithoutNotify(fullscreen);
        Screen.fullScreenMode = fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
}