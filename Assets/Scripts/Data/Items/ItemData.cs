using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public AssetReferenceSprite icon;
    public string description;
    public int maxStack = 99;
}
