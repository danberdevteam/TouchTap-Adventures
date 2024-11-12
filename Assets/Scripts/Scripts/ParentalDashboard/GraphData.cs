
using System;
using System.Collections.Generic;
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

    void Start()
    {
        lineChart = GetComponent<LineChart>();

        switch (graphType)
        {
            case GraphType.IncorrectClicks:
                lineChart.EnsureChartComponent<Title>().text = "Incorrect Clicks";
                for (int i = 0; i < SessionNumber.Count; i++)
                {
                    lineChart.EnsureChartComponent<XAxis>().AddData(SessionNumber[i]);
                    lineChart.AddData(0, InCorrectClicks[i]);
                }
                break;
            case GraphType.Accuracy:
                lineChart.EnsureChartComponent<Title>().text = "Accuracy";
                for (int i = 0; i < SessionNumber.Count; i++)
                {
                    lineChart.EnsureChartComponent<XAxis>().AddData(SessionNumber[i]);
                    lineChart.AddData(0, Accuracy[i]);
                }
                break;
            case GraphType.TImeSpent:
                lineChart.EnsureChartComponent<Title>().text = "Time Spent";
                for (int i = 0; i < SessionNumber.Count; i++)
                {
                    lineChart.EnsureChartComponent<XAxis>().AddData(SessionNumber[i]);
                    lineChart.AddData(0, TimeSpent[i]);
                }
                break;
        }




    }

    // Update is called once per frame
    void Update()
    {

    }
}
