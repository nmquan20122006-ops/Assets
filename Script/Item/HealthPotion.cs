using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Health Potion",menuName ="Inventory/Items/Health Potion")]
public class HealthPotion : ItemSO
{
    [SerializeField] private int _healAmount = 20;
    public override bool Use(GameObject user)
    {
        if (user == null) return false;
        if(user.TryGetComponent<IHealable>(out var healableTarget))
        {
            if (healableTarget.currentHealth >= healableTarget.maxHealth)
            {
                return false;
            }
            healableTarget.Heal(_healAmount);
            return true;
        }
        return false;
    }
}
