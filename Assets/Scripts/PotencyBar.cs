using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotencyBar : MonoBehaviour
{
    public static PotencyBar Instance;

    Animator anim;

    public void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    public static void PlayAnimation()
    {
        Instance.anim.Play(Random.Range(0,2) == 0 ? "1" : "2");
    }

   
}
