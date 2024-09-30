using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour, IClickable
{
    public Action PickedUp;
    public Action Released;

    bool isFollowingMouse;

    Vector2 followOffset = Vector2.down *.4f;
    Vector2 previousMousePosition;
    Vector3 velocity;
    public float rotationSpeed = 5f; // Rotation speed factor
    public float maxRotationAngle = 45f; // Maximum rotation angle
    public float MouseForce = 1; // Maximum rotation angle
    public float gravity = 1; // Maximum rotation angle

    public Vector2 wallConstrains;

    public float groundHeight = -1;
    float tableHeight = -1;

    public AudioClip landSound;
    public AudioClip PickupSound;
    private void Update()
    {


        if (isFollowingMouse)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 newPosition = mousePosition + followOffset;

            // Calculate x velocity of the mouse
            float xVelocity = (mousePosition.x - previousMousePosition.x) / Time.deltaTime * MouseForce;

            // Map x velocity to rotation angle within -maxRotationAngle to maxRotationAngle
            float targetAngle = Mathf.Clamp(xVelocity, -maxRotationAngle, maxRotationAngle);
            targetAngle *= -1;

            // Smoothly rotate towards the target angle
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Update the position
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            previousMousePosition = mousePosition;
        } else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        }

        if(!isFollowingMouse)
            Gravity();
        Constraints();
    }

    void Gravity()
    {
        velocity += Vector3.down * gravity * Time.deltaTime;
        transform.position += new Vector3(velocity.x, velocity.y, 0);
    }

    bool inAir;

    void Constraints()
    {
        Vector2 pos = transform.position;

        if (pos.y < groundHeight) // \/
        {
            pos.y = groundHeight;
            if (inAir)
            {
                inAir = false;
                Player.PlaySound(landSound);
            }
        } else
        {
            inAir = true;
        }

        if (pos.x < wallConstrains.x) // <
            pos.x = wallConstrains.x;
        if (pos.x > wallConstrains.y) // >
            pos.x = wallConstrains.y;

        transform.position = pos;
    }

    public void Click()
    {
        isFollowingMouse = true;

        //followOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Player.PlaySound(PickupSound);

        PickedUp?.Invoke();
        groundHeight = tableHeight;
    }

    public void Release()
    {
        velocity = Vector3.zero;
        isFollowingMouse = false;
        Released?.Invoke();

        groundHeight = Physics2D.Raycast(transform.position, Vector2.down, 20, 1 << 8).point.y + (transform.localScale.y *.5f);
    }

}
