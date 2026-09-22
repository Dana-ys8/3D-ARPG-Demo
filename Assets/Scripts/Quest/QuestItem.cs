using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestItem: MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Slider progressSlider;

    public void SetQuest(QuestInstance quest)
    {
        questNameText.text = quest.questData.questName;
        progressSlider.minValue = 0;
        progressSlider.maxValue = 1;
        float progress = (float)quest.currentCount / quest.questData.targetCount;
        progressSlider.value = progress;

        progressText.text =$"{quest.currentCount}/{quest.questData.targetCount}";
    }
}
