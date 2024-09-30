using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ClickAndDrag))]
public class ItemObject : MonoBehaviour
{
    public Action<ItemObject> ItemUsed;

    ClickAndDrag cad;
    Vector3 ShelfPosition;
    public ItemInstance item;
    SpriteRenderer sprite;

    bool isHeld;
   [HideInInspector] public bool wantToPerformOnItem;

    // how many seconds it takes to use an item on another item.
    public float TimeHoveringUntillSelected = 1;

    public Material mat;

  


    public BoxCollider2D col;

    public AudioClip RefusedSound;


    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        mat = GetComponent<SpriteRenderer>().material;
        sprite = GetComponent<SpriteRenderer>();

        cad = GetComponent<ClickAndDrag>();
        cad.PickedUp += MouseClicked;
        cad.Released += MouseReleased;

        ShelfPosition = transform.position;
    }

    private void OnDisable()
    {
        cad.PickedUp -= MouseClicked;
        cad.Released -= MouseReleased;
    }

    public void Setup(ItemInstance item)
    {
        sprite.sprite = item.ItemData.Sprite;
        this.item = item;
    }

    void MouseReleased()
    {
        isHeld = false;
        if (wantToPerformOnItem)
        {
            // we do PerformEffect from hovering over and item
            item.target = closestTouchingItem.GetComponent<ItemObject>();
            PerformEffect();
        }

        col.size = Vector2.one;
        col.offset = Vector2.zero;
        

    }

    void MouseClicked()
    {
        isHeld = true;
        Table.BringToFront(this);

        // set collider size, so that its the mouse position, helps with precision
        col.size = Vector2.one * 0.5f;
        col.offset = Vector2.up * .5f;
    }

    void PerformEffect()
    {
        // let the cauldron perform the effect, so it can add modifiers and so on.
        Cauldron.instance.PerformEffect(item);

        if(item.ItemData.WhereToPlay == PlayPosition.Cauldron)
            Splash.Play(transform.position.x);


        ItemUsed?.Invoke(this);
    }

    void DroppedInCauldron()
    {
        // we dont want items not played in cauldron to be able to fall into the cauldron
        if (item.ItemData.WhereToPlay != PlayPosition.Cauldron)
        {
            transform.position = Vector3.zero;
            cad.groundHeight = -1;
            Player.PlaySound(RefusedSound);
            Player.ItemRefused();
            return;
        }

        // we do PerformEffect from touching thew cauldron
        PerformEffect();
    }

    List<GameObject> touchingItems = new List<GameObject>();
    GameObject closestTouchingItem;
    float hoveringItemTime;


    private void Update()
    {
        hoveringItemUpdate();
    }
    #region Hovering item / cauldron


    private void hoveringItemUpdate()
    {
        if (touchingItems.Count > 0 && isHeld)
        {
            if (item.ItemData.WhereToPlay != PlayPosition.Item) return;

            // Get closest touching object
            float closestDistance = 99999;
            GameObject newClosestTouchingItem = null;
            foreach (GameObject item in touchingItems)
            {
                float dist = Vector2.Distance(transform.position, item.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    newClosestTouchingItem = item;
                }
            }

            // if there is a new closest item
            if (newClosestTouchingItem != closestTouchingItem)
            {
                closestTouchingItem = newClosestTouchingItem;
                hoveringItemTime = 0;
            }

            hoveringItemTime += Time.deltaTime;

            if (hoveringItemTime > TimeHoveringUntillSelected)
            {
                wantToPerformOnItem = true;
            }
        }
        else
        {
            hoveringItemTime = 0;
            wantToPerformOnItem = false;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            DroppedInCauldron();
        }

        if (collision.tag == "Item")
        {
            touchingItems.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            touchingItems.Remove(collision.gameObject);
        }
    }
    #endregion
}
