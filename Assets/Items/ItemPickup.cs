using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemPickup<T> : MonoBehaviour where T : ItemDataSO
{
    [SerializeField]
    protected T _itemData;

    protected abstract void OnTriggerEnter2D(Collider2D collision);

}
