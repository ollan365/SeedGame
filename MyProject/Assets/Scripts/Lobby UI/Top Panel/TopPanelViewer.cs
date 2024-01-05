using UnityEngine;
using UnityEngine.UI;

public class TopPanelViewer : MonoBehaviour
{
	[SerializeField]
	private Text textNickname;
	[SerializeField]
	private Text textID;
	[SerializeField]
	private Text textLevel;
	[SerializeField]
	private Slider sliderExperience;
	[SerializeField]
	private Text textGoldSeed;
	[SerializeField]
	private Text textSeed;

	private void Awake()
	{
		BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
	}

	public void UpdateNickname()
	{
		// �г����� ������ gamer_id�� ����ϰ�, �г����� ������ �г��� ���
		textNickname.text = UserInfo.Data.nickname == null ?
							UserInfo.Data.gamerId : UserInfo.Data.nickname;
		textID.text = UserInfo.Data.gamerId;
	}

	public void UpdateGameData()
	{
		int currentLevel = BackendGameData.Instance.UserGameData.level;

		textLevel.text = $"{BackendGameData.Instance.UserGameData.level}";
		// �ӽ÷� �ִ� ����ġ�� 100���� ����
		sliderExperience.value = BackendGameData.Instance.UserGameData.experience /
								 BackendChartData.levelChart[currentLevel].maxExperience;
		textGoldSeed.text = $"{BackendGameData.Instance.UserGameData.goldSeed}";
		textSeed.text = $"{BackendGameData.Instance.UserGameData.seed}";
	}
}



