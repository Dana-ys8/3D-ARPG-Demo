using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public int maxHealth = 100;
}