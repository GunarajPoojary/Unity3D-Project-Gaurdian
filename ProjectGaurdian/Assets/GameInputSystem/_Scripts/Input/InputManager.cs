using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private InputActionAsset _gameInputActions;
    [SerializeField] private InputActionReference _gameplayPauseAction;



    private InputActionMap _gameplayActionMap;
    private InputActionMap _uIActionMap;


    public event Action OnPausePerformed;


    private void Awake()
    {
        Instance = this;

        Init();
    }

    private void Start()
    {
        _uIActionMap.Disable();
    }

    private void Update()
    {
        if (_gameplayPauseAction.action.WasPerformedThisFrame())
        {
            OnPausePerformed?.Invoke();
        }
    }


    private void Init()
    {
        _gameplayActionMap = _gameInputActions.FindActionMap("Gameplay");
        _uIActionMap = _gameInputActions.FindActionMap("UI");
    }






    // Refer https://discussions.unity.com/t/how-to-rebind-composite-actions/760229 for Unity Discussion
    // This is for Composite keys such as WASD. We don't want to rebind them  
    // public void RebindMoveUp()
    // {
    // Temporarily disable the Move Action
    // _moveAction.Disable();

    // BindingSyntax compositeAccessor = _moveAction.ChangeCompositeBinding("WASD");

    // BindingSyntax upPart = compositeAccessor.NextPartBinding("up");

    // // Use the absolute binding index for targeted UI updates or rebinding operations
    // _upIndex = upPart.bindingIndex;

    // _moveAction.PerformInteractiveRebinding(_upIndex)
    //                                  .WithCancelingThrough("<Keyboard>/escape")
    //                                  .OnMatchWaitForAnother(0.1f)
    //                                  .WithControlsExcluding("Mouse")
    //                                  .OnCancel(HandleRebindCancel)
    //                                  .OnComplete(HandleRebindComplete)
    //                                  .Start();
    // }


    public void Rebind(InputAction action, Action onComplete)
    {
        // Temporarily disable Action
        action.Disable();

        action.PerformInteractiveRebinding()
                   .WithCancelingThrough("<Keyboard>/escape") // Avoid binding with Escape key
                                                              //    .WithControlsExcluding("<Mouse>/leftButton") // Also exclude LMB
                   .OnCancel((_) => onComplete?.Invoke())
                   .OnComplete((_) => onComplete?.Invoke())
                   .Start();
    }

    public void EnableUIInput()
    {
        _uIActionMap.Enable();
        _gameInputActions.Disable();
    }

    public void DisableGameplayInput()
    {
        _uIActionMap.Disable();
        _gameInputActions.Enable();
    }
}