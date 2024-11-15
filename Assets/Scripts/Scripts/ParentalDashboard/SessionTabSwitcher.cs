
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionTabSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject SessionsTabHolder, SessionsTabPanelPrefab;
    // public List<GameObject> Tab
    public SessionTabButton[] SessionsTabButtons;
    public Color InactiveTabBG, ActiveTabBG;
    public Vector2 InactiveTabButtonSize, ActiveTabButtonSize;
    public List<GameObject> SessionPanels;

    [SerializeField] ParentalManager manager;


    void Start()
    {
        SessionsTabButtons = GetComponentsInChildren<SessionTabButton>();
        for (int i = 0; i < SessionsTabButtons.Length; i++)
        {
            int tabIndex = i;
            // print("Button vals" + i);
            SessionsTabButtons[i].GetComponent<Button>().onClick.AddListener(() => SwitchToTab(tabIndex));
            GameObject instantiatedPanel = Instantiate(SessionsTabPanelPrefab, SessionsTabHolder.transform);
            instantiatedPanel.GetComponentInChildren<MiniGameTabSwitcher>().sessionData.minigames = manager.dataCollector.minigameStats[i];
            SessionPanels.Add(instantiatedPanel);
            if (i != 0)
            {
                instantiatedPanel.SetActive(false);
            }
            else
            {
                SessionsTabButtons[0].GetComponent<Button>().image.color = ActiveTabBG;
            }

        }



    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchToTab(int TabID)
    {
        // print(TabID);
        foreach (GameObject sessionPanel in SessionPanels)
        {
            sessionPanel.gameObject.SetActive(false);
        }

        SessionPanels[TabID].gameObject.SetActive(true);
        foreach (SessionTabButton button in SessionsTabButtons)
        {
            button.transform.GetComponent<Button>().image.color = InactiveTabBG;
            // button.GetComponent<Button>().image.rectTransform.sizeDelta = InactiveTabButtonSize;
        }
        SessionsTabButtons[TabID].GetComponent<Button>().image.color = ActiveTabBG;
        // SessionsTabButtons[TabID].GetComponent<Button>().image.rectTransform.sizeDelta = ActiveTabButtonSize;
    }
}
