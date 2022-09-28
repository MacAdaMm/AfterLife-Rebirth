using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        GameManager.Instance.SetCheckpoint(this);
        //LevelManager.Current.SpawnPoint = transform;

        Debug.Log("Saving Game...");
        SaveManager.Save();
    }
}
