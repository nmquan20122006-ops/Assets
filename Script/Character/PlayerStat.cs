using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Character
{   
    private Animator animator;
    private AttackController attackController;
    public static PlayerStat Instance { get; private set; }
    public GameObject PlayerGameObject => gameObject;
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    // Start is called before the first frame update
    protected override void Start()
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
        if (!isDead)
        {
            animator.SetTrigger("TakeHit");
            if (attackController != null)
            {
                attackController.Interrupt();
            }
        }
    }
}
