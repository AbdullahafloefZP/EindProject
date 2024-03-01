using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Page;
using Inventory.Model;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryPage inventory;
        [SerializeField] private InventorySO inventoryData;
        public List<InventoryItemz> initialItems = new List<InventoryItemz>();

        private void Start()
        {
            prepareUI();
            prepareInventoryData();
        }

        private void prepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventory;
            foreach (InventoryItemz item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventory(Dictionary<int, InventoryItemz> inventoryState)
        {
            inventory.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventory.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void prepareUI()
        {
            inventory.InitializeInventory(inventoryData.Size);
            this.inventory.OnDescriptionRequested += HandleDescriptionRequest;
            this.inventory.OnSwapItems += HandleSwapItems;
            this.inventory.OnStartDragging += HandleDragging;
            this.inventory.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {

        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItemz inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventory.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
            {
                InventoryItemz inventoryItem = inventoryData.GetItemAt(itemIndex);
                if (inventoryItem.IsEmpty)
                {
                    inventory.ResetSelection();
                    return;
                }
                ItemSO item = inventoryItem.item;
                inventory.UpdateDescription(itemIndex, item.ItemImage, item.name);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventory.isActiveAndEnabled == false)
                {
                    inventory.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventory.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                }
                else
                {
                    inventory.Hide();
                }
            }
        }

    }
}