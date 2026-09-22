using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;

    private ItemData itemData;
    private int quantity;

    private AsyncOperationHandle<Sprite> iconHandle;
    private bool hasHandle = false;

    public void SetItem(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;

        quantityText.text = quantity.ToString();

        icon.sprite = null;

        if (itemData == null || itemData.icon == null)
            return;

        iconHandle = itemData.icon.LoadAssetAsync<Sprite>();

        if (!iconHandle.IsValid())
        {
            Debug.LogError(
                $"Failed to create valid handle for icon: {itemData.name}"
            );
            return;
        }

        hasHandle = true;

        iconHandle.Completed += OnIconLoaded;
    }

    private void OnIconLoaded(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Icon loaded successfully: {handle.Result.name}");

            if (icon != null)
            {
                icon.sprite = handle.Result;
            }
        }
        else
        {
            Debug.LogError(
                $"Failed to load icon: {itemData.name}"
            );
        }
    }

    private void OnDestroy()
    {
        ReleaseIcon();
    }

    public void ReleaseIcon()
    {
        if (hasHandle && iconHandle.IsValid())
        {
            Addressables.Release(iconHandle);
        }

        hasHandle = false;
    }
}
