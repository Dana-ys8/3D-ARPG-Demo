using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestSystem : MonoBehaviour
{
    [SerializeField]
    private List<QuestInstance> quests = new List<QuestInstance>();

    [SerializeField]
    private QuestData testQuest;

    public event Action<QuestInstance> OnQuestUpdated;

    private void Start()
    {
        AddQuest(testQuest);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            UpdateProgress(testQuest, 1);
        }
    }

    public void AddQuest(QuestData questData)
    {
        if (questData == null)
            return;

        QuestInstance existingQuest = quests.Find(quest => quest.questData == questData);

        if (existingQuest != null)
            return;

        quests.Add(new QuestInstance(questData));
    }

    public IReadOnlyList<QuestInstance> Quests => quests;

    public void UpdateProgress(QuestData questData, int amount)
    {
        if (questData == null || amount <= 0)
            return;

        QuestInstance quest = quests.Find(item => item.questData == questData);

        if (quest == null)
            return;

        if (quest.IsCompleted)
            return;

        quest.currentCount += amount;

        if (quest.currentCount > quest.questData.targetCount)
        {
            quest.currentCount = quest.questData.targetCount;
        }

        OnQuestUpdated?.Invoke(quest);

        Debug.Log(
            $"{quest.questData.questName}: " +
            $"{quest.currentCount}/{quest.questData.targetCount}"
        );
    }
}
