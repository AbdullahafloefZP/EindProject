using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;

    List<InventoryItem> listOfItems = new List<InventoryItem>();

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

        private void HandleItemSelection(InventoryItem obj)
        {
            Debug.Log(obj.name);
        }

        private void HandleBeginDrag(InventoryItem obj)
        {
            
        }

        private void HandleSwap(InventoryItem obj)
        {
            
        }

        private void HandleEndDrag(InventoryItem obj)
        {
            
        }

        private void HandleShowItemActions(InventoryItem obj)
        {
            
        }


    public void Show()
        {
            gameObject.SetActive(true);
            //ResetSelection();
        }

    public void Hide()
        {
            //actionPanel.Toggle(false);
            gameObject.SetActive(false);
            //ResetDraggedItem();
        }
    
}
