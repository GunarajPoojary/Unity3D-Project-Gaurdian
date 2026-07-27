using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState _currentState = GameState.Gameplay;

    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        InputManager.Instance.OnPausePerformed += HandlePausePerformed;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPausePerformed -= HandlePausePerformed;
    }

    private void Start()
    {
        EnterGameplayState();
    }






    private void EnterGameplayState()
    {
        SwitchState(GameState.Gameplay);
    }

    private void SwitchState(GameState state)
    {
        switch (state)
        {
            case GameState.UI:
                PauseGame();
                break;
            case GameState.Gameplay:
                ResumeGame();
                break;
        }

        _currentState = state;
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

    public void ResumeGame()
    {
        InputManager.Instance.EnableGameplayInput();
        InputManager.Instance.DisableUIInput();
        UIManager.Instance.ClosePauseMenu();
    }
}