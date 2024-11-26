using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameCharacterSelector : MonoBehaviour
{
  static string CatIDPlayeprefKey = "CatSelected";
    
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(CatIDPlayeprefKey))
        {

        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
