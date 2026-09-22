using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCombatNet : NetworkBehaviour
{
    [SerializeField] private AttackData attackData;

    private Animator animator;
    private bool isAttacking;
    private OwnerNetworkAnimator networkAnimator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<OwnerNetworkAnimator>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        networkAnimator.SetTrigger("attack");
    }

    // Animation Event µ÷ÓÃ
    public void FinishAttack()
    {
        isAttacking = false;
    }
}
