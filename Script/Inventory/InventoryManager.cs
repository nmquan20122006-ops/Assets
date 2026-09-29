using System.Collections.Generic;
using UnityEngine;
public class InventoryManager : MonoBehaviour, ICollectible
{
    [SerializeField] private List<InventoryItem> _listItems = new List<InventoryItem>();
    public List<InventoryItem> listItems => _listItems;
    public System.Action eInventoryChanged;
    public bool Collect(ItemSO item)
    {
        Debug.Log("Nhận " + item.name);
        foreach(InventoryItem itemInInventory in _listItems)
        {
            if (itemInInventory.data == item)
            {
                itemInInventory.AddStack(1);
                eInventoryChanged?.Invoke();
                return true;
            }
        }
        _listItems.Add(new InventoryItem(item, 1));
        eInventoryChanged?.Invoke();
        return true;
    }
    public void UseItem(InventoryItem inventoryItem, GameObject user)
    {
        if (inventoryItem == null || inventoryItem.data == null
            || user == null || inventoryItem.stackSize <= 0) return;
        bool isSuccess = inventoryItem.data.Use(user);
        if (isSuccess)
        {
            inventoryItem.RemoveStack(1);
            eInventoryChanged?.Invoke();
        }

    }
}
