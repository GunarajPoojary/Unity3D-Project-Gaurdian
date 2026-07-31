using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

////TODO: Implement key conflict handling

[DisallowMultipleComponent]
public class RebindActionUI : MonoBehaviour
{
    [Header("Action")]
    [SerializeField] private InputActionReference _inputAction;
    [SerializeField] private string _bindingId;


    [Header("UI References")]
    [SerializeField] private TMP_Text _actionLabel;
    [SerializeField] private TMP_Text _bindingText;
    [SerializeField] private float _rebindTimeout;
    [SerializeField] private Button _resetToDefaultBindingButton;

    [SerializeField] private GameObject _rebindGuideText;


    private RebindingOperation _rebindOperation;


    public int BindingIndex
    {
        get
        {
            InputAction action = _inputAction.action;

            if (action == null || string.IsNullOrEmpty(_bindingId) || !Guid.TryParse(_bindingId, out var bindingGuid))
                return -1;

            return action.bindings.IndexOf(x => x.id == bindingGuid);
        }
    }


    private void OnEnable()
    {
        UpdateActionName();

        if (_resetToDefaultBindingButton != null)
            _resetToDefaultBindingButton.onClick.AddListener(ResetToDefaultBinding);

        int bindingIndex = BindingIndex;
        UpdateBindingDisplay(bindingIndex);

    }

    private void OnDisable()
    {
        if (_resetToDefaultBindingButton != null)
            _resetToDefaultBindingButton.onClick.RemoveListener(ResetToDefaultBinding);

        _rebindOperation?.Cancel();

    }

    private void OnDestroy()
    {
        _rebindOperation?.Dispose();
        _rebindOperation = null;
    }




    private void UpdateActionName()
    {
        // The composite binding has index 0 and composite part binding starts from index 1
        if (_actionLabel != null && _inputAction != null && _inputAction.action != null)
            _actionLabel.text = _inputAction.action.bindings[0].isComposite ? ToTitle(_inputAction.action.bindings[BindingIndex].name) : _inputAction.action.name;
    }


    private string ToTitle(string text)
    {
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text);
    }




    // See https://discussions.unity.com/t/how-to-rebind-composite-actions/760229 for Unity Discussion
    public void StartInteractiveRebind()
    {
        InputAction action = _inputAction.action;

        if (action == null)
        {
            Debug.LogWarning("No Input Action assigned.");
            return;
        }

        _rebindOperation?.Cancel(); // Will null out _rebindOperation.

        int bindingIndex = BindingIndex;

        // An "InvalidOperationException: Cannot rebind action x while it is enabled" will
        // be thrown if rebinding is attempted on an action that is enabled.
        //
        // Disable the Input Action Asset since we don't want any type of interaction during rebinding
        // if (action.actionMap.asset.enabled)
        //     action.actionMap.asset.Disable();

        if (_bindingText != null)
            _bindingText.text = "?";

        if (_rebindGuideText != null)
            _rebindGuideText.SetActive(true);

        void CleanUp()
        {
            _rebindOperation?.Dispose();
            _rebindOperation = null;

            // if (!action.actionMap.asset.enabled)
            //     action.actionMap.asset.Enable();
        }

        // Configure the rebind.
        _rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            // Ignore mouse position/delta so tiny mouse movement doesn't get picked up as the binding,
            // and let Escape cancel the rebind.
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(
                operation =>
                {
                    if (_rebindGuideText != null)
                        _rebindGuideText.SetActive(false);

                    CleanUp();
                    UpdateBindingDisplay(bindingIndex);
                })
            // We want device state to update but not actions firing during rebinding.
            .WithActionEventNotificationsBeingSuppressed()
            // We use a timeout to illustrate that its possible to skip cancel buttons and let rebind timeout.
            .WithTimeout(_rebindTimeout)
            .OnComplete(
                operation =>
                {
                    if (_rebindGuideText != null)
                        _rebindGuideText.SetActive(false);

                    CleanUp();


                    UpdateBindingDisplay(bindingIndex);

                });

        _rebindOperation.Start();
    }

    public void ResetToDefaultBinding()
    {
        InputAction action = _inputAction.action;
        int bindingIndex = BindingIndex;

        action.RemoveBindingOverride(bindingIndex);

        UpdateBindingDisplay(bindingIndex);
    }

    public void UpdateBindingDisplay(int bindingIndex)
    {
        var displayString = "?";

        InputAction action = _inputAction.action;

        if (action != null && bindingIndex < action.bindings.Count)
            displayString = GetDisplayKeyString(action, bindingIndex);

        if (_bindingText != null)
            _bindingText.text = displayString;
    }

    private string GetDisplayKeyString(InputAction action, int bindingIndex)
    {
        return InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}