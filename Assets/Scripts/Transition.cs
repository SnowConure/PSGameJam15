using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public static Transition i;
    Material mat;

    public float duration = 6;  // Duration of the lerp
    public float speed = 2.0f;  // Duration of the lerp

    public AudioClip clip;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        i = this;
        mat = GetComponent<SpriteRenderer>().material;
    }

    Action Callback;
    public void StartTransition(Action callback)
    {
        Player.PlaySound(clip);
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(LerpCoroutine(1));
        Callback = callback;
    }

    private IEnumerator LerpCoroutine(int direction)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime * duration * speed;
            
            mat.SetFloat("_Size", time);

            yield return null;
        }
        Callback?.Invoke();
        Callback = null;

        while (time > 0)
        {
            time -= Time.deltaTime * duration * speed;
            Debug.Log("huh?" + time);
            mat.SetFloat("_Size", time < 0 ? 0 : time);

            yield return null;
        }
        mat.SetFloat("_Size", 0);
    }
}
