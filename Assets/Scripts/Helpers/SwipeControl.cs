using UnityEngine;
using System.Collections;

public class SwipeControl : MonoBehaviour
{

    public event SwipeAction onSwipe;

    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;

    void Update()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            isSwipe = true;
                            fingerStartTime = Time.time;
                            fingerStartPos = touch.position;
                            break;

                        case TouchPhase.Canceled:
                            isSwipe = false;
                            break;

                        case TouchPhase.Ended:
                            AnalyzeVector(touch.position);
                            break;
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSwipe = true;
                fingerStartTime = Time.time;
                fingerStartPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                AnalyzeVector(Input.mousePosition);
            }
        }
    }

    void AnalyzeVector(Vector2 pos)
    {
        float gestureTime = Time.time - fingerStartTime;
        float gestureDist = (pos - fingerStartPos).magnitude;

        if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
        {
            Vector2 direction = pos - fingerStartPos;
            Vector2 swipeType = Vector2.zero;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // the swipe is horizontal:
                swipeType = Vector2.right * Mathf.Sign(direction.x);
            }
            else
            {
                // the swipe is vertical:
                swipeType = Vector2.up * Mathf.Sign(direction.y);
            }

            if (swipeType.x != 0.0f)
            {
                if (swipeType.x > 0.0f)
                    onSwipe(Direction.Right);
                else
                    onSwipe(Direction.Up);
            }

            if (swipeType.y != 0.0f)
            {
                if (swipeType.y > 0.0f)
                    onSwipe(Direction.Left);
                else
                    onSwipe(Direction.Down);
            }
        }
    }
}

      