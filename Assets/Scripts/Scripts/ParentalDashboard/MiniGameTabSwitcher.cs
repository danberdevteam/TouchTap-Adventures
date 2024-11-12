using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameTabSwitcher : MonoBehaviour
{
    public List<GameObject> Tabs;
    public GameObject PanelBox;
    public MiniGameStatsPanel[] TabsPanel;
    public Button[] TabButtons;
    public Color InactiveTabBG, ActiveTabBG;
    public Vector2 InactiveTabButtonSize, ActiveTabButtonSize;

    public SessionData sessionData;


    [SerializeField] GameObject TabButtonPrefabs;
    [SerializeField] GameObject SessionPanelPrefab;
    void Start()
    {
        int gNumber = 0;
        foreach (MinigameStats stats in sessionData.minigames)
        {
            // dummyTextRemoveMe.text += "\n" + stats.gameName;
            Instantiate(TabButtonPrefabs, this.transform).GetComponentInChildren<TMP_Text>().text = stats.gameName;
            GameObject panel = Instantiate(SessionPanelPrefab, PanelBox.transform);
            panel.GetComponent<MiniGameStatsPanel>().minigameStats = stats;
            gNumber++;
        }
        TabButtons = GetComponentsInChildren<Button>();
        for (int i = 0; i < TabButtons.Length; i++)
        {
            int tabIndex = i;
            print("Button vals" + i);
            TabButtons[i].onClick.AddListener(() => SwitchToTab(tabIndex));
        }
        TabsPanel = PanelBox.GetComponentsInChildren<MiniGameStatsPanel>();
        foreach (MiniGameStatsPanel tab in TabsPanel)
        {
            Tabs.Add(tab.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchToTab(int TabID)
    {
        // print(TabID);
        foreach (GameObject tab in Tabs)
        {
            tab.gameObject.SetActive(false);
        }

        Tabs[TabID].gameObject.SetActive(true);
        foreach (Button button in TabButtons)
        {
            button.image.color = InactiveTabBG;
            // button.image.rectTransform.sizeDelta = InactiveTabButtonSize;
        }
        TabButtons[TabID].image.color = ActiveTabBG;
        // TabButtons[TabID].image.rectTransform.sizeDelta = ActiveTabButtonSize;
    }
}
