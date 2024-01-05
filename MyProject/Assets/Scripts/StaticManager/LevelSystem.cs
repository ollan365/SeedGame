using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private GameManager GM;

    public void Process()
    {
        int currentLevel = BackendGameData.Instance.UserGameData.level;

        // ������ �ѹ� �÷����� ������ ����ġ ȹ��
        BackendGameData.Instance.UserGameData.experience += GM.Stage * 10;

        // ���� ����ġ�� �ִ� ����ġ���� ũ�ų� ����, ���� ������ �ִ� �������� ���� ��
        if( BackendGameData.Instance.UserGameData.experience >= BackendChartData.levelChart[currentLevel-1].maxExperience &&
            BackendChartData.levelChart.Count > currentLevel)
        {
            // ������ ���� ����
            BackendGameData.Instance.UserGameData.seed += BackendChartData.levelChart[currentLevel - 1].rewardSeed;
            // ����ġ�� 0���� �ʱ�ȭ
            BackendGameData.Instance.UserGameData.experience = BackendGameData.Instance.UserGameData.experience - BackendChartData.levelChart[currentLevel - 1].maxExperience;
            // ���� 1 ����
            BackendGameData.Instance.UserGameData.level++;
        }

        // ���� ���� ������Ʈ
        BackendGameData.Instance.GameDataUpdate();
    }
}
