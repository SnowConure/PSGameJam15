using System;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public static Cauldron instance;
    private void Awake()
    {
        instance = this;
    }

    static ItemInstance dummyItem = new ItemInstance(1);


    public Action<ItemInstance> ItemAboutToGetPerformed;
    public Action ItemPlayed; // remember this is played on each item played. so if you subscribe to ItemAboutToGetPerformed the same frame ItemPlayed will be played

    public AudioClip ItemDroppedInCauldronSound;
    public AudioClip ItemPlayedOnAnother;

    public void PerformEffect(ItemInstance item)
    {
        ItemAboutToGetPerformed?.Invoke(item);

        item.PerformEffect();

        ItemPlayed?.Invoke();

        if(item.ItemData.WhereToPlay == PlayPosition.Item)
            Player.PlaySound(ItemPlayedOnAnother);
        else
            Player.PlaySound(ItemDroppedInCauldronSound);


        // very bad code, but its for tutorial. its last hour on jam so idgaf
        Player.i.TutorialText.SetActive(false);
        if(Player.i.tutorialRoutine != null)
            Player.i.StopCoroutine(Player.i.tutorialRoutine);

    }

    // given a value, it will try to estimate the final value after the item is played
    // I use this for the item description.
    public static ItemInstance GetEstimatedValue(ItemInstance itemInstance)
    {
        dummyItem.Value = itemInstance.Value;
        dummyItem.Value2 = itemInstance.Value2;
        dummyItem.ItemData = itemInstance.ItemData;
        dummyItem.isDummy = true;
        instance.ItemAboutToGetPerformed?.Invoke(dummyItem);
        return dummyItem;
    }
}
