using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField] private List<InventoryItemz> inventoryItems;
    [field: SerializeField] public int Size { get; private set; } = 10;
}

[Serializable]
public struct InventoryItemz
{
    public int quantity;
    public ItemSO item;
    public bool IsEmpty => item == null;
    public InventoryItemz ChangeQuantity(int newQuantity)
    {
        return new InventoryItemz
        {
            item = this.item,
            quantity = newQuantity,
        };
    }
    public static InventoryItemz GetEmptyItem()
        => new InventoryItemz
        {
            item = null,
            quantity = 0,
        };
}

