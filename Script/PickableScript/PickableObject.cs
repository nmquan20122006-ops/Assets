using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private PlayerWeaponData weaponData;
    private SpriteRenderer spriteRenderer;
    private Collider2D itemCollider;
    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = weaponData.worldSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible icollecible = collision.GetComponent<ICollectible>();
        if (icollecible != null)
        {
            if (icollecible.Collect(weaponData))
            {
                Destroy(gameObject);
            }
        }
    }
}
