using UnityEngine;
using UnityEngine.Events;

public class SwipeDetector : MonoBehaviour
{
    // Minimum distance for a swipe to be registered (in pixels)
    public float minSwipeDistance = 50f;

    // Maximum time for a tap to be registered (in seconds)
    public float maxTapTime = 0.2f;

    // Maximum movement allowed for a tap (in pixels)
    public float maxTapMovement = 10f;

    // Events to trigger on each swipe direction and tap
    public UnityEvent OnSwipeUp;
    public UnityEvent OnSwipeDown;
    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;
    public UnityEvent OnTap;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float touchStartTime;
    private bool swipeInProgress = false;

    void Update()
    {
        DetectSwipeOrTap();
    }

    void DetectSwipeOrTap()
    {
        #region Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    swipeInProgress = true;
                    startTouchPosition = touch.position;
                    touchStartTime = Time.time;
                    break;

                case TouchPhase.Moved:
                    // Optional: Update for real-time swipe tracking
                    break;

                case TouchPhase.Ended:
                    if (swipeInProgress)
                    {
                        endTouchPosition = touch.position;
                        float touchDuration = Time.time - touchStartTime;
                        EvaluateSwipeOrTap(touchDuration);
                        swipeInProgress = false;
                    }
                    break;
            }
        }
        #endregion

        #region Mouse Input (Editor Testing)
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                swipeInProgress = true;
                startTouchPosition = Input.mousePosition;
                touchStartTime = Time.time;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (swipeInProgress)
                {
                    endTouchPosition = Input.mousePosition;
                    float touchDuration = Time.time - touchStartTime;
                    EvaluateSwipeOrTap(touchDuration);
                    swipeInProgress = false;
                }
            }
        }
        #endregion
    }

    void EvaluateSwipeOrTap(float touchDuration)
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;
        float swipeDistance = swipeDelta.magnitude;

        if (swipeDistance >= minSwipeDistance)
        {
            swipeDelta.Normalize();

            // Determine swipe direction
            if (IsSwipeUp(swipeDelta))
            {
                OnSwipeUp.Invoke();
            }
            else if (IsSwipeDown(swipeDelta))
            {
                OnSwipeDown.Invoke();
            }
            else if (IsSwipeLeft(swipeDelta))
            {
                OnSwipeLeft.Invoke();
            }
            else if (IsSwipeRight(swipeDelta))
            {
                OnSwipeRight.Invoke();
            }
        }
        else if (swipeDistance <= maxTapMovement && touchDuration <= maxTapTime)
        {
            // It's a tap
            OnTap.Invoke();
        }
        // Else, the input is too small to be considered a swipe or tap
    }

    bool IsSwipeUp(Vector2 swipeDirection)
    {
        return Vector2.Dot(swipeDirection, Vector2.up) > 0.7f;
    }

    bool IsSwipeDown(Vector2 swipeDirection)
    {
        return Vector2.Dot(swipeDirection, Vector2.down) > 0.7f;
    }

    bool IsSwipeLeft(Vector2 swipeDirection)
    {
        return Vector2.Dot(swipeDirection, Vector2.left) > 0.7f;
    }

    bool IsSwipeRight(Vector2 swipeDirection)
    {
        return Vector2.Dot(swipeDirection, Vector2.right) > 0.7f;
    }
}
