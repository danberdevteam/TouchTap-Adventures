using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // Variables for sinusoidal movement
    public float amplitude = 0.5f;  // The height of the movement
    public float frequency = 1f;    // How fast the object oscillates

    // Rotation speed
    public float rotationSpeed = 50f;  // Rotation speed in degrees per second

    // Private variable to track the initial position
    private Vector3 startPosition;

    public PickupType specialType = PickupType.simple;
    
    public bool specialPickup = false;
    [Tooltip("setup A,B,C or shapes objects if your pickups have multiple objects")]
    public GameObject[] specialPickupObjects;
    
    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
        //if (specialType != PickupType.simple)  SetSpecialPickup();
    }

    void Update()
    {
        // Sinusoidal movement
        // float newY = Mathf.Sin(Time.time * frequency) * amplitude;
        // transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);

        // Constant rotation
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    public void SetSpecialPickup(int idx = 0)
    {
        if (specialPickupObjects.Length == 0)   return;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        specialPickupObjects[idx].gameObject.SetActive(true);
    }
    
    public void SetSpecialPickup(string name)
    {
        if (specialPickupObjects.Length == 0 || name == "nil")   return;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == name)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        //specialPickupObjects[idx].gameObject.SetActive(true);
    }
}

public enum PickupType
{
    simple,
    alphabet,
    shape
}
