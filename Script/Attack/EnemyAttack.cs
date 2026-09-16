using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAttack : MonoBehaviour
{
    [field: SerializeField] public float attackCooldown {  get; set; }
    [field: SerializeField] public int attackDamage { get; set; }
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackRange = 0.5f;
    [field: SerializeField] public bool isAttackFinished { get; set; }
    private Animator animator;
    private Enemy enemy;
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        attackHitbox.SetActive(false);
    }
    public void AttackLogic()
    {
        isAttackFinished = false;
        animator.SetTrigger("Attack");
    }
    public void EnableAttack()
    {
        attackHitbox.SetActive(true);
    }
    public void DisableAttack()
    {
        attackHitbox.SetActive(false);
    }
    public void OnAttackFinished()
    {
        isAttackFinished = true;
    }
}