using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Slider : MonoBehaviour, IDragHandler, IEndDragHandler
{
  public Image fillArea;
  public Transform start;
  public Transform end;

  public List<Vector2[]> polygons = new List<Vector2[]>();

  public bool onTrigger = false;
  public bool finish = false;
  public float pathDistance;
  public float currentDistance;

  void Start()
  {
    GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0.6f);
    pathDistance = Vector3.Distance(transform.position, end.position);
    currentDistance = Vector3.Distance(transform.position, end.position);

  }

  public void OnDrag(PointerEventData eventData)
  {
    currentDistance = Vector3.Distance(start.position, transform.position);

    Vector2 position = transform.localPosition;
    Vector2 newPos = position + eventData.delta;
    transform.localPosition = newPos;

    if (onTrigger && !finish)
    {
      float fillAmount = Mathf.Clamp(currentDistance / pathDistance, 0, 1);
      fillArea.fillAmount = fillAmount;
    }
  }

  void Update()
  {
    if (fillArea.fillAmount == 1)
    {
      finish = true;
    }
  }

  void OnTriggerStay2D(Collider2D other)
  {
    if (other.tag == "Fill")
    {
      onTrigger = true;
      print(" exited end");
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (other.tag == "Fill")
    {
      onTrigger = false;
      print("exited btw");
    }

    if (other.name == end.name)
    {
      finish = true;

      fillArea.fillAmount = 1;
      AlphabetGameManager.Instance.IncreseCorrectLetter();
    }
  }

  // Handle the click event to reset the slider to the clicked position
  public void OnEndDrag(PointerEventData eventData)
  {
    // Check if the current position is close enough to the end position
    float distanceToEnd = Vector3.Distance(transform.position, end.position);
    if (distanceToEnd > 0.1f) // Adjust this threshold as needed
    {
      // Reset to the start position if not close enough to the end
      transform.position = start.position;
      fillArea.fillAmount = 0;
      finish = false;
      AlphabetGameManager.Instance.IncreaseMissedLetter();
    }
  }






}
