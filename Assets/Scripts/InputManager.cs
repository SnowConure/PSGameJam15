using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    public static Action<GameObject> OnNewHoverObject;
    public static GameObject HoveringObject;

    Action ReleaseMouseButton;

    bool singleClickAsHold;

    bool MouseButtonDown;

    float mouseButtonHeldTime;

    void Update()
    {
        InputUpdate();
        HoldTimer();
        UpdateHovering();


    }

    void UpdateHovering()
    {
        GameObject gameObject = GetObjectAtMousePosition();

        if (gameObject == HoveringObject) return; // if is same no new object, so ignore

        HoveringObject = gameObject;
        OnNewHoverObject?.Invoke(HoveringObject);
    }
    // Counts how long the mouse button been pressed
    void HoldTimer()
    {
        if (!MouseButtonDown) return;
        mouseButtonHeldTime += Time.deltaTime;
    }

    void InputUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseButtonHeldTime = 0;
            MouseButtonDown = true;
            Click();
        }
        if (Input.GetMouseButtonUp(0))
        {
            MouseButtonDown = false;
            Release();
        }
    }

    public static Vector2 MousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static GameObject GetObjectAtMousePosition()
    {
        return Physics2D.OverlapPoint(MousePosition())?.gameObject;
    }

    public static GameObject[] GetObjectsAtMousePosition()
    {
        return Physics2D.OverlapPointAll(MousePosition()).Select(col => col.gameObject).ToArray();
    }

    void Click()
    {
        // check for collision
        IClickable clickedObject = GetObjectAtMousePosition()?.GetComponent<IClickable>();
        if (clickedObject != null)
        {
            // Click object
            clickedObject.Click();
            ReleaseMouseButton += clickedObject.Release;
        }
    }
    void Release()
    {
        // Send a release call to clicked object
        ReleaseMouseButton?.Invoke();
        // reset the action to null
        ReleaseMouseButton = null;
    }
}
