using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] ItemData[] AllItems;
    ItemData[] UnlockedItems;
    public static ItemData[] GetAllAvailableItems() => Instance.AllItems;
    public static ItemData GetRandomItem() => Instance.AllItems[Random.Range(0, Instance.AllItems.Length)];

    public AudioClip WinSound;
    public AudioClip LostSound;

    public bool inShop;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartRound();
    }

    public void StartNewGame()
    {
        EndScreen.RemoveScreen();
        Player.i.StartNewGame();
    }

    public static void EndGame()
    {
        EndScreen.Perform(Player.i.RoundNumber);
        Player.PlaySound(Instance.LostSound);
    }

    void StartRound()
    {
        Player.i.StartRound();
    }

    public void EnterShop()
    {
        Transition.i.StartTransition(() => { Shop.EnterShop(); });
        Player.PlaySound(Instance.WinSound);

    }
    public void ExitShop()
    {
        inShop = false;
        StartRound();
    }
}
