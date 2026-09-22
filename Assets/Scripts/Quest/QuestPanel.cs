using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class QuestPanel : MonoBehaviour
{
    //[SerializeField] private QuestSystem questSystem;
    [SerializeField] private Transform questList;
    [SerializeField] private QuestItem questItemPrefab;
    [SerializeField] private RedDot redDot;

    private QuestSystem questSystem;

    private void OnEnable()
    {
        TryBindQuestSystem();
    }

    private void Update()
    {
        if (questSystem == null)
        {
            TryBindQuestSystem();
        }
    }

    private void OnDisable()
    {
        if (questSystem != null)
        {
            questSystem.OnQuestUpdated -= OnQuestUpdated;
            questSystem = null;
        }
    }

    private void TryBindQuestSystem()
    {
        if (NetworkManager.Singleton == null)
            return;

        if (!NetworkManager.Singleton.IsClient)
            return;

        NetworkObject playerObject =
            NetworkManager.Singleton.LocalClient?.PlayerObject;

        if (playerObject == null)
            return;

        QuestSystem system = playerObject.GetComponent<QuestSystem>();

        if (system == null)
        {
            Debug.LogError("QuestSystem not found on local Player.");
            return;
        }

        questSystem = system;

        questSystem.OnQuestUpdated += OnQuestUpdated;

        redDot.Hide();
        Refresh();
    }

    private void OnQuestUpdated(QuestInstance quest)
    {
        Refresh();
    }

    private void Refresh()
    {
        if (questSystem == null)
            return;

        foreach (Transform child in questList)
        {
            Destroy(child.gameObject);
        }

        foreach (QuestInstance quest in questSystem.Quests)
        {
            QuestItem item = Instantiate(questItemPrefab, questList);

            item.SetQuest(quest);
        }
    }
}
