using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private PlayerInput _input;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _input.OnJumpPerformed += HandleJump;
    }

    private void OnDisable()
    {
        _input.OnJumpPerformed -= HandleJump;
    }

    private void Update()
    {
        if (_input.MoveInput == Vector2.zero) return;

        Debug.Log($"Move input is {_input.MoveInput}");
    }

    private void HandleJump()
    {
        Debug.Log("Jump");
    }
}