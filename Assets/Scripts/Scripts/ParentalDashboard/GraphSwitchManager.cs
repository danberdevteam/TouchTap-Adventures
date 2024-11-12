using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphSwitchManager : MonoBehaviour
{
    [SerializeField] GameObject ButtonHolder;
    [SerializeField] GameObject GraphPanelHolder;
    public Color InactiveTabBG, ActiveTabBG;
    public Vector2 InactiveTabButtonSize, ActiveTabButtonSize;

    Button[] switchButtons;
    GraphHandler[] graphHandlers;
    // Start is called before the first frame update
    void Start()
    {
        int tabIndex = 0;
        foreach (var button in ButtonHolder.GetComponentsInChildren<Button>())
        {
            int t = tabIndex;
            button.onClick.AddListener(() => SwitchToTab(t));
            tabIndex++;
            print(tabIndex + "Tab index");
        }
        switchButtons = ButtonHolder.GetComponentsInChildren<Button>();
        graphHandlers = GraphPanelHolder.GetComponentsInChildren<GraphHandler>();

        foreach (var panels in graphHandlers)
        {
            panels.gameObject.SetActive(false);
        }

        // print("GraphSwitcherCalled");
    }

    void SwitchToTab(int tabNumber)
    {
        print(tabNumber + "tabNUmber");

        foreach (GraphHandler sessionPanel in graphHandlers)
        {
            sessionPanel.gameObject.SetActive(false);
        }

        graphHandlers[tabNumber].gameObject.SetActive(true);
        foreach (Button button in switchButtons)
        {
            button.image.color = InactiveTabBG;
            button.image.rectTransform.sizeDelta = InactiveTabButtonSize;
        }
        switchButtons[tabNumber].image.color = ActiveTabBG;
        switchButtons[tabNumber].image.rectTransform.sizeDelta = ActiveTabButtonSize;
    }
}
