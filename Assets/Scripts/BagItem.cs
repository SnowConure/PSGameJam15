using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    public Image image;
    public TMP_Text valueText;
    public TMP_Text valueText2;

    public ItemInstance instance;

    public void Setup(ItemInstance item)
    {
        instance = item;
        image.sprite = item.ItemData.Sprite;
        valueText.text = item.Value.ToString();
        valueText2.text = item.Value2 == 0 ? "" : item.Value2.ToString();
    }
}
