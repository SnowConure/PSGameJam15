using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler
{
    public Image image;
    public TMP_Text costText;

    public ItemData itemData;
    public ItemDescription description;

    public void Setup(ItemData data)
    {
        itemData = data;
        image.sprite = data.Sprite;
        gameObject.SetActive(true);
        costText.text = data.cost.ToString();
    }

    public void OnClick()
    {
        Shop.instance.BuyItem(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayInDescription();
    }
    public void DisplayInDescription()
    {
        description.UpdateDescriptionUI(itemData);
    }
}
