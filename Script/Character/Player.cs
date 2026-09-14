using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{   
    private Animator animator;
    private AttackController attackController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (!IsDead())
        {
            animator.SetTrigger("TakeHit");
            if (attackController != null)
            {
                attackController.ResetCombo();
            }
        }
    }
}
