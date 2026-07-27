using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _attackAction;
    [SerializeField] private InputActionReference _interactAction;
    [SerializeField] private InputActionReference _sprintAction;


    private Vector2 _moveInput;

    public Vector2 MoveInput
    {
        get
        {
            return _moveInput;
        }
    }


    public event Action OnSprintPerformed;
    public event Action OnInteractPerformed;
    public event Action OnAttackPerformed;
    public event Action OnJumpPerformed;
    


    private void Update()
    {
        _moveInput = _moveAction.action.ReadValue<Vector2>();

        if (_jumpAction.action.WasPerformedThisFrame())
        {
            OnJumpPerformed?.Invoke();
        }

        if (_attackAction.action.WasPerformedThisFrame())
        {
            OnAttackPerformed?.Invoke();
        }

        if (_interactAction.action.WasPerformedThisFrame())
        {
            OnInteractPerformed?.Invoke();
        }

        if (_sprintAction.action.WasPerformedThisFrame())
        {
            OnSprintPerformed?.Invoke();
        }
    }
}