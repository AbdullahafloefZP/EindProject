using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class Item : MonoBehaviour
{
    [field: SerializeField] public ItemSO InventoryItem { get; private set; }

    [field: SerializeField] public int Quantity { get; set; } = 1;


    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(DestroyItemRoutine());
    }

    private IEnumerator DestroyItemRoutine()
    {
        yield return new WaitForSeconds(0.1f); // Optional delay for audio to play

        Destroy(gameObject);
    }
}
