using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITitleScreen : UIMenu
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnStartButtonPressed);
        quitButton.onClick.RemoveListener(OnQuitButtonPressed);
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
