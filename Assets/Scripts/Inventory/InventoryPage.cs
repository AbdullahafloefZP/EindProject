using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private MouseFollower mouseFollower;

    List<InventoryItem> listOfItems = new List<InventoryItem>();
    private int currentlyDraggedItemIndex = -1;
    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
    public event Action<int, int> OnSwapItems;

    public void InitializeInventory(int inventorysize)
        {
            for (int i = 0; i < inventorysize; i++)
            {
                InventoryItem Item =
                    Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                Item.transform.SetParent(contentPanel);
                listOfItems.Add(Item);
                Item.OnItemClicked += HandleItemSelection;
                Item.OnItemBeginDrag += HandleBeginDrag;
                Item.OnItemDroppedOn += HandleSwap;
                Item.OnItemEndDrag += HandleEndDrag;
                Item.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
        }

        private void HandleItemSelection(InventoryItem inventoryItem)
        {
            int index = listOfItems.IndexOf(inventoryItem);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        private void HandleBeginDrag(InventoryItem inventoryItem)
        {
            int index = listOfItems.IndexOf(inventoryItem);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItem);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleSwap(InventoryItem inventoryItem)
        {
            int index = listOfItems.IndexOf(inventoryItem);
            if (index == -1)
            {
                
                return;
            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItem);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
            return;
        }

        private void HandleEndDrag(InventoryItem inventoryItem)
        {
            ResetDraggedItem();
        }

        private void HandleShowItemActions(InventoryItem inventoryItem)
        {
            
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfItems.Count > itemIndex)
            {
                listOfItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }


    public void Show()
        {
            gameObject.SetActive(true);
        }

    public void Hide()
        {
            //actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }
    
}
