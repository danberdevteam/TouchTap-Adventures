using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class LevelViewChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public int levelViewNumber;
    LeanSelectableByFinger selector;
    void Start()
    {
        selector = GetComponent<LeanSelectableByFinger>();
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void ChangeLevel()
    {
        FindObjectOfType<TrackManager>().SetTrackTheme(levelViewNumber);
        print("Change level");

    }
}
