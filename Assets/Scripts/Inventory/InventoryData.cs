using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int quantity;
}

public class InventoryData : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

    public IReadOnlyList<InventoryItem> Items => items;

    [SerializeField] private ItemData healthPotion;
    [SerializeField] private ItemData ironSword;

    private void Start()
    {
        AddItem(healthPotion, 5);
        AddItem(ironSword, 1);
    }


    public void AddItem(ItemData itemData, int quantity)
    {
        if (itemData == null || quantity <= 0)
            return;

        InventoryItem existingItem = items.Find(item => item.itemData == itemData);

        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else
        {
            items.Add(new InventoryItem
            {
                itemData = itemData,
                quantity = quantity
            });
        }
    }

    public void RemoveItem(ItemData itemData, int quantity)
    {
        if (itemData == null || quantity <= 0)
            return;

        InventoryItem existingItem = items.Find(item => item.itemData == itemData);

        if (existingItem == null)
            return;

        existingItem.quantity -= quantity;

        if (existingItem.quantity <= 0)
        {
            items.Remove(existingItem);
        }
    }
}
