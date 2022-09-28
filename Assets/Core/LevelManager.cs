using ShadyPixel.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private string _targetEntryPoint;

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

    [field: SerializeField] public Transform SpawnPoint { get; set; }

    [Header("UI")]
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject deathScreen;

    public event Action<GameObject> OnPlayerSpawn;

    public void Pause()
    {
        if (CurrentState != LevelState.Gameplay) { return; }

        Debug.Log("Pause");
        Time.timeScale = 0f;
        CurrentState = LevelState.Paused;
        pauseScreen.SetActive(true);
    }

    public void SetEntryPoint(string levelEntryPointId)
    {
        _targetEntryPoint = levelEntryPointId;
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

        var cams = FindObjectsOfType<Cinemachine.CinemachineVirtualCamera>();
        foreach (var cam in cams)
        {
            OnPlayerSpawn += (go) =>
            {
                cam.Follow = go.transform;
            };
        }
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
        var entryPoints = FindObjectsOfType<LevelEntry>();

        LevelEntry entryPoint = null;
        for (int i = 0; i < entryPoints.Length; i++)
        {
            if(entryPoints[i].Id == _targetEntryPoint)
            {
                entryPoint = entryPoints[i];
            }
        }
        //var entryPoint = entryPoints.Where((point) => point.Id == _targetEntryPoint).FirstOrDefault();
        var spawnPosition = SpawnPoint.position;

        if (entryPoint != null)
        {
            spawnPosition = entryPoint.SpawnOffset.position;
        }

        PlayerInstance.transform.position = spawnPosition;

        PlayerInstance.name = playerPrefab.name;

        if (PlayerInstance.TryGetComponent(out playerHealth))
        {
            playerHealth.OnDeath += OnPlayerDeath;
        }
        
        CurrentState = LevelState.Gameplay;
        hud.SetActive(true);
        Time.timeScale = 1f;
        OnPlayerSpawn?.Invoke(PlayerInstance);
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