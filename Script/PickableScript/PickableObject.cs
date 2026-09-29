using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickableObject : MonoBehaviour
{
    [SerializeField] private ItemSO item;
    private SpriteRenderer spriteRenderer;
    private Collider2D itemCollider;
    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = item.itemSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Debug.Log("Collision với: " + collision.name);
        ICollectible icollecible = collision.GetComponent<ICollectible>();
        if (icollecible == null)
        {
            Debug.Log("Không có ICollectible");
        }
        if (icollecible != null)
        {
            Debug.Log("Có ICollectible");
            if (icollecible.Collect(item))
            {
                Debug.Log("Nhặt thành công");
                Destroy(gameObject);
            }
        }
    }
}
