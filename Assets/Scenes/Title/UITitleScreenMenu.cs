using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITitleScreenMenu : UIMenu
{
    [Header("Start Game")]
    [SerializeField] private Button startButton;
    [SerializeField] private string _startingScene;

    [Space]
    [Header("Quit Game")]
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
        SceneManager.LoadScene(_startingScene);
    }

    private void OnQuitButtonPressed()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }

        Application.Quit();
    }
}
