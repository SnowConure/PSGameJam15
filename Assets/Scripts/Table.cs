using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public static Table i;
    private void Awake()
    {
        i = this;
    }

    // prefab
    public GameObject ItemPrefab;

    // Item Objects on table
    public List<ItemObject> itemObjects = new List<ItemObject>();

    public static void RemoveItemFromTable(ItemObject item)
    {
        i.itemObjects.Remove(item);
        Destroy(item.gameObject);
    }

    public static void AddItemToTable(ItemInstance item)
    {
        i.CreateNewItemObject(item);
    }

    public static void TakeItemFromBag()
    {
        ItemInstance item = Bag.DrawRandomItem();
        if (item == null) return;

        i.CreateNewItemObject(item);
    }

    void CreateNewItemObject(ItemInstance item)
    {
        ItemObject newItem = Instantiate(ItemPrefab, new Vector3(Random.Range(-3.7f, 4.0f), -1, 0), Quaternion.identity).GetComponent<ItemObject>(); 
        newItem.Setup(item);
        itemObjects.Add(newItem);
        newItem.ItemUsed += ItemUsed;
    }

    // when an item is used and has performed its effect.
    // THIS SHOULD ALWAYS BE THE LAST THING THAT HAPPENS AFTER AN ITEM HAS BEEN PLAYED
    void ItemUsed(ItemObject item)
    {
        // remove the item used
        RemoveItemFromTable(item);

        // if there is less than five ingridients on the table, add until there are.
        while (i.itemObjects.Count < 5)
        {
            if (Bag.inventory.Count <= 0)
                break;

            TakeItemFromBag();
        }

        // check for win
        Player.i.CheckWin();
    }

    // brings selected item to front. re-ordering the sort order.
    public static void BringToFront(ItemObject item)
    {
        SpriteRenderer itemRenderer = item.gameObject.GetComponent<SpriteRenderer>();
        int selectedOrder = itemRenderer.sortingOrder;
        foreach (var itemObject in i.itemObjects)
        {
            SpriteRenderer renderer = itemObject.gameObject.GetComponent<SpriteRenderer>();
            if (renderer.sortingOrder > selectedOrder)
                renderer.sortingOrder--;
        }
        itemRenderer.sortingOrder = i.itemObjects.Count;
    }
}


