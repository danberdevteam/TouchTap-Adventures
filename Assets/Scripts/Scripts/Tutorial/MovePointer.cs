using UnityEngine.Splines;
using UnityEngine;

public class MovePointer : MonoBehaviour
{
    public SplineContainer splineContainer; // Reference to the SplineContainer
    public float speed = 2f; // Speed of movement
    private Spline spline;
    private float t = 0f; // Parameter to traverse the spline

    void Start()
    {
        // if (splineContainer != null)
        // {
        //     spline = splineContainer.Spline;
        // }
        spline = GetComponentInParent<SplineContainer>().Spline;
    }

    void Update()
    {
        if (spline == null) return;

        // Move parameter along the spline
        t += speed * Time.deltaTime / spline.GetLength();

        if (t > 1f)
        {
            t -= 1f; // Loop back to the start
        }

        // Get the position on the spline at parameter t
        Vector3 position = spline.EvaluatePosition(t);
        transform.localPosition = position;
    }
}
