using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[CustomEditor(typeof(RebindActionUI))]
[CanEditMultipleObjects]
public class RebindActionUIEditor : Editor
{
    private const string ACTION_PROPERTY_PATH = "_inputAction";
    private const string BINDING_ID_PROPERTY_PATH = "_bindingId";
    private const string ACTION_LABEL_PROPERTY_PATH = "_actionLabel";
    private const string BINDING_TEXT_PROPERTY_PATH = "_bindingText";
    private const string REBIND_TIMEOUT_PROPERTY_PATH = "_rebindTimeout";
    private const string REBIND_GUIDE_TEXT_PROPERTY_PATH = "_rebindGuideText";
    private const string RESET_TO_DEFAULT_BINDING_BUTTON_PROPERTY_PATH = "_resetToDefaultBindingButton";


    private SerializedProperty _actionProperty;
    private SerializedProperty _bindingIDProperty;
    private SerializedProperty _actionLabelProperty;
    private SerializedProperty _bindingTextProperty;
    private SerializedProperty _rebindTimeoutProperty;
    private SerializedProperty _rebindGuideTextProperty;
    private SerializedProperty _resetToDefaultBindingButtonProperty;


    private List<string> _bindingChoices = new();
    private List<string> _bindingIDs = new();
    private string _defaultBindingChoice = "No selection found";

    private DropdownField _bindingDropdown;


    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        // I'm keeping the default script field, you don't have to
        root.Add(CreateDefaultScriptField());

        root.Add(CreateActionField());

        root.Add(CreateBindingDropdown());

        root.Add(CreatePropertyField());

        // Queue the RefreshBindingDropdown callback to run later
        // This will refresh the Binding Dropdown every Editor tick  
        root.schedule.Execute(RefreshBindingDropdown);

        return root;
    }




    private VisualElement CreatePropertyField()
    {
        var container = new VisualElement();

        _actionLabelProperty = serializedObject.FindProperty(ACTION_LABEL_PROPERTY_PATH);
        _bindingTextProperty = serializedObject.FindProperty(BINDING_TEXT_PROPERTY_PATH);
        _rebindTimeoutProperty = serializedObject.FindProperty(REBIND_TIMEOUT_PROPERTY_PATH);
        _rebindGuideTextProperty = serializedObject.FindProperty(REBIND_GUIDE_TEXT_PROPERTY_PATH);
        _resetToDefaultBindingButtonProperty = serializedObject.FindProperty(RESET_TO_DEFAULT_BINDING_BUTTON_PROPERTY_PATH);

        //See https://docs.unity3d.com/ScriptReference/UIElements.PropertyField.html
        container.Add(new PropertyField(_actionLabelProperty));
        container.Add(new PropertyField(_bindingTextProperty));
        container.Add(new PropertyField(_rebindTimeoutProperty));
        container.Add(new PropertyField(_rebindGuideTextProperty));
        container.Add(new PropertyField(_resetToDefaultBindingButtonProperty));

        return container;
    }

    private VisualElement CreateDefaultScriptField()
    {
        var scriptField = new PropertyField(serializedObject.FindProperty("m_Script"));
        scriptField.SetEnabled(false);

        return scriptField;
    }

    private VisualElement CreateActionField()
    {
        _actionProperty = serializedObject.FindProperty(ACTION_PROPERTY_PATH);

        var inputActionField = new PropertyField(_actionProperty);

        inputActionField.RegisterValueChangeCallback(_ => RefreshBindingDropdown());

        return inputActionField;
    }

    private VisualElement CreateBindingDropdown()
    {
        _bindingIDProperty = serializedObject.FindProperty(BINDING_ID_PROPERTY_PATH);

        // For more infor related to DropdownField, see https://docs.unity3d.com/6000.5/Documentation/ScriptReference/UIElements.DropdownField.html
        _bindingDropdown = new DropdownField(label: "Binding", choices: _bindingChoices, defaultIndex: 0);
        _bindingDropdown.style.marginBottom = 8;

        _bindingDropdown.RegisterValueChangedCallback(OnBindingChanged);

        return _bindingDropdown;
    }





    private void RefreshBindingDropdown()
    {
        // if (evt.changedProperty == null)
        _actionProperty = serializedObject.FindProperty(ACTION_PROPERTY_PATH);

        _bindingChoices.Clear();
        _bindingIDs.Clear();

        InputActionReference inputActionRef = _actionProperty.objectReferenceValue as InputActionReference;

        if (inputActionRef == null)
        {
            // Debug.Log("No Input Action assigned in the inspector.");

            _bindingDropdown.value = _defaultBindingChoice;
            _bindingDropdown.SetEnabled(false);
            return;
        }

        BuildBindingChoices(inputActionRef.action);
    }

    private void BuildBindingChoices(InputAction inputAction)
    {
        string compositeName = "";

        foreach (var binding in inputAction.bindings)
        {
            if (binding.isComposite)
            {
                compositeName = binding.name;
                continue;
            }

            string bindingName;

            // Don't use "/" for which the editor will treat it as submenu unless you want it
            if (binding.isPartOfComposite)
            {
                bindingName = compositeName + " | " + ToTitle(binding.name) + " : ";
            }
            else
            {
                bindingName = ToTitle(binding.action) + " : ";
            }

            bindingName += InputControlPath.ToHumanReadableString(binding.effectivePath);

            _bindingChoices.Add(bindingName);
            _bindingIDs.Add(binding.id.ToString());
        }

        if (_bindingChoices.Count == 0)
        {
            _bindingDropdown.value = _defaultBindingChoice;
            _bindingDropdown.SetEnabled(false);
            return;
        }

        _bindingDropdown.choices = _bindingChoices;
        _bindingDropdown.SetEnabled(true);

        int currentIndex = _bindingIDs.IndexOf(_bindingIDProperty.stringValue);

        if (currentIndex < 0)
            currentIndex = 0;

        _bindingDropdown.SetValueWithoutNotify(_bindingChoices[currentIndex]);

        if (_bindingIDs[currentIndex] != _bindingIDProperty.stringValue)
        {
            _bindingIDProperty.stringValue = _bindingIDs[currentIndex];
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void OnBindingChanged(ChangeEvent<string> evt)
    {
        int selectedIndex = _bindingChoices.IndexOf(evt.newValue);

        if (selectedIndex < 0)
            return;

        _bindingIDProperty.stringValue = _bindingIDs[selectedIndex];
        serializedObject.ApplyModifiedProperties();
    }

    private string ToTitle(string text)
    {
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text);
    }
}