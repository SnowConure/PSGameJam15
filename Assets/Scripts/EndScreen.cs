using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Device;

public class EndScreen : MonoBehaviour
{
    public static EndScreen i;
    private void Awake()
    {
        i = this;
        anim = GetComponent<Animator>();
    }

    public TMP_Text dayText;
    public Animator anim;
    public GameObject screen;

    public static void Perform(int day)
    {
        i.dayText.text = "You Reached day: " + day;
        i.screen.SetActive(true);
        i.anim.Play("End");
    }
    public static void RemoveScreen()
    {
        i.screen.SetActive(false);
    }
}
