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
            // JSON 데이터 파싱 성공
            try
            {
                LitJson.JsonData jsonData = bro.FlattenRows();

                // 받아온 데이터의 개수가 0이면 데이터가 없는 것
                if(jsonData.Count <= 0)
                {
                    Debug.LogWarning("데이터가 존재하지 않습니다.");
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
            // JSON 데이터 파싱 실패
            catch(System.Exception e)
            {
                // try-catch 에러 출력
                Debug.LogError(e);
            }
        }
        else
        {
            Debug.LogError($"{Constants.LEVEL_CHART}의 차트 불러오기에 에러 발생 : {bro}");
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