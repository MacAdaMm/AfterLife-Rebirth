using UnityEngine;

public class ItemDataSO : ScriptableObject
{
    [field: SerializeField]
    public string InGameName { get; protected set; }

    [field: SerializeField]
    public string Description { get; protected set; }

    [field: SerializeField]
    public Sprite InventorySprite { get; protected set; }
}