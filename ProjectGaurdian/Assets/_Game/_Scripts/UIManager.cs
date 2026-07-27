using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{


    public static UIManager Instance { get; private set; }



    [SerializeField] private PauseMenu _pauseMenu;


    private void Awake()
    {
        Instance = this;
    }

    public void OpenPauseMenu()
    {
        _pauseMenu.Open();
    }

    public void ClosePauseMenu()
    {
        _pauseMenu.Close();
    }
}