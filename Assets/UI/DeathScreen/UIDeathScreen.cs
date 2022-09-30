using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDeathScreen : UIMenu
{
    [Header("Respawn")]
    [SerializeField] private Button respawnButton;

    [Header("Main Menu")]
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        respawnButton.onClick.AddListener(OnRespawnButtonPressed);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
    }


    private void OnDestroy()
    {
        respawnButton.onClick.RemoveListener(OnRespawnButtonPressed);
        mainMenuButton.onClick.RemoveListener(OnMainMenuButtonPressed);
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
