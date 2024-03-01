using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private List<InventoryItemz> inventoryItems;
        [field: SerializeField] public int Size { get; private set; } = 10;
        public event Action<Dictionary<int, InventoryItemz>> OnInventoryUpdated;
    
        public void Initialize()
        {
            inventoryItems = new List<InventoryItemz>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItemz.GetEmptyItem());
            }
        }
    
        public int AddItem(ItemSO item, int quantity)
        {
            if(item.IsStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }
            quantity = AddStackableItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity)
        {
            InventoryItemz newItem = new InventoryItemz{item = item, quantity = quantity};
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull()
            => inventoryItems.Where(item => item.IsEmpty).Any() == false;

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                if (inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake =
                        inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i]
                            .ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i]
                            .ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            while(quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
            return quantity;
        }

        public void AddItem(InventoryItemz item)
        {
            AddItem(item.item, item.quantity);
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItemz item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
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


