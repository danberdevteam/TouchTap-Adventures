using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameCharacterSelectorManager : MonoBehaviour
{


    public List<Material> catMaterials;

    int currentInt = 0;
    static string CatIDPlayeprefKey = "CatSelected";

    public GameObject CatPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(CatIDPlayeprefKey))
        {
            currentInt = PlayerPrefs.GetInt(CatIDPlayeprefKey);
        }

        CatPlayer.GetComponent<SkinnedMeshRenderer>().material = catMaterials[currentInt];


    }

    // Update is called once per frame
    void Update()
    {

    }
}
