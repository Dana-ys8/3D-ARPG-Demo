using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private AttackData attackData;

    private Animator animator;
    private bool isAttacking;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("attack");
    }

    // Animation Event µ÷ÓÃ
    public void FinishAttack()
    {
        isAttacking = false;
    }
}
