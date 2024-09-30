using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// todo: dont recreate the bag everything you click it. you change the difference.
/// </summary>
public class BagVisuals : MonoBehaviour
{
    public GameObject bagRowPrefab;
    public Transform bagRowsContentObject;
    public GameObject AddEffect;

    public GameObject BagMenu;

    bool isOpen = false;

    Dictionary<string, itemGroup> itemGroups = new Dictionary<string, itemGroup>();

    List<BagRow> bagRows = new List<BagRow>();

    public bool startOpen = false;

    public GameObject openBag, closedBag, openBag2;


    public AudioClip openSound, CloseSound;

    private void Awake()
    {
        if(startOpen)
        BagMenu.SetActive(true);
        else
        BagMenu.SetActive(false);

       

    }

    private void Start()
    {
        Bag.ItemAdded += PlayAddEffect;
    }

    public void ToggleBag()
    {
        isOpen = !isOpen;
        if (isOpen)
            OpenBag();
        else CloseBag();
    }

    void OpenBag()
    {
        Player.PlaySound(openSound);
        UpdateBag();
        BagMenu.SetActive(true);
        openVisual(true);
    }

    public void UpdateBag()
    {
        itemGroups.Clear();

        // populate the itemsgroup dictionary will all info
        foreach (ItemInstance item in Bag.inventory)
        {
            // check of there are any groups for this item? else create one
            if (!itemGroups.ContainsKey(item.ItemData.name))
            {
                itemGroups.Add(item.ItemData.name, new itemGroup());
            }
            // add item to group
            itemGroups[item.ItemData.name].Add(item);

        }
        // save all the keys in an array, so we can loop thorugh them all later.
        string[] groupKeys = itemGroups.Keys.ToArray();

        // add so there are for sure more bagrows than itemgroups
        for (int i = 0; i < groupKeys.Length; i++)
            if (bagRows.Count < itemGroups.Count)
                bagRows.Add(Instantiate(bagRowPrefab, bagRowsContentObject).GetComponent<BagRow>());

        // if there are to many bagrows dont delete, but disable.
        for (int i = 0; i < bagRows.Count; i++)
        {
            if (i >= groupKeys.Length) // disable not used bagRows
            {
                bagRows[i].gameObject.SetActive(false);
            }
            else // here we know this bagrow is gonna be used, so set it up
            {
                bagRows[i].Setup(itemGroups[groupKeys[i]]);
            }
        }

    }

    void CloseBag()
    {
        Player.PlaySound(CloseSound);

        openVisual(false);
        BagMenu.SetActive(false);
    }


    int EffectsInPlay = 0;
    void PlayAddEffect(ItemInstance item)
    {
        GameObject ef = Instantiate(AddEffect, openBag.transform);
        ef.GetComponent<Image>().sprite = item.ItemData.Sprite;
        ef.GetComponent<AddToBagEffect>().dead += tryCloseVisual;
        EffectsInPlay++;
        openVisual(true);

    }

    void tryCloseVisual()
    {
        EffectsInPlay--;
        if (EffectsInPlay <= 0)
        {
            openVisual(false);
        }
            
    }

    void openVisual(bool open)
    {
        if (open)
        {
            closedBag.SetActive(false);
            openBag.SetActive(true);
            openBag2.SetActive(true);
        } else
        {
            closedBag.SetActive(true);
            openBag.SetActive(false);
            openBag2.SetActive(false);
        }
    }
}

public class itemGroup
{
    public List<ItemInstance> items = new List<ItemInstance>();
    public void Add(ItemInstance item) => items.Add(item);
    public void Remove(ItemInstance item) => items.Remove(item);
    public bool Contains(ItemInstance item) => items.Contains(item);
}