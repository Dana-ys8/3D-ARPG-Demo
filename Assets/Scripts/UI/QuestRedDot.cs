using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class QuestRedDot : MonoBehaviour
{
    [SerializeField] private RedDot redDot;

    private QuestSystem questSystem;

    private void OnEnable()
    {
        TryBindQuestSystem();
    }

    private void Update()
    {
        // Player 可能比 UI 晚生成，所以持续尝试绑定
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
            return;

        questSystem = system;
        questSystem.OnQuestUpdated += OnQuestUpdated;
    }

    private void OnQuestUpdated(QuestInstance quest)
    {
        redDot.Show();
    }
}