using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
public class GraphView : MonoBehaviour
{
    // Start is called before the first frame update
    LineChart lineChart;
    public String HeadingText; public List<String> SessionNumber; public List<float> data;

    void Start()
    {
        lineChart = GetComponent<LineChart>();
        lineChart.EnsureChartComponent<Title>().text = HeadingText;
        // lineChart.EnsureChartComponent<Title>().text. = HeadingText;
        for (int i = 0; i < SessionNumber.Count; i++)
        {
            lineChart.EnsureChartComponent<XAxis>().AddData(SessionNumber[i]);
            lineChart.AddData(0, data[i]);
        }
    }

    public void RemoveGraph()
    {
        Destroy(this.gameObject);
    }

}
