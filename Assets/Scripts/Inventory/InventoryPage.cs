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
    public Sprite image, image2;
    public int quantity;

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
            Debug.Log(inventoryItem.name);
            listOfItems[0].Select();
        }

        private void HandleBeginDrag(InventoryItem inventoryItem)
        {
            int index = listOfItems.IndexOf(inventoryItem);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            mouseFollower.Toggle(true);
            mouseFollower.SetData(index == 0 ? image : image2, quantity);
        }

        private void HandleSwap(InventoryItem inventoryItem)
        {
            int index = listOfItems.IndexOf(inventoryItem);
            if (index == -1)
            {
                mouseFollower.Toggle(false);
                currentlyDraggedItemIndex = -1;
                return;
            }
            listOfItems[currentlyDraggedItemIndex].SetData(index == 0 ? image : image2, quantity);
            listOfItems[index].SetData(currentlyDraggedItemIndex == 0 ? image : image2, quantity);
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleEndDrag(InventoryItem inventoryItem)
        {
            mouseFollower.Toggle(false);
        }

        private void HandleShowItemActions(InventoryItem inventoryItem)
        {
            
        }


    public void Show()
        {
            gameObject.SetActive(true);
            //ResetSelection();
            listOfItems[0].SetData(image, quantity);
            listOfItems[1].SetData(image2, quantity);
        }

    public void Hide()
        {
            //actionPanel.Toggle(false);
            gameObject.SetActive(false);
            //ResetDraggedItem();
        }
    
}
