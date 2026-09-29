using UnityEngine;
[System.Serializable]
public class InventoryItem 
{   
    [SerializeField] private ItemSO _data;
    [SerializeField] private int _stackSize;
    public ItemSO data => _data;
    public int stackSize => _stackSize;
    public bool isFullStack => _data != null && _stackSize >= _data.maxStackSize;
    public InventoryItem(ItemSO itemData, int amount =1)
    {
        _data = itemData; _stackSize = amount;
    }
    public void AddStack(int amount)
    {
        _stackSize += amount;
    }
    public void RemoveStack(int amount)
    {
        _stackSize -= amount;
        if (_stackSize <= 0) _stackSize = 0;
    }
}
