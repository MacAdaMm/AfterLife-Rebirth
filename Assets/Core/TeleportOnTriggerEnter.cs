using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportOnTriggerEnter : MonoBehaviour
{
    [SerializeField]
    private string _sceneName = "Title";

    [SerializeField]
    private string _levelEntryId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.LoadScene(_sceneName, _levelEntryId);
    }
}