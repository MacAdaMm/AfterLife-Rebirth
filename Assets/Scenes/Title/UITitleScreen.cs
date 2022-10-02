using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITitleScreen : UIMenu
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnStartButtonPressed);
        _quitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(OnStartButtonPressed);
        _quitButton.onClick.RemoveListener(OnQuitButtonPressed);
    }

    private void OnStartButtonPressed()
    {
        GameManager.Instance.LoadGame();
    }

    private void OnQuitButtonPressed()
    {
        GameManager.Instance.QuitApplication();
    }
}
