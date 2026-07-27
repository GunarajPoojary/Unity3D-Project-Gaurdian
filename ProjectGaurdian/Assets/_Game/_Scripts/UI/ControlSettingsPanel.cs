using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.html
public class ControlSettingsPanel : SettingsPanel
{

    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _sprintAction;
    [SerializeField] private InputActionReference _attackAction;
    [SerializeField] private InputActionReference _interactAction;

    [Header("Rebind Key Buttons")]
    [SerializeField] private Button _jumpKeyButton;
    [SerializeField] private TMP_Text _jumpLabel;

    [SerializeField] private Button _sprintKeyButton;
    [SerializeField] private TMP_Text _sprintLabel;

    [SerializeField] private Button _attackKeyButton;
    [SerializeField] private TMP_Text _attackLabel;

    [SerializeField] private Button _interactKeyButton;
    [SerializeField] private TMP_Text _interactLabel;

    [SerializeField] private Button _applySettingsButton;
    [SerializeField] private Button _revertSettingsButton;

    private const string LMB_LABEL = "<Mouse>/leftButton";
    private const string RMB_LABEL = "<Mouse>/rightButton";
    private const string MMB_LABEL = "<Mouse>/middleButton";
    private const string KEYBOARD_LEFT_SHIFT_LABEL = "<Keyboard>/leftShift";

    private void Awake()
    {
        RefreshUI();
    }

    private void OnEnable()
    {
        _jumpKeyButton.onClick.AddListener(RebindJump);
        _sprintKeyButton.onClick.AddListener(RebindSprint);
        _attackKeyButton.onClick.AddListener(RebindAttack);
        _interactKeyButton.onClick.AddListener(RebindInteract);
    }

    private void OnDisable()
    {
        _jumpKeyButton.onClick.RemoveListener(RebindJump);
        _sprintKeyButton.onClick.RemoveListener(RebindSprint);
        _attackKeyButton.onClick.RemoveListener(RebindAttack);
        _interactKeyButton.onClick.RemoveListener(RebindInteract);
    }





    private void RefreshUI()
    {
        SetKeyLabel(_jumpLabel, _jumpAction.action.bindings[0]);
        SetKeyLabel(_attackLabel, _attackAction.action.bindings[0]);
        SetKeyLabel(_sprintLabel, _sprintAction.action.bindings[0]);
        SetKeyLabel(_interactLabel, _interactAction.action.bindings[0]);
    }

    private void SetKeyLabel(TMP_Text label, InputBinding labelName)
    {
        label.text = GetDisplayName(labelName);
    }

    private string GetDisplayName(InputBinding binding)
    {
        return binding.effectivePath switch
        {
            LMB_LABEL => "LMB",
            RMB_LABEL => "RMB",
            MMB_LABEL => "MMB",
            KEYBOARD_LEFT_SHIFT_LABEL => "LSHIFT",
            _ => binding.ToDisplayString()
        };
    }

    private void RebindAction(TMP_Text label, Button keyButton, InputAction inputAction)
    {
        label.text = "?";
        keyButton.enabled = false;

        InputManager.Instance.Rebind(inputAction, (key) => { keyButton.enabled = true; SetKeyLabel(label, inputAction.bindings[0]); });
    }

    private void RebindJump()
    {
        RebindAction(_jumpLabel, _jumpKeyButton, _jumpAction);
    }

    private void RebindSprint()
    {
        RebindAction(_sprintLabel, _sprintKeyButton, _sprintAction);
    }

    private void RebindAttack()
    {
        RebindAction(_attackLabel, _attackKeyButton, _attackAction);
    }

    private void RebindInteract()
    {
        RebindAction(_interactLabel, _interactKeyButton, _interactAction);
    }
}