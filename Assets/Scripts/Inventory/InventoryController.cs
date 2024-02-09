using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryPage inventory;
    public int inventorySize = 10;

    private void Start()
    {
        inventory.InitializeInventory(inventorySize);
        // PrepareUI();
        // PrepareInventoryData();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventory.isActiveAndEnabled == false)
            {
                inventory.Show();
                // foreach (var item in inventoryData.GetCurrentInventoryState())
                // {
                //     inventory.UpdateData(item.Key,
                //     item.Value.item.ItemImage,
                //     item.Value.quantity);
                // }
            }
            else
            {
                inventory.Hide();
            }
        }
    }
        
}
