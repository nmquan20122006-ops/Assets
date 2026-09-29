using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _amountText;
    private InventoryItem _currentItem;
    public void SetItem(InventoryItem inventoryItem)
    {
        _currentItem = inventoryItem;
        _itemImage.sprite = inventoryItem.data.itemSprite;
        _itemImage.enabled = true;

        _amountText.text = inventoryItem.stackSize.ToString();  
    }
    public void Clear()
    {
        _currentItem = null;
        _itemImage.sprite = null;
        _itemImage.enabled = false;
        _amountText.text = "";
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        draggableItem.parentAfterDrag = this.transform;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentItem == null) { return; }
        Debug.Log("Click slot: " + _currentItem.data.name);
    }
}
