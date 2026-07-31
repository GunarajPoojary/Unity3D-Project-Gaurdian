using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState _currentState = GameState.Gameplay;

    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
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



    public void PauseGame()
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