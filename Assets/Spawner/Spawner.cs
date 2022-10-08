using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private ParticleSystem _particleSystem;

    [field: SerializeField]
    public UnityEvent OnSpawnPrefabUnityEvent { get; }
    public void SpawnPrefab()
    {
        SpawnPrefab(_prefab);
    }
    public void SpawnPrefab(GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        go.transform.position = this.transform.position;
        OnSpawnPrefabUnityEvent?.Invoke();
        _particleSystem?.Play();
    }
}
