using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntry : MonoBehaviour
{
    [field: SerializeField]
    public string Id { get; private set; }

    [field: SerializeField]
    public Transform SpawnOffset { get; private set; } 
}