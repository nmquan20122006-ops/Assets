using UnityEngine;
public enum ItemType { Consumable, Currency, Equipment }

[CreateAssetMenu(menuName = "Item/items")]
public class ItemSO : ScriptableObject
{
    [Header("Basic info")]
    [SerializeField] string _itemid;
    [SerializeField] string _itemName;
    [SerializeField] Sprite _itemSprite;
    [SerializeField] string _description;

    [Header("Item properties")]
    [SerializeField] bool _isStackable;
    [SerializeField] private int _maxStackSize;
    [SerializeField] private ItemType _itemType;
    public string itemId => _itemid;
    public string itemName => _itemName;
    public Sprite itemSprite => _itemSprite;
    public string description => _description;
    public bool isStackable => _isStackable;
    public int maxStackSize => _maxStackSize;

    public virtual bool Use(GameObject user)
    {
        return false;
    }
}
