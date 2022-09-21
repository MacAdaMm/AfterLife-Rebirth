using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Current { get; private set; }

    public GameObject playerObject;
    public Transform SpawnPoint;
    Health playerHealth;

    public void Awake()
    {
        Current = this;
    }
    public void OnDestroy()
    {
        Current = null;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        var player = Instantiate(playerObject);
        player.transform.position = SpawnPoint.position;
        if (player.TryGetComponent(out playerHealth))
        {
            playerHealth.OnDeath += OnGameOverEvent;
        }

    }
    private void OnGameOverEvent()
    {
        throw new System.NotImplementedException();
    }

    public void Pause()
    {

    }
    public void Unpause()
    {

    }

}
