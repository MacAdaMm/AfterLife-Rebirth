using ShadyPixel.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelState
{
    Gameplay,
    Paused,
    GameOver
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Current { get; private set; }

    public GameObject PlayerInstance { get; private set; }

    public Health PlayerHealth => playerHealth;
    private Health playerHealth;

    public LevelState CurrentState { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private bool spawnPlayerOnStart = true;

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("UI")]
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject deathScreen;

    public void Pause()
    {
        if(CurrentState == LevelState.GameOver) { return; }

        Debug.Log("Pause");
        Time.timeScale = 0f;
        CurrentState = LevelState.Paused;
        pauseScreen.SetActive(true);
    }

    public void Unpause()
    {
        if (CurrentState == LevelState.GameOver) { return; }

        Debug.Log("Unpause");
        Time.timeScale = 1f;
        CurrentState = LevelState.Gameplay;
        pauseScreen.SetActive(false);
    }

    private void Awake()
    {
        Current = this;

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
        if (spawnPlayerOnStart)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        PlayerInstance = Instantiate(playerPrefab);
        PlayerInstance.transform.position = spawnPoint.position;
        if (PlayerInstance.TryGetComponent(out playerHealth))
        {
            playerHealth.OnDeath += OnPlayerDeath;
        }
        
        CurrentState = LevelState.Gameplay;
        hud.SetActive(true);
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Game Over");
        deathScreen.SetActive(true);
        CurrentState = LevelState.GameOver;
    }

    private void Update()
    {
        if (InputManager.InputActions.Player.Pause.WasPerformedThisFrame())
        {
            if(CurrentState == LevelState.Gameplay)
            {
                Pause();
            }
            else if(CurrentState == LevelState.Paused)
            {
                Unpause();
            }
        }
    }
}