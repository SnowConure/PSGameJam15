using System;
using UnityEngine;

public class AddToBagEffect : MonoBehaviour
{
    public Vector2 startPos;

    public Action dead;

    public float speed;
    public float lifeTime;
    RectTransform rt;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = startPos;
    }

    private void Update()
    {
        rt.anchoredPosition -= (Vector2.up * Time.deltaTime * speed);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            dead?.Invoke();
            Destroy(gameObject);

        }
    }
}
