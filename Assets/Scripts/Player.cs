using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player i;

    static float score = 0;
    public static float Score { get { return score; } set { i.UpdateScoreText(value, score); score = value;  } }

    int targetScore = 20;
    public int TargetScore { get { return targetScore; } set { targetScore = value; i.UpdateTargetScoreText(value); } }

    [HideInInspector] public int RoundNumber;

    public TMP_Text scoreText;
    public TMP_Text targetScoreText;
    public Image scoreFill;

    public ItemData[] StartBag;

    public float DifficultyCurve = 1.65f;

    public GameObject EndGameHighlight;

    public static Action RoundEnded;

    AudioSource audio;

    public GameObject TutorialText;

    public int RoundGold = 7;

    public AudioClip CanWinSound;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        i = this;

        StartGame();
    }



    public void StartNewGame()
    {
        // add all items on table to bag and remove them from the table
        while (Table.i.itemObjects.Count > 0)
        {
            Table.RemoveItemFromTable(Table.i.itemObjects[0]);
        }
        Cauldron.instance.ItemAboutToGetPerformed = null;

        Shop.CurrentCoinAmount = 0;
        Bag.inventory.Clear();
        TargetScore = 0;

        

        StartGame();
        StartRound();
    }


    public void StartGame()
    {
        RoundNumber = 0;
        // fill bag
        foreach (var item in StartBag)
        {
            Bag.AddItem(Bag.CreateItem(item));
        }
    }

    public void StartRound()
    {
        EndGameHighlight.SetActive(false);
        Score = 0;
        RoundNumber++; // increment round number
        TargetScore = CalculateTargetScore(RoundNumber, TargetScore); // calculate the target score this round

    

        // draw 5 items
        for (int i = 0; i < 5; i++)
        {
            Table.TakeItemFromBag();
        }
    }

    void UpdateScoreText(float newValue,float old)
    {
        if (newValue > old)
            PotencyBar.PlayAnimation();

        // update text
        scoreText.text = newValue.ToString();
        scoreFill.fillAmount = newValue / TargetScore;

    }
    void UpdateTargetScoreText(float newValue)
    {
        // set target score text
        targetScoreText.text = TargetScore.ToString();
    }
    public void CheckWin()
    {
        if (Score >= TargetScore)
        {
            PlaySound(CanWinSound);
            EndGameHighlight.SetActive(true);
        }
    }

    public void TryCompleteRound()
    {
        if (GameManager.Instance.inShop)
            return;

        if (Score >= TargetScore)
        {
            CompleteRound();
        } else
        {
            GameManager.EndGame();
        }
    }

    void CompleteRound()
    {
        Debug.Log("Round Complete!");

        GameManager.Instance.inShop = true;
        // add all items on table to bag and remove them from the table
        while (Table.i.itemObjects.Count > 0)
        {
            Bag.AddItem(Table.i.itemObjects[0].item);
            Table.RemoveItemFromTable(Table.i.itemObjects[0]);
        }

        // reset the cauldron (modifiers)
        Cauldron.instance.ItemAboutToGetPerformed = null;

        RoundEnded?.Invoke();

        GameManager.Instance.EnterShop();
        Shop.CurrentCoinAmount += RoundGold;
    }

    int CalculateTargetScore(int round, int currentTarget)
    {
        return (int)Mathf.Pow(round, DifficultyCurve) + 10;
    }

    public static void PlaySound(AudioClip clip, float volume = 1)
    {
        i.audio.PlayOneShot(clip, volume);
    }

    public static void ItemRefused()
    {
        i.TutorialText.SetActive(true);
        if (i.tutorialRoutine != null)
        {
            i.StopCoroutine(Player.i.tutorialRoutine);
        }
        i.tutorialRoutine = i.StartCoroutine(i.TutorialTimer());
    }

    public Coroutine tutorialRoutine;

    IEnumerator TutorialTimer()
    {
        yield return new WaitForSeconds(4);
        TutorialText.SetActive(false);
    }
}
