using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    public GameObject shopObject;

    public ShopItem[] shopItems;

    static int currentCoinAmount = 0;
    public static int CurrentCoinAmount { get { return currentCoinAmount; } set { currentCoinAmount = value; instance.UpdateCoinText(value); } }
    public TMP_Text coinText;
    public TMP_Text coinTextNormal;

    public AudioClip boughtItemSound;
    public Animator anim;
    private void Awake()
    {
        instance = this;
        if(shopObject.activeSelf) shopObject.SetActive(false);
    }

    void UpdateCoinText(int value) { coinText.text = value.ToString(); coinTextNormal.text = value.ToString(); }

    public static void EnterShop()
    {
        instance.shopObject.SetActive(true);
        instance.SetupItems();
        instance.shopItems[0].DisplayInDescription();
        
        //instance.anim.Play("Enter");
    }

    public static void ExitShop()
    {
        if (!GameManager.Instance.inShop) return;
        GameManager.Instance.inShop = false;

        Transition.i.StartTransition(() => {
            GameManager.Instance.ExitShop();
            instance.shopObject.SetActive(false);
            });

        
    }

    void SetupItems()
    {

        foreach(ShopItem item in shopItems)
        {
            item.Setup(GameManager.GetRandomItem());
        }
    }

    public void BuyItem(ShopItem shopItem)
    {
        if (CurrentCoinAmount < shopItem.itemData.cost)
            return;

        CurrentCoinAmount -= shopItem.itemData.cost;

        Bag.AddItem(Bag.CreateItem(shopItem.itemData));

        shopItem.gameObject.SetActive(false);

        Player.PlaySound(boughtItemSound);
    }
}
