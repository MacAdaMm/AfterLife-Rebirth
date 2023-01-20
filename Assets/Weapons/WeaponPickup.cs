using ShadyPixel.SaveLoad;
using UnityEngine;

[RequireComponent(typeof(SaveHandler))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class WeaponPickup : ItemPickup<WeaponDataSO>, ISaveable
{
    private bool _isCollected;

    public void RevealPickup()
    {
        if (!_isCollected)
        {
            gameObject.SetActive(true);
        }
    }
    public void Collect()
    {
        if (!_isCollected)
        {
            _isCollected = true;
            gameObject.SetActive(false);
            Debug.Log($"{GetType().Name} -> Collected {_itemData.InGameName}");
        }
    }
    public object CaptureState()
    {
        return _isCollected;
    }
    public void RestoreState(object state)
    {
        _isCollected = (bool)state;
        gameObject.SetActive(!_isCollected);

        if(!_isCollected && TryGetComponent(out ParticleSystem particleSystem))
        {
            particleSystem.Stop();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision) => Collect();
}
