using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is / is gonna be fuicking havoc
/// good luck...
/// </summary>

public class ItemAnimator : MonoBehaviour
{
    SpriteRenderer sr;
    ItemObject item;

    public Material NormalMaterial;
    public Material PixelMaterial;



    void Awake()
    {
        item = GetComponent<ItemObject>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        // hovering an item will play an animation
        HoveringUpdate();
    }


    bool HoveringSelect;
    float hovering_ShrinkDir;
    float hovering_Progress = 0;
    public float hovering_Speed = 1;
    public AnimationCurve hovering_Curve;
    bool isChangeingSize;

    void HoveringUpdate()
    {
        if (item.wantToPerformOnItem != HoveringSelect)
        {
            // new state for hovering select
            HoveringSelect = item.wantToPerformOnItem;

            hovering_ShrinkDir = HoveringSelect ? 1 : -1;
            hovering_Progress = HoveringSelect ? 0 : 1;
            isChangeingSize = true;
            sr.material = PixelMaterial;
            transform.position = transform.position - Vector3.forward * 0.8f;
        }

        if (isChangeingSize)
        {
            // timer
            hovering_Progress += Time.deltaTime * hovering_ShrinkDir * hovering_Speed;

            // set shader parameter
            sr.material.SetFloat("_Size", Mathf.Lerp(1, 10, hovering_Curve.Evaluate(hovering_Progress)));

            // check for if we're done changeing size
            if (hovering_Progress > 1 || hovering_Progress < 0)
            {
                isChangeingSize = false;
                if(hovering_Progress < 0)
                {
                    sr.material = NormalMaterial;
                }
            }
        }
       
    }

}
