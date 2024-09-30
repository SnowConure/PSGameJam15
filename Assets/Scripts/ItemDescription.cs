using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    public bool HoverOnUI;

    private void Start()
    {
        if (HoverOnUI) return;
        InputManager.OnNewHoverObject += UpdateDescription;
        Cauldron.instance.ItemPlayed += UpdateDescription;
    }
  

    public GameObject border;
    public TMP_Text descriptionText;
    public TMP_Text nameText;
    public Image image;

    public GameObject currentItemShown;

    public void UpdateDescription()
    {
        if (currentItemShown == null) return;
        UpdateDescriptiononInstance(currentItemShown.GetComponent<ItemObject>().item);
    }

    // played when the mouse hover over a new item
    public void UpdateDescription(GameObject gameObject)
    {

      //  GetComponent<RectTransform>().anchoredPosition = Input.mousePosition / GetComponentInParent<Canvas>().scaleFactor;
        
        if (gameObject == null || gameObject.tag != "Item")
        {
            border.SetActive(false);
            currentItemShown = null;
            return;
        }

        if (gameObject == currentItemShown) return;
        currentItemShown = gameObject;


        if(gameObject.tag == "Item")
            UpdateDescriptiononInstance(gameObject.GetComponent<ItemObject>().item);
        border.SetActive(true);

    }
    public void UpdateDescriptionUI(ItemData data)
    {
        UpdateDescriptiononInstance(data);
        
    }

    public void UpdateDescriptiononInstance(ItemInstance item)
    {
        Debug.Log(item.ItemData.Name + " 1");
        if (item == null) return;

        image.sprite = item.ItemData.Sprite;
        nameText.text = item.ItemData.Name;

        // format description

        float value1 = item.Value;
        float value2 = item.Value2;
        ItemInstance itemInstance = Cauldron.GetEstimatedValue(item);
        float modValue1 = itemInstance.Value - value1;
        float modValue2 = itemInstance.Value2 - value2;

        string value1String = value1 + (modValue1 == 0 ? "" : "+(" + modValue1 + ")");
        string value2String = value2 + (modValue2 == 0 ? "" : "+(" + modValue2 + ")");

        string description = item.ItemData.Description;
        description = description.Replace("{value}", value1String)
                       .Replace("{value1}", value1String)
                       .Replace("{value2}", value2String)
                       .Replace("(s1)", (modValue1 + value1 > 1 ? "s" : ""))
                        .Replace("(s2)", (modValue2 + value2 > 1 ? "s" : ""));

        descriptionText.text = description;

    }
    public void UpdateDescriptiononInstance(ItemData item)
    {
        if (item == null) return;

        image.sprite = item.Sprite;
        nameText.text = item.Name;

        // format description

        float value1 = item.defaultValue;
        float value2 = item.defaultValue2;

        string description = item.Description;

        description = description.Replace("{value}", value1.ToString())
                       .Replace("{value1}", value1.ToString())
                       .Replace("{value2}", value2.ToString());

        descriptionText.text = description;

    }

}
