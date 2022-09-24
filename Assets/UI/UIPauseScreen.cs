using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPauseScreen : UIMenu
{
    [Header("Resume")]
    [SerializeField] private Button resumeButton;

    [Header("Main Menu")]
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(OnResumeButtonPressed);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
    }

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(OnResumeButtonPressed);
        mainMenuButton.onClick.RemoveListener(OnMainMenuButtonPressed);
    }

    private void OnResumeButtonPressed()
    {
        LevelManager.Current.Unpause();
    }

    private void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene(0);
    }
}

