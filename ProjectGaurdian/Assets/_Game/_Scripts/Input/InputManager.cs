using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }



    [Tooltip("The player preference key to be used when serializing binding overrides to player preferences (Required).")]
    [SerializeField] private string _playerPreferenceKey;

    [Tooltip("Specifies whether to load and apply binding overrides when the component is enabled")]
    [SerializeField] private bool _loadOnEnable = true;

    [Tooltip("Specifies whether to save binding overrides when the component is disabled")]
    [SerializeField] private bool _saveOnDisable = true;

    [SerializeField] private InputActionAsset _gameInputActions;



    private InputActionMap _gameplayActionMap;
    private InputActionMap _uIActionMap;
    private Action<string> _rebindCompleteAction;

    private void Awake()
    {
        Instance = this;

        Init();
    }

    private void OnEnable()
    {
        if (_loadOnEnable)
            Load();
    }

    private void OnDisable()
    {
        if (_saveOnDisable)
            Save();
    }

    private void Start()
    {
        _uIActionMap.Disable();
    }

    private void OnDestroy()
    {
        SaveBindings();
    }

    [ContextMenu("Validate")]
    private void ValidateInput()
    {
        string assetEnabled = _gameInputActions.enabled ? "enabled" : "disabled";
        string gameplayEnabled = _gameplayActionMap.enabled ? "enabled" : "disabled";
        string uiEnabled = _uIActionMap.enabled ? "enabled" : "disabled";

        Debug.Log($"Input Action Asset is {assetEnabled}");
        Debug.Log($"Gameplay Action is {gameplayEnabled}");
        Debug.Log($"UI Action is {uiEnabled}");
    }

    public void EnableUIInput()
    {
        _uIActionMap.Enable();
    }

    public void DisableGameplayInput()
    {
        _gameplayActionMap.Disable();
    }

    public void DisableUIInput()
    {
        _uIActionMap.Disable();
    }

    public void EnableGameplayInput()
    {
        _gameInputActions.Enable();
    }




    private void Init()
    {
        LoadBindings();

        _gameplayActionMap = _gameInputActions.FindActionMap("Gameplay");
        _uIActionMap = _gameInputActions.FindActionMap("UI");
    }

    private void LoadBindings()
    {
        string rebind = PlayerPrefs.GetString("rebinds");

        _gameInputActions.LoadBindingOverridesFromJson(rebind);
    }

    private void SaveBindings()
    {
        string rebind = _gameInputActions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebind);
    }






    /// <summary>
    /// Loads binding overrides from player preferences and applies them to the associated input action asset.
    /// </summary>
    public void Load()
    {
        if (!IsValidConfiguration())
            return;

        var rebinds = PlayerPrefs.GetString(_playerPreferenceKey);
        if (string.IsNullOrEmpty(rebinds))
            return; // OK, we may not have saved any binding overrides yet.

        _gameInputActions.LoadBindingOverridesFromJson(rebinds);
    }

    /// <summary>
    /// Saves binding overrides from the associated input action asset and persists them to player preferences.
    /// </summary>
    public void Save()
    {
        if (!IsValidConfiguration())
            return;

        var rebinds = _gameInputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(_playerPreferenceKey, rebinds);
    }

    private bool IsValidConfiguration()
    {
        if (_gameInputActions == null)
        {
            Debug.LogWarning("Unable to apply binding overrides from player preferences without an associated action asset.");
            return false;
        }

        if (string.IsNullOrEmpty(_playerPreferenceKey))
        {
            Debug.LogWarning("Unable to load binding overrides from player preferences without a non-empty preference key.");
            return false;
        }

        return true;
    }


}