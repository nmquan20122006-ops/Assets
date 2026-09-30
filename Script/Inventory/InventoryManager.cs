using System.Collections.Generic;
using UnityEngine;
public class InventoryManager : MonoBehaviour, ICollectible
{
    [SerializeField] private List<InventoryItem> _listItems = new List<InventoryItem>();
    public List<InventoryItem> listItems => _listItems;
    public System.Action eInventoryChanged;
    public int Collect(ItemSO item, int amount)
    {
        int remainingAmount = amount;
        Debug.Log("Nhận " + item.name);
        //xu ly item cung loai
        foreach (InventoryItem itemInInventory in _listItems)
        {
            if (itemInInventory.data == item && !itemInInventory.isFullStack)
            {
                int spaceLeft = item.maxStackSize - itemInInventory.stackSize;
                if (spaceLeft > 0)
                {
                    // nhet vua du khong tran stack
                    int amountToAdd = Mathf.Min(remainingAmount, spaceLeft);
                    itemInInventory.AddStack(amount);
                    eInventoryChanged?.Invoke();
                    remainingAmount -= amountToAdd;
                    if (remainingAmount <= 0) break;
                }
            }
        }
        //neu van con do va con slot
        int maxSlots = 32;

        while (remainingAmount > 0 && _listItems.Count < maxSlots)
        {
            int maxStack = item.maxStackSize;
            int amountForNewSlot = Mathf.Min(remainingAmount, maxStack);
            _listItems.Add(new InventoryItem(item, amountForNewSlot));
            remainingAmount -= amountForNewSlot;
        }

        int totalCollected = amount - remainingAmount;
        if (totalCollected > 0) eInventoryChanged?.Invoke();
        return totalCollected;
    }
    public bool IsRunOutOfItem(InventoryItem inventoryItem)
    {
        if (inventoryItem.stackSize <= 0)
        {
            _listItems.Remove(inventoryItem);
            return true;
        }
        return false;
    }
    public void UseItem(InventoryItem inventoryItem, GameObject user)
    {
        if (inventoryItem == null || inventoryItem.data == null
            || user == null || inventoryItem.stackSize <= 0) return;
        bool isUsed = inventoryItem.data.Use(user);
        if (isUsed)
        {
            inventoryItem.RemoveStack(1);
            IsRunOutOfItem(inventoryItem);
            eInventoryChanged?.Invoke();
        }
    }
}