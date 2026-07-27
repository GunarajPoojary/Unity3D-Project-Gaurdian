using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Instance.OnPausePerformed += HandlePausePerformed;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPausePerformed -= HandlePausePerformed;
    }






    private void HandlePausePerformed()
    {
        Debug.Log("Enter Pause Game State");

        PauseGame();
    }

    private void PauseGame()
    {
        // Enter the Pause state
        // Disable Gameplay Input
        InputManager.Instance.DisableGameplayInput();
        InputManager.Instance.EnableUIInput();
        UIManager.Instance.OpenPauseMenu();
    }
}