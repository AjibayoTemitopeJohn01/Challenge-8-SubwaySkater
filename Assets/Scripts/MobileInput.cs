using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    private const float DeadZone = 100.0f;
    public static MobileInput Instance
    {
        set;
        get;
    }
    
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 swipeDelta, startTouch;

    public bool Tap
    {
        get
        {
            return tap;
        }
    }

    public bool SwipeLeft
    {
        get
        {
            return swipeLeft;
        }
    }
    
    public bool SwipeRight
    {
        get
        {
            return swipeRight;
        }
    }
    
    public bool SwipeUp
    {
        get
        {
            return swipeUp;
        }
    }
    
    public bool SwipeDown
    {
        get
        {
            return swipeDown;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Resetting all the booleans
        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;
        
        // Getting Player Inputs
        
        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }
        #endregion
        
        #region Mobile Inputs
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
            }
        }
        #endregion
        
        // Calculate Distance
        swipeDelta = Vector2.zero;
        if (startTouch != Vector2.zero)
        {
            // Check with mobile
            if (Input.touches.Length != 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            // Check with PC
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }
        
        // Check if we're beyond the deadzone
        if (swipeDelta.magnitude > DeadZone)
        {
            // This is a confirmed swipe
            var x = swipeDelta.x;
            var y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // left or Right
                if (x < 0)
                {
                    swipeLeft = true; // Move Left
                    Debug.Log("Move Left");
                }
                else
                {
                    swipeRight = true; // Move Right
                    Debug.Log("Move Right");
                }
                    
            }
            else
            {
                // Up or Down
                if (y < 0)
                {
                    swipeDown = true; // Jump Down
       //             Debug.Log(" Fall Down");
                }
                else
                {
                    swipeUp = true; // Jump Up
       //             Debug.Log("Jump Up");
                }
                    
            }

            startTouch = swipeDelta = Vector2.zero;
        }
    }
}