using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{


    public static UIManager Instance { get; private set; }


    [SerializeField] private Canvas _settingsMenuCanvas;


    private void Awake()
    {
        _settingsMenuCanvas.enabled = false;
    }

    public void OpenPauseMenu()
    {
        Debug.Log("Open Pause Menu");
    }
}