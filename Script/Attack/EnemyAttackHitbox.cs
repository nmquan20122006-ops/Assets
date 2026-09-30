using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    private EnemyAttack enemyAttack;
    void Start()
    {
        enemyAttack = GetComponentInParent<EnemyAttack>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStat player = other.GetComponent<PlayerStat>();
            if (player != null)
            {
                player.TakeDamage(enemyAttack.attackDamage);
            }
        }
    }
}
