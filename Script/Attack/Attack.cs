using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator animator;
    private Weapon weapon;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (animator != null)
            {   
                animator.SetTrigger("Attack1");

            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack2");
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack3");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (animator != null)
            {
                animator.SetTrigger("Dead");
            }
        }
    }

}
