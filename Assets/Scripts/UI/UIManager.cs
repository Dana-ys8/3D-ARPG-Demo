using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject questPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleQuest();
        }
    }

    public void ToggleInventory()
    {
        Debug.Log("Toggle Inventory");

        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void ToggleQuest()
    {
        questPanel.SetActive(!questPanel.activeSelf);
    }

    public bool IsUIOpen
    {
        get
        {
            return inventoryPanel.activeSelf || questPanel.activeSelf;
        }
    }
}
