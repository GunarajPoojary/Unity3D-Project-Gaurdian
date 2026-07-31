using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _gameplayPauseAction;


    private void Update()
    {
        if (_gameplayPauseAction.action.WasPerformedThisFrame())
        {
            HandlePausePerformed();
        }
    }

    private void HandlePausePerformed()
    {
        Debug.Log("Enter Pause Game State");

        GameManager.Instance.PauseGame();
    }
}