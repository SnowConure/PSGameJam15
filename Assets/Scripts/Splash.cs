using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public static Splash instance;

    float y;

    Animator anim;
    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        y = transform.position.y;
    }

    public static void Play(float slide)
    {
        instance.transform.position = new Vector2 (Mathf.Clamp(slide, 5.248f, 7.095f), instance.y);
        instance.anim.Play("Splash");
    }
}
