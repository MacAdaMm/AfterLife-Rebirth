using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationShaker : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField]
    private float _duration = 0.1f;

    [SerializeField]
    private Vector3 _strength = new Vector3(0f,0f,45f);

    [SerializeField]
    private int _vibrato = 10;

    [SerializeField]
    private float _randomness = 90f;

    [SerializeField]
    private bool _fadeOut = true;

    [SerializeField]
    private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;

    public void Shake()
    {
        _target.DOShakeRotation(_duration, _strength, _vibrato, _randomness, _fadeOut, _randomnessMode);
    }
}
