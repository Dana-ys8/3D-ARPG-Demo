using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Game/Quest Data")]
public class QuestData: ScriptableObject
{
    public string questName;

    [TextArea]
    public string description;

    public int targetCount = 1;

    public int rewardGold = 0;
}