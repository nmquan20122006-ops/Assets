using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickableObject : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    [SerializeField] private int amount;
    private SpriteRenderer spriteRenderer;
    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = item.itemSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<ICollectible>(out var collectible)) return;
        int amountAdd = collectible.Collect(item, amount);
        if (amountAdd > 0)
        {
            amount -= amountAdd;
            if (amount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
