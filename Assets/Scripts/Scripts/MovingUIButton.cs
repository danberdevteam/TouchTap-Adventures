using UnityEngine;
using UnityEngine.EventSystems;

public class MovingUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed = false;

    public Player player;


    void Start()
    {

    }

    void FixedUpdate()
    {
        // Call the function continuously while the button is pressed
        if (isPressed)
        {
            // MovePlayer();
        }
    }

    // Detect when the button is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;

    }

    // Detect when the button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

    }

    // Function to call repeatedly while the button is pressed
   


}
