using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadMenu()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void LoadDashBoard()
    {
        SceneManager.LoadScene("ParentalDashboard");
        
    }
    public void LoadLeaderBoard()
    {
        SceneManager.LoadScene("Leaderboard");
    }
    public void LoadIAPMenu()
    {
        SceneManager.LoadScene("PurchasingScene");
    }
}
