using UnityEngine;
using TMPro;

public class ColorLooper : MonoBehaviour
{
    public TMP_Text tmpText;            // Reference to the TMP_Text component
    public Color[] colors;              // Array of colors to loop through
    public float colorChangeSpeed = 1f; // Speed of color change

    private int currentColorIndex = 0;
    private int nextColorIndex = 1;
    private float t = 0f;

    void Start()
    {
        if (colors.Length < 2)
        {
            Debug.LogError("Please assign at least two colors to the colors array.");
            return;
        }
        tmpText=GetComponent<TMP_Text>();

        tmpText.color = colors[currentColorIndex];
    }

    void Update()
    {
        // Lerp between the current color and the next color
        tmpText.color = Color.Lerp(colors[currentColorIndex], colors[nextColorIndex], t);

        // Increment t based on the change speed
        t += Time.deltaTime * colorChangeSpeed;

        // Check if the transition is complete
        if (t >= 1f)
        {
            // Reset t
            t = 0f;

            // Update the current and next color indices
            currentColorIndex = nextColorIndex;
            nextColorIndex = (nextColorIndex + 1) % colors.Length;
        }
    }
}
