
using System;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using XCharts.Runtime;


public enum GraphType
{
    IncorrectClicks, Accuracy, TImeSpent
}

public class GraphData : MonoBehaviour
{
    // Start is called before the first frame update
    LineChart lineChart;
    public List<String> SessionNumber;
    public List<float> InCorrectClicks;
    public List<float> Accuracy;
    public List<float> TimeSpent;
    public GraphType graphType;
    Vector3 originalSize;
    String graphHead;
    public List<float> data;

    [SerializeField] GraphView graphView;

    void Start()
    {

        lineChart = GetComponent<LineChart>();

        switch (graphType)
        {
            case GraphType.IncorrectClicks:
                lineChart.EnsureChartComponent<Title>().text = "Incorrect Clicks";
                graphHead = "Incorrect Clicks";
                for (int i = 0; i < SessionNumber.Count; i++)
                {
                    lineChart.EnsureChartComponent<XAxis>().AddData(SessionNumber[i]);
                    lineChart.AddData(0, InCorrectClicks[i]);
                    data = InCorrectClicks;
                }
                break;
            case GraphType.Accuracy:
                lineChart.EnsureChartComponent<Title>().text = "Accuracy";
                graphHead = "Accuracy";
                for (int i = 0; i < SessionNumber.Count; i++)
                {
                    lineChart.EnsureChartComponent<XAxis>().AddData(SessionNumber[i]);
                    lineChart.AddData(0, Accuracy[i]);
                    data = Accuracy;
                }
                break;
            case GraphType.TImeSpent:
                lineChart.EnsureChartComponent<Title>().text = "Time Spent";
                graphHead = "Time Spent";
                for (int i = 0; i < SessionNumber.Count; i++)
                {
                    lineChart.EnsureChartComponent<XAxis>().AddData(SessionNumber[i]);
                    lineChart.AddData(0, TimeSpent[i]);
                    data = TimeSpent;
                }
                break;
        }
        originalSize = this.transform.localScale;



    }
    bool cliked = false;
    GraphView view;
    public void GraphClicked()
    {
        if (!cliked)
        {
            if (this.transform.parent.transform.parent.GetComponentsInChildren<GraphView>().Length > 0)
            {
                foreach (var grV in this.transform.parent.transform.parent.GetComponentsInChildren<GraphView>())
                {
                    Destroy(grV.gameObject);
                }
            }
            view = Instantiate(graphView, this.transform.parent.transform.parent);
            view.HeadingText = graphHead;
            view.SessionNumber = SessionNumber;
            view.data = data;
        }
        else
        {
            view.RemoveGraph();
            cliked = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
