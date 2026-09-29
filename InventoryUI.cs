using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private InventorySlot[] _slots;
    private void Start()
    {
        UpdateUi();
    }
    public void UpdateUi()
    {
        Debug.Log("UPDATE UI - số item: " + _inventoryManager.listItems.Count);
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < _inventoryManager.listItems.Count)
            {
                Debug.Log("Set slot " + i);
                _slots[i].SetItem(_inventoryManager.listItems[i]);
            }
            else
            {
                _slots[i].Clear();
            }
        }
    }
    private void OnEnable()
    {
        Debug.Log("InventoryUI OnEnable");
        _inventoryManager.eInventoryChanged += UpdateUi;
        UpdateUi();
    }
    private void OnDisable()
    {
        _inventoryManager.eInventoryChanged -= UpdateUi;
    }
}
