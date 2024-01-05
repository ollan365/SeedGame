using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackendChartData : MonoBehaviour
{
    public static List<LevelChartData> levelChart;
    static BackendChartData()
    {
        levelChart = new List<LevelChartData>();
    }
    public static void LoadAllChart()
    {
        LoadLevelChart();
    }
    private static void LoadLevelChart()
    {
        var bro = Backend.Chart.GetChartContents(Constants.LEVEL_CHART);
        if (bro.IsSuccess())
        {
            // JSON ������ �Ľ� ����
            try
            {
                LitJson.JsonData jsonData = bro.FlattenRows();

                // �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                if(jsonData.Count <= 0)
                {
                    Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                }
                else
                {
                    for (int i = 0; i < jsonData.Count; ++i) {
                        LevelChartData newChart = new LevelChartData();
                        newChart.level = int.Parse(jsonData[i]["level"].ToString());
                        newChart.maxExperience = int.Parse(jsonData[i]["maxExperience"].ToString());
                        newChart.rewardSeed = int.Parse(jsonData[i]["rewardSeed"].ToString());

                        levelChart.Add(newChart);

                    }
                }
            }
            // JSON ������ �Ľ� ����
            catch(System.Exception e)
            {
                // try-catch ���� ���
                Debug.LogError(e);
            }
        }
        else
        {
            Debug.LogError($"{Constants.LEVEL_CHART}�� ��Ʈ �ҷ����⿡ ���� �߻� : {bro}");
        }
    }
}


[System.Serializable]
public class LevelChartData
{
    public int level;
    public int maxExperience;
    public int rewardSeed;
}