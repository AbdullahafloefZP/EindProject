using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private List<InventoryItemz> inventoryItems;
        [field: SerializeField] public int Size { get; private set; } = 10;
    
        public void Initialize()
        {
            inventoryItems = new List<InventoryItemz>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItemz.GetEmptyItem());
            }
        }
    
        public void AddItem(ItemSO item, int quantity)
        {
            // if(item.IsStackable == false)
            // {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    if (inventoryItems[i].IsEmpty)
                    {
                        inventoryItems[i] = new InventoryItemz{item = item, quantity = quantity};
                    }
                    // while(quantity > 0 && IsInventoryFull() == false)
                    // {
                    //     quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                    // }
                    // InformAboutChange();
                    // return quantity;
                }
            // }
            // quantity = AddStackableItem(item, quantity);
            // InformAboutChange();
            // return quantity;
        }
    
        public Dictionary<int, InventoryItemz> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItemz> returnValue =
                new Dictionary<int, InventoryItemz>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }
    
        public InventoryItemz GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }
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
}


