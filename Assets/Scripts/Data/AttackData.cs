using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Game/Attack Data")]
public class AttackData : ScriptableObject
{
    public int damage = 20;
    public float attackRange = 2f;
    public float attackRadius = 1f;
}
