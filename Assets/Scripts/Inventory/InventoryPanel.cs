using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InventoryPanel: MonoBehaviour
{
    //[SerializeField] private InventoryData inventoryData;
    [SerializeField] private Transform itemGrid;
    [SerializeField] private ItemSlot itemSlotPrefab;
    private InventoryData inventoryData;

    private void OnEnable()
    {
        TryBindInventoryData();
    }

    private void Update()
    {
        // Player 可能比 UI 晚生成
        if (inventoryData == null)
        {
            TryBindInventoryData();
        }
    }

    private void OnDisable()
    {
        inventoryData = null;
    }

    private void TryBindInventoryData()
    {
        if (NetworkManager.Singleton == null)
            return;

        if (!NetworkManager.Singleton.IsClient)
            return;

        NetworkObject playerObject =
            NetworkManager.Singleton.LocalClient?.PlayerObject;

        if (playerObject == null)
            return;

        InventoryData data = playerObject.GetComponent<InventoryData>();

        if (data == null)
        {
            Debug.LogError("InventoryData not found on local Player.");
            return;
        }

        inventoryData = data;

        Refresh();
    }

    private void Refresh()
    {
        if (inventoryData == null)
            return;

        foreach (Transform child in itemGrid)
        {
            ItemSlot slot = child.GetComponent<ItemSlot>();

            if (slot != null)
            {
                slot.ReleaseIcon();
            }

            Destroy(child.gameObject);
        }

        foreach (InventoryItem item in inventoryData.Items)
        {
            ItemSlot slot = Instantiate(itemSlotPrefab, itemGrid);

            slot.SetItem(
                item.itemData,
                item.quantity
            );
        }
    }
}
