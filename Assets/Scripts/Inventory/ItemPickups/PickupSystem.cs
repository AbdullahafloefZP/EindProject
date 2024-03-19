using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class PickupSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

    public GameObject pickupMessageUI;

    private bool canPickup;
    private Item currentItem;

    private void Start()
    {
        canPickup = false;
        HidePickupMessage();
    }

    private void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E) && currentItem != null)
        {
            int reminder = inventoryData.AddItem(currentItem.InventoryItem, currentItem.Quantity);
            if (reminder == 0)
                currentItem.DestroyItem();
            else
                currentItem.Quantity = reminder;
            
            HidePickupMessage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            canPickup = true;
            currentItem = item;
            ShowPickupMessage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            canPickup = false;
            currentItem = null;
            HidePickupMessage();
        }
    }

    private void ShowPickupMessage()
    {
        if (pickupMessageUI != null)
        {
            pickupMessageUI.SetActive(true);
        }
    }

    private void HidePickupMessage()
    {
        if (pickupMessageUI != null)
        {
            pickupMessageUI.SetActive(false);
        }
    }
}
