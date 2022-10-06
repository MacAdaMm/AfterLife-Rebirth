using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionShaker : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField]
    private float _duration = 0.1f;

    [SerializeField]
    private float _strength = 1f;

    [SerializeField]
    private int _vibrato = 10;

    [SerializeField]
    private float _randomness = 90f;

    [SerializeField]
    private bool _snapping = false;

    [SerializeField]
    private bool _fadeOut = true;

    [SerializeField]
    private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;

    public void Shake()
    {
        _target.DOShakePosition(_duration, _strength, _vibrato, _randomness, _snapping, _fadeOut, _randomnessMode);
    }
}
