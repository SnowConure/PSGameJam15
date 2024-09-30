using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemData : ScriptableObject
{
    public string Name;
    public string Description;
    public int EffectIndex;
    public Sprite Sprite;
    public float defaultValue;
    public float defaultValue2;
    public PlayPosition WhereToPlay;
    public int cost;
    public ItemRarity rarity;

}

public enum PlayPosition
{
    Cauldron,
    Item
}
public enum ItemRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}