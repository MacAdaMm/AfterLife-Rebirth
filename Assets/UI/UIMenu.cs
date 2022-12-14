using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenu : MonoBehaviour
{
    [Space]
    [SerializeField] private GameObject _defaultSelected;

    public bool IsVisable => gameObject.activeInHierarchy;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        if (EventSystem.current && _defaultSelected)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_defaultSelected);
        }
    }
}
