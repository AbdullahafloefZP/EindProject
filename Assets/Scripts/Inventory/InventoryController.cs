using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryPage inventory;
    [SerializeField] private InventorySO inventoryData;

    private void Start()
    {
        prepareUI();
        // inventoryData.Initialize();
        // PrepareUI();
        // PrepareInventoryData();
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
        
    }

    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        
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
