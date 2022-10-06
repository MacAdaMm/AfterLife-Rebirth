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
        OnSavePointActivated += ChangeActiveSavePoint;

        if (TryGetComponent(out LevelEntryPoint entryPoint) && entryPoint.Id == GameManager.Instance.CurrentCheckpointId)
        {
            ChangeActiveSavePoint(this);
        }
    }
    private void OnDestroy()
    {
        OnSavePointActivated -= ChangeActiveSavePoint;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if(collision.TryGetComponent(out Health health))
        {
            health.HealMax();
        }
        GameManager.Instance.SetCheckpoint(this);
        OnSavePointActivated.Invoke(this);
        SaveManager.Save();
    }

    private void ChangeActiveSavePoint(SavePoint savePoint)
    {
        var isActiveSavePoint = savePoint == this;
        gameObject.GetComponent<SpriteRenderer>().sprite = isActiveSavePoint ? _activeSprite : _inactiveSprite;
    }
}
