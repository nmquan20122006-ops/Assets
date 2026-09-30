using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private InventorySlot[] _slots;
    private GameObject _playerUser;
    public int slotAmount => _slots.Length;
    private void Awake()
    {
        if (_playerUser == null) _playerUser = PlayerStat.Instance.PlayerGameObject;
    }
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
        _inventoryManager.eInventoryChanged += UpdateUi;
        foreach(var slot in _slots)
        {
            slot.eOnSlotClicked += HandleSlotClicked;
        }
        UpdateUi();
    }
    private void OnDisable()
    {
        _inventoryManager.eInventoryChanged -= UpdateUi;
        foreach(var slot in _slots)
        {
            slot.eOnSlotClicked -= HandleSlotClicked;
        }
    }
    private void HandleSlotClicked(InventoryItem itemToUse)
    {
        if (itemToUse == null || _playerUser == null) return;
        _inventoryManager.UseItem(itemToUse, _playerUser);
    }
}
