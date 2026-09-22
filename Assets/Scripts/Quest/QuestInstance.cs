using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class QuestInstance
{
    public QuestData questData;
    public int currentCount;

    public bool IsCompleted => currentCount >= questData.targetCount;

    public QuestInstance(QuestData questData)
    {
        this.questData = questData;
        currentCount = 0;
    }
}
