using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemInstance
{
    public ItemInstance(ItemData itemData)
    { 
        ItemData = itemData;
        Value = ItemData.defaultValue;
        Value2 = ItemData.defaultValue2;
    }
    /// <summary>
    /// Dont use this to create items. It will create an empty "dummy" item.
    /// </summary>
    /// <param name="value">dummy value</param>
    public ItemInstance(float value)
    {
        Value = value;
        isDummy = true;
    }

    ~ItemInstance()
    {
        Debug.Log("deleted an item instace! " + ItemData.name);
    }

    public ItemData ItemData;
    public float Value = 0;
    public float Value2 = 0;

    // not always used, its the item this item is played on.
    public ItemObject target;

    public bool isDummy = false;

    public virtual void PerformEffect(){}
}
// Add score
public class Item1 : ItemInstance
{
    public Item1(ItemData itemData) : base(itemData){}
    public override void PerformEffect()
    {
        Player.Score += Value;
    }

}
// Multiply
public class Item2 : ItemInstance
{
    public Item2(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score *= Value;
    }

}
// Next item played has value x2 when played.
public class Item3 : ItemInstance
{
    bool ignoreFirstUnsubscribe = false;
    public Item3(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Cauldron.instance.ItemAboutToGetPerformed += Multiply;
        Cauldron.instance.ItemPlayed += Unsubscribe;
    }
    void Multiply(ItemInstance item)
    {
        item.Value *= Value;
        item.Value2 *= Value;
    }
    void Unsubscribe()
    {
        if (ignoreFirstUnsubscribe == false)
        {
            ignoreFirstUnsubscribe = true;
            return;
        }
        Cauldron.instance.ItemAboutToGetPerformed -= Multiply;
        Cauldron.instance.ItemPlayed -= Unsubscribe;

    }
}
// -1 score. gain a random item
public class Item5 : ItemInstance
{
    public Item5(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score += Value;

        // add item
        for (int i = 0; i < Value2; i++)
        {
            ItemData[] items = GameManager.GetAllAvailableItems();
            Table.AddItemToTable(Bag.CreateItem(items[Random.Range(0, items.Count())]));
        }
   }
}

// copy an item to your bag.
public class Item6 : ItemInstance
{
    public Item6(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        for (int i = 0; i < Value; i++)
        {
            Bag.AddItem(Bag.CreateItemDuplicate(target.item));

        }
    }

}
// the next item played will be played twice.
public class Item7 : ItemInstance
{
    bool ignoreFirstUnsubscribe = false;
    public Item7(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Cauldron.instance.ItemAboutToGetPerformed += PlayItemEffectAgain;
        Cauldron.instance.ItemPlayed += Unsubscribe;
    }
    void PlayItemEffectAgain(ItemInstance item)
    {
        if (item.isDummy) return;

        for (int i = 0; i < Value; i++) 
        {
            ItemInstance dupeItem = Bag.CreateItemDuplicate(item);
            dupeItem.PerformEffect();

        }

        //ItemInstance dupeItem = Bag.CreateItemDuplicate(item);
        //Cauldron.instance.PerformEffect(dupeItem);
    }
    void Unsubscribe()
    {
        if (ignoreFirstUnsubscribe == false)
        {
            ignoreFirstUnsubscribe = true;
            return;
        }
        Cauldron.instance.ItemAboutToGetPerformed -= PlayItemEffectAgain;
        Cauldron.instance.ItemPlayed -= Unsubscribe;
    }

}

// /2 score then add 5
public class Item8 : ItemInstance
{
    public Item8(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        if(Player.Score != 0)
            Player.Score = Player.Score / Value;
        Player.Score += Value2;

    }

}

// give item on shelf +1 value
public class Item10 : ItemInstance
{
    public Item10(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {

        target.item.Value += Value;
        target.item.Value2 += Value;

    }

}

// add potency equal to current gold
public class Item11 : ItemInstance
{
    public Item11(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score += Shop.CurrentCoinAmount;

    }

}

//If next item played is of the same type as the last, x3 its value
public class Item12 : ItemInstance
{
    int itemsPlayed = 0;
    string itemName = "";
    public Item12(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Cauldron.instance.ItemAboutToGetPerformed += ItemWasPlayed;
        Cauldron.instance.ItemPlayed += Unsubscribe;
    }
    void ItemWasPlayed(ItemInstance item)
    {
        if(itemsPlayed == 1)
            itemName = item.ItemData.Name;
        if (itemsPlayed == 2 && itemName == item.ItemData.Name)
        {
            item.Value *= Value;
            item.Value2 *= Value;
        }
    }
    void Unsubscribe()
    {
        itemsPlayed++;

        if (itemsPlayed == 3) // third time it triggers. time to unsub
        {
            Cauldron.instance.ItemAboutToGetPerformed -= ItemWasPlayed;
            Cauldron.instance.ItemPlayed -= Unsubscribe;
        }
    }
}
// give an item on shelf +3 value, chance to destroy item instead.
public class Item13 : ItemInstance
{
    public Item13(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        target.item.Value += Value;
        target.item.Value2 += Value;

        if (Random.Range(0, 10) < 5)
            Table.RemoveItemFromTable(target);

    }
}
// gain 1 gold
public class Item14 : ItemInstance
{
    public Item14(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Shop.CurrentCoinAmount += (int)Value;

    }
}
// for rest of round add 1 to Potency anytime a item is played
public class Item15 : ItemInstance
{
    public Item15(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        // dont need to unsub, we reset cauldron evey round anyways.
        Cauldron.instance.ItemPlayed += AddPotency;
        Player.RoundEnded += Unsubscribe;
    }
    void AddPotency()
    {
        Player.Score += Value;
    }
    void Unsubscribe()
    {
        Cauldron.instance.ItemPlayed -= AddPotency;
        Player.RoundEnded -= Unsubscribe;
    }
}

// -10 potency but double the value of the next two items played
public class Item16 : ItemInstance
{
    int itemsPlayed = 0;
    public Item16(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Cauldron.instance.ItemAboutToGetPerformed += ItemWasPlayed;
        Cauldron.instance.ItemPlayed += Unsubscribe;

        Player.Score -= Value;
    }
    void ItemWasPlayed(ItemInstance item)
    {
        item.Value *= Value2;
        item.Value2 *= Value2;
    }
    void Unsubscribe()
    {
        itemsPlayed++;

        if (itemsPlayed == 3) // third time it triggers. time to unsub
        {
            Cauldron.instance.ItemAboutToGetPerformed -= ItemWasPlayed;
            Cauldron.instance.ItemPlayed -= Unsubscribe;
        }
    }
}
// +1 potency for every item currently in your bag:
public class Item17 : ItemInstance
{
    public Item17(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score += Bag.inventory.Count;
    }
}
// -10 or +10 to potency
public class Item18 : ItemInstance
{
    public Item18(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score += Random.Range(0, 2) == 0 ? -Value : Value;
    }
}
// Discard an item and add its Value to potency
public class Item19 : ItemInstance
{
    public Item19(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score += target.item.Value;
        Table.RemoveItemFromTable(target);
    }
}
// place an item back into your bag and draw 2
public class Item20 : ItemInstance
{
    public Item20(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Bag.AddItem(target.item);
        for (int i = 0; i < Value; i++)
        {
            Table.TakeItemFromBag();
        }
        Table.RemoveItemFromTable(target);

    }
}

// Draw a random item and instantly play it with x2 value
public class Item21 : ItemInstance
{
    public Item21(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        ItemInstance item = Bag.DrawRandomItem();

        if (item == null) // no item in bag, return
            return;
        
        item.Value *= Value;
        item.Value2 *= Value;

        if (item.ItemData.WhereToPlay == PlayPosition.Item) // if its supposed to be played on an item. get a random itom on table
        {
            item.target = Table.i.itemObjects[Random.Range(0,Table.i.itemObjects.Count)];
        }

        Cauldron.instance.PerformEffect(item);
    }
}
// Gain 3 gold if current potency is a multiple of 3
public class Item22 : ItemInstance
{
    public Item22(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        if(Player.Score % Value2 == 0)
        {
            Shop.CurrentCoinAmount += (int)Value;
        }
    }
}
// give an item a random value between -5 and 5
public class Item23 : ItemInstance
{
    public Item23(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {

        int num = Random.Range((int)Mathf.Floor(Value), (int)Mathf.Floor(Value2) +1);
        target.item.Value += num;
        target.item.Value2 += num;
    }
}
// --
public class Item24 : ItemInstance
{
    public Item24(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score += Bag.inventory.Count;
    }
}
// +1 Potency for each unique item in bag
public class Item25 : ItemInstance
{
    List<string> items = new List<string>();
    public Item25(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        foreach(var item in Bag.inventory)
        {
            if(!items.Contains(item.ItemData.name))
                items.Add(item.ItemData.name);
        }
        Player.Score += items.Count*Value;
    }
}
// Add current hour to potency
public class Item26 : ItemInstance
{
    public Item26(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score += System.DateTime.Now.Hour * Value;
    }
}
// Remove An Item Give its value to next item played.
public class Item27 : ItemInstance
{
    bool ignoreFirstUnsubscribe = false;
    float mod = 0;
    public Item27(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Cauldron.instance.ItemAboutToGetPerformed += Multiply;
        Cauldron.instance.ItemPlayed += Unsubscribe;

        mod = target.item.Value;
        Table.RemoveItemFromTable(target);
    }
    void Multiply(ItemInstance item)
    {
        item.Value += mod;
        item.Value2 += mod;
    }
    void Unsubscribe()
    {
        if (ignoreFirstUnsubscribe == false)
        {
            ignoreFirstUnsubscribe = true;
            return;
        }
        Cauldron.instance.ItemAboutToGetPerformed -= Multiply;
        Cauldron.instance.ItemPlayed -= Unsubscribe;

    }
}
// Convert all your gold into potency 1:1 ratio
public class Item28 : ItemInstance
{
    public Item28(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score += Shop.CurrentCoinAmount * Value;
        Shop.CurrentCoinAmount = 0;
    }
}
// Reduce target potency with 20% and ser you Potency to 0
public class Item29 : ItemInstance
{
    public Item29(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.i.TargetScore = (int)(Player.i.TargetScore * (1 - Value / 100));
        Player.Score = Value2;
    }
}
// Set Potency to the inverse
public class Item30 : ItemInstance
{
    public Item30(ItemData itemData) : base(itemData) { }
    public override void PerformEffect()
    {
        Player.Score = -Player.Score;
    }
}
