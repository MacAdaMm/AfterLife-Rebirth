using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = _activeSprite;
        GameManager.Instance.SetCheckpoint(this);
        SaveManager.Save();
    }
}
