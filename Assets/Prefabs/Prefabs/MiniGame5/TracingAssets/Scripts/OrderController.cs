using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.EventSystems;
using System.Collections;

public class OrderController : MonoBehaviour /*IPointerClickHandler*/
{
  // public GameObject starPanel;
  public ChangeLetters changeLetter;
  public List<Slider> sliders = new List<Slider>();

  public List<SplineContainer> destinationPosition;
  public int pathCount = 0;
  public bool success = false;



  /// Line
  public GameObject Pointer;
  GameObject ptr;

  int activeSliderIndex = -1;
  public Canvas canvas;



  void Start()
  {
    ShowSlider(pathCount);
  }

  //   public void OnPointerClick(PointerEventData eventData)
  //   {

  //     sliders[activeSliderIndex].transform.position = 
  //     eventData.position;
  // // Camera.main.ScreenPointToRay(eventData.position;
  //     // Calculate the new distance and update the UI

  //   }

  void Update()
  {


    if (Input.touchCount > 0 && activeSliderIndex != -1)
    {
      Touch touch = Input.GetTouch(0);
      Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
      // print("touch pos" + touchPosition);


      // if (touch.phase == TouchPhase.Began)  // Check if the touch has just started
      // {
      //   Vector2 pos;
      //   RectTransformUtility.ScreenPointToLocalPointInRectangle(
      //       canvas.transform as RectTransform,
      //       touch.position,
      //       canvas.worldCamera,
      //       out pos);

      //   sliders[activeSliderIndex].transform.localPosition = new Vector3(pos.x, pos.y, sliders[activeSliderIndex].transform.position.z);
      //   Debug.Log("Touch position on canvas: " + pos);
      //   // Use pos, which is the touch position in canvas coordinates
      // }

    }

    if (success)
    {
      changeLetter.TimeScale = changeLetter.TimeScale + Time.deltaTime;
    }
    else
    {


      if (pathCount < sliders.Count && sliders[pathCount].finish)
      {
        Debug.Log("Show Path");
        pathCount++;
        ShowSlider(pathCount);
      }
      else if (pathCount == sliders.Count)
      {

        Destroy(ptr);
        Debug.Log("Finish");
        pathCount = 0;
        success = true;
        // isFinished=true;
        transform.DOScale(1.2f, 1f)  // Scale to 1.5 over 1 second
       .SetLoops(-1, LoopType.Yoyo)  // Loop infinitely, and yoyo means it will go back and forth
       .SetEase(Ease.InOutSine);
        // localPointer.gameObject.SetActive(false);
        changeLetter.finishParticleEffect.Play();
        changeLetter.TimeScale = 0;
        StartCoroutine(ScrollToNext());
      }
    }




  }

  IEnumerator ScrollToNext()
  {
    yield return new WaitForSeconds(2);
    changeLetter.ScrollToNextWord();
  }

  void ShowSlider(int index)
  {
    for (int i = 0; i < sliders.Count; i++)
    {
      if (i == index)
      {
        sliders[i].gameObject.SetActive(true);
        ShowHelperLine(i);
        activeSliderIndex = i;
      }

      else sliders[i].gameObject.SetActive(false);

    }
  }

  void ShowHelperLine(int index)
  {
    if (ptr)
    {
      Destroy(ptr);
    }
    ptr = Instantiate(Pointer, destinationPosition[index].gameObject.transform);
    print(index + "index");
  }
}
