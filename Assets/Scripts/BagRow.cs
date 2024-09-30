using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagRow : MonoBehaviour
{
    [Header("prefabs")]
    public GameObject BagItemPrefab;

    [Header("sizes")]
    public float itemCellSize = 50;


    [Header("requirements")]

    RectTransform rt;
    public RectTransform itemCollection;
    public RectTransform button;
    public RectTransform arrowUp;
    public RectTransform arrowDown;

    public Image image;
    public TMP_Text itemName;
    public TMP_Text itemAmount;

    List<BagItem> items = new List<BagItem>();

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Setup(itemGroup itemGroup)
    {
        image.sprite = itemGroup.items[0].ItemData.Sprite;
        itemName.text = itemGroup.items[0].ItemData.Name;
        itemAmount.text = itemGroup.items.Count.ToString();


        // add so there are for sure more bagitems than itemgroups
        for (int i = 0; i < itemGroup.items.Count; i++)
            if (items.Count < itemGroup.items.Count)
                items.Add(Instantiate(BagItemPrefab, itemCollection).GetComponent<BagItem>());

        // if there are to many bagrows dont delete, but disable.
        for (int i = 0; i < items.Count; i++)
        {
            if (i >= itemGroup.items.Count) // disable not used bagRows
            {
                items[i].gameObject.SetActive(false);
            }
            else // here we know this bagrow is gonna be used, so set it up
            {
                items[i].Setup(itemGroup.items[i]);
            }
        }

    }

    bool isExpanded = false;
    public void Clicked()
    {
        isExpanded = !isExpanded;
        if(isExpanded)
            Expand();
        else
            Shrink();


    }

    void Expand()
    {
        itemCollection.gameObject.SetActive(true);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, button.sizeDelta.y + itemCollection.sizeDelta.y);
        arrowUp.gameObject.SetActive(!isExpanded);
        arrowDown.gameObject.SetActive(isExpanded);



    }
    void Shrink()
    {
        itemCollection.gameObject.SetActive(false);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, button.sizeDelta.y);
        arrowUp.gameObject.SetActive(isExpanded);
        arrowDown.gameObject.SetActive(!isExpanded);

    }
}
