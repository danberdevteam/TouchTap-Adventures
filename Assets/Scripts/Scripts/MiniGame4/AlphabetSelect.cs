using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;


public enum Letter
{
    a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z
}
public class AlphabetSelect : MonoBehaviour
{
    // Start is called before the first frame update
    LeanSelectableByFinger selector;
    public Letter letter;

    void Start()
    {
        selector = GetComponent<LeanSelectableByFinger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selector.IsSelected)
        {

            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero); // Assuming the ground is at y=0

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                // Set the GameObject's position to the touch position on the x-z plane
                transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
            }
        }
    }
}
