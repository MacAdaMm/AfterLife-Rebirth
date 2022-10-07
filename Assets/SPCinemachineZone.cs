using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPCinemachineZone : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private static SPCinemachineZone _currentActiveZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateZone();
        }
    }
    
    private void ActivateZone()
    {
        if (_currentActiveZone == this)
        {
            return;
        }

        _virtualCamera.gameObject.SetActive(true);

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

        _virtualCamera.gameObject.SetActive(false);
    }

    private void Awake()
    {
        _virtualCamera.gameObject.SetActive(false);
    }
}