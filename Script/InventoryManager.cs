using System.Collections.Generic;
using UnityEngine;
public class InventoryManager : MonoBehaviour, ICollectible
{
    [SerializeField] private List<InventoryItem> _listItems = new List<InventoryItem>();
    private Character _character;
    private void Awake()
    {
        _character = GetComponent<Character>();
    }
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
    public void UseItem(InventoryItem inventoryItem)
    {
        if (inventoryItem == null)
        {
            return;
        }
        
    }
}
