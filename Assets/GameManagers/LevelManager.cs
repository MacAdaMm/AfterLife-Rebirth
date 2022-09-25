using ShadyPixel.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelState
{
    Initialization,
    Gameplay,
    Paused,
    GameOver
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Current { get; private set; }

    /// <summary> REDO IN FUTURE
    /// Reference to player and health probably doesnt need to live in level manager.
    /// Might need to replace with player singleton eventually.
    /// </summary>
    public GameObject PlayerInstance { get; private set; }
    public Health PlayerHealth => playerHealth;
    private Health playerHealth;

    public LevelState CurrentState { get; private set; }

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("UI")]
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject deathScreen;

    public void Pause()
    {
        if (CurrentState != LevelState.Gameplay) { return; }

        Debug.Log("Pause");
        Time.timeScale = 0f;
        CurrentState = LevelState.Paused;
        pauseScreen.SetActive(true);
    }

    public void Unpause()
    {
        if (CurrentState != LevelState.Paused) { return; }

        Debug.Log("Unpause");
        Time.timeScale = 1f;
        CurrentState = LevelState.Gameplay;
        pauseScreen.SetActive(false);
    }

    private void Awake()
    {
        Current = this;

        CurrentState = LevelState.Initialization;
        hud.SetActive(false);
        pauseScreen.SetActive(false);
        deathScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        Current = null;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        PlayerInstance = Instantiate(playerPrefab);
        PlayerInstance.transform.position = spawnPoint.position;
        PlayerInstance.name = playerPrefab.name;

        if (PlayerInstance.TryGetComponent(out playerHealth))
        {
            playerHealth.OnDeath += OnPlayerDeath;
        }
        
        CurrentState = LevelState.Gameplay;
        hud.SetActive(true);
        Time.timeScale = 1f;
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Game Over");
        deathScreen.SetActive(true);
        CurrentState = LevelState.GameOver;
    }

    private void Update()
    {
        HandlePlayerInput();
    }

    private void HandlePlayerInput()
    {
        if (InputManager.InputActions.Player.Pause.WasPerformedThisFrame())
        {
            if (CurrentState == LevelState.Gameplay)
            {
                Pause();
            }
            else if (CurrentState == LevelState.Paused)
            {
                Unpause();
            }
        }
    }
}