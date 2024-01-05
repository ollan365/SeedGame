using BackEnd;
using UnityEngine;

public class DailyRankRegister : MonoBehaviour
{
	public void Process(int newScore)
	{
		UpdateMyRankData(newScore);
	}

	private void UpdateMyRankData(int newScore)
	{
		string rowInDate = string.Empty;

		// ��ŷ �����͸� ������Ʈ�Ϸ��� ���� �����Ϳ��� ����ϴ� �������� inDate ���� �ʿ�
		Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
		{
			if (!callback.IsSuccess())
			{
				Debug.LogError($"������ ��ȸ �� ������ �߻��߽��ϴ� : {callback}");
				return;
			}

			Debug.Log($"������ ��ȸ�� �����߽��ϴ� : {callback}");

			if (callback.FlattenRows().Count > 0)
			{
				rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
			}
			else
			{
				Debug.LogError("�����Ͱ� �������� �ʽ��ϴ�.");
				return;
			}

			Param param = new Param()
			{
				{ "bestStage", newScore }
			};

			Backend.URank.User.UpdateUserScore(Constants.DAILY_RANK_UUID, Constants.USER_DATA_TABLE, rowInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"��ŷ ��Ͽ� �����߽��ϴ� : {callback}");
				}
				else
				{
					Debug.LogError($"��ŷ ��� �� ������ �߻��߽��ϴ� : {callback}");
				}
			});
		});
	}
}

