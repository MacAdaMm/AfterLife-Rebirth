using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    public static event Action<SavePoint> OnSavePointActivated;

    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;

    private void Awake()
    {
        //NOTE: Beacuse the LevelManager is destroyed in each scene we never have to remove the callback as it gets removed on the next scene load.
        //FindObjectOfType<LevelManager>().OnPlayerSpawn += OnPlayerSpawn;

        OnSavePointActivated += ChangeActiveSavePoint;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDestroy()
    {
        OnSavePointActivated -= ChangeActiveSavePoint;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        OnSavePointActivated.Invoke(this);
        GameManager.Instance.SetCheckpoint(this);
        SaveManager.Save();
    }

    private void ChangeActiveSavePoint(SavePoint savePoint)
    {
        var isActiveSavePoint = savePoint == this;
        gameObject.GetComponent<SpriteRenderer>().sprite = isActiveSavePoint ? _activeSprite : _inactiveSprite;        
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if(sceneMode != LoadSceneMode.Additive && TryGetComponent(out LevelEntryPoint entryPoint) && entryPoint.Id == GameManager.Instance.CurrentCheckpointId)
        {
            ChangeActiveSavePoint(this);
        }
        
    }
}
