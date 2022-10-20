using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SPCinemachineZone : MonoBehaviour
{
    [SerializeField] 
    private CinemachineVirtualCamera _virtualCamera;

    [field: SerializeField]
    public UnityEvent OnZoneEnter { get; private set; }

    [field: SerializeField] 
    public UnityEvent OnZoneExit { get; private set; }

    private static SPCinemachineZone _currentActiveZone;

    private void ActivateZone()
    {
        if (_currentActiveZone == this)
        {
            return;
        }

        _virtualCamera.gameObject.SetActive(true);
        OnZoneEnter.Invoke();

        if (_currentActiveZone != null)
        {
            _currentActiveZone.DisableZone();
        }

        _currentActiveZone = this;
    }

    private void DisableZone()
    {
        if (_currentActiveZone == this)
        {
            _currentActiveZone = null;
        }

        OnZoneExit.Invoke();
        _virtualCamera.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateZone();
        }
    }

    private void Awake()
    {
        _virtualCamera.gameObject.SetActive(false);
    }
}