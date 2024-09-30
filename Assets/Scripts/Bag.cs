using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public static Action<ItemInstance> ItemAdded;

    public static List<ItemInstance> inventory = new List<ItemInstance>();

    public static void AddItem(ItemInstance item)
    {
        inventory.Add(item);
        ItemAdded?.Invoke(item);
    }

    public static void RemoveItem(ItemInstance item) 
    {
        inventory.Remove(item);
    }

    public static ItemInstance DrawRandomItem()
    {
        if(inventory.Count <= 0)
        {
            Debug.Log("no more items in bag");
            return null;
        }


        int rng = UnityEngine.Random.Range(0, inventory.Count);
        ItemInstance drawnItem = inventory[rng];
        inventory.RemoveAt(rng);

        return drawnItem;
    }



    public static ItemInstance CreateItem(ItemData itemData)
    {
        ItemInstance newItem = null;
        switch (itemData.EffectIndex)
        {
            case 1: newItem = new Item1(itemData); break;
            case 2: newItem = new Item2(itemData); break;
            case 3: newItem = new Item3(itemData); break;
            //case 4: newItem = new Item4(itemData); break;
            case 5: newItem = new Item5(itemData); break;
            case 6: newItem = new Item6(itemData); break;
            case 7: newItem = new Item7(itemData); break;
            case 8: newItem = new Item8(itemData); break;
            //case 9: newItem = new Item9(itemData); break;
            case 10: newItem = new Item10(itemData); break;
            case 11: newItem = new Item11(itemData); break; 
            case 12: newItem = new Item12(itemData); break;
            case 13: newItem = new Item13(itemData); break;
            case 14: newItem = new Item14(itemData); break;
            case 15: newItem = new Item15(itemData); break;
            case 16: newItem = new Item16(itemData); break;
            case 17: newItem = new Item17(itemData); break;
            case 18: newItem = new Item18(itemData); break;
            case 19: newItem = new Item19(itemData); break;
            case 20: newItem = new Item20(itemData); break;
            case 21: newItem = new Item21(itemData); break;
            case 22: newItem = new Item22(itemData); break;
            case 23: newItem = new Item23(itemData); break;
            case 24: newItem = new Item24(itemData); break;
            case 25: newItem = new Item25(itemData); break;
            case 26: newItem = new Item26(itemData); break;
            case 27: newItem = new Item27(itemData); break;
            case 28: newItem = new Item28(itemData); break;
            case 29: newItem = new Item29(itemData); break;
            case 30: newItem = new Item30(itemData); break;
            default: Debug.LogError("Effectindex " + itemData.EffectIndex +" does not exist"); break;
        }

        return newItem;
        
    }

    public static ItemInstance CreateItemDuplicate(ItemInstance itemInstance)
    {
        ItemInstance item = CreateItem(itemInstance.ItemData);
        item.Value = itemInstance.Value;
        item.Value2 = itemInstance.Value2;
        item.target = itemInstance.target;

        return item;
    }
}
