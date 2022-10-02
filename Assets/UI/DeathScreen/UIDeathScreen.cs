using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDeathScreen : UIMenu
{
    [Header("Respawn")]
    [SerializeField] private Button _respawnButton;

    [Header("Main Menu")]
    [SerializeField] private Button _mainMenuButton;

    private void Awake()
    {
        _respawnButton.onClick.AddListener(OnRespawnButtonPressed);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
    }


    private void OnDestroy()
    {
        _respawnButton.onClick.RemoveListener(OnRespawnButtonPressed);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonPressed);
    }

    private void OnRespawnButtonPressed()
    {
        GameManager.Instance.LoadGame();
    }

    private void OnMainMenuButtonPressed()
    {
        GameManager.Instance.LoadScene("Title");
    }
}
