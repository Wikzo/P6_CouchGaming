using UnityEngine;
using System.Collections;

public class PieChartMeshController : MonoBehaviour
{
    PieChartMesh mPieChart;
    float[] mData;

    float[] defaultValues;

    void Start()
    {
        StartCoroutine(WaitThenSetup());
    }

    IEnumerator WaitThenSetup()
    {
        yield return new WaitForSeconds(1f);
        SetupPieChart();
    }

    void SetupPieChart()
    {
        mPieChart = gameObject.AddComponent("PieChartMesh") as PieChartMesh;
        if (mPieChart != null)
        {
            mPieChart.Init(mData, 100, 0, 100, null);

            if (DataSaver.Instance.highScores != null)
            {
                mData = new float[4];
                foreach (var score in DataSaver.Instance.highScores)
                {
                    mData[0] = score.RedWins;
                    mData[1] = score.BlueWins;
                    mData[2] = score.GreenWins;
                    mData[3] = score.PinkWins;
                }
            }
            else
            {
                defaultValues = new float[4] { 0, 0, 0, 0 };
                mData = defaultValues;
            }
            mPieChart.Draw(mData);
        }
    }

    float[] GenerateRandomValues(int length)
    {
        float[] targets = new float[length];

        for (int i = 0; i < length; i++)
        {
            targets[i] = Random.Range(0f, 100f);
        }
        return targets;
    }
}
