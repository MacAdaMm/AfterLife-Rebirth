using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour, ISaveable
{
    /* 
     * BUG: When theres a save handler on this GameObject and an item has spawned, If you dont "pick up" the item before we reload you can never pick up the item right now...
     * 
     * NOTE: We could add some state to the spawner or maybe a sub class of it for items to handle this case, but then the item would have to know about the spawner.
     * NOTE: We could also save the item state if it hasn't been picked up, But then we will have to handle that some other way as theres no object created when the level is loaded.
     * EXAMPLE: If an item isn't picked up in a level, maybe the level manager holds a ref to each item that is importent enough to save. Then on load it recreates each item somehow.
    */

    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    private GameObject _spawnableParentObject;
    [SerializeField]
    private bool _disableOnSpawn;

    [field: SerializeField]
    public UnityEvent OnSpawnPrefabUnityEvent { get; }

    public object CaptureState()
    {
        return enabled;
    }
    public void RestoreState(object state)
    {
        enabled = (bool)state;
    }

    public void SpawnPrefab()
    {
        SpawnPrefab(_prefab);
    }
    public void SpawnPrefab(GameObject prefab)
    {
        if (enabled)
        {
            if (_disableOnSpawn)
            {
                enabled = false;
            }

            GameObject go = _spawnableParentObject ? Instantiate(prefab, _spawnableParentObject.transform) : Instantiate(prefab);
            go.transform.position = transform.position;

            OnSpawnPrefabUnityEvent?.Invoke();
            _particleSystem?.Play();

        }
    }
}
