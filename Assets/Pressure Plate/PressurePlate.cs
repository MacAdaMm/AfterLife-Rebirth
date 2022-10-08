using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] public Sprite _inactiveSprite;
    [SerializeField] public Sprite _activeSprite;
    
    [field: SerializeField] public UnityEvent OnTriggered { get; private set; }
    public GameObject TriggeringObject { get; protected set; }
    public bool IsPressed { get => TriggeringObject != null; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPressed)
        {
            TriggeringObject = collision.gameObject;
            _renderer.sprite = _activeSprite;
            OnTriggered.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == TriggeringObject)
        {
            TriggeringObject = null;
            _renderer.sprite = _inactiveSprite;
        }
    }
}
