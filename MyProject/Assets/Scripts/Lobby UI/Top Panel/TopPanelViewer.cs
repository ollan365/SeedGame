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
		// 닉네임이 없으면 gamer_id를 출력하고, 닉네임이 있으면 닉네임 출력
		textNickname.text = UserInfo.Data.nickname == null ?
							UserInfo.Data.gamerId : UserInfo.Data.nickname;
		textID.text = UserInfo.Data.gamerId;
	}

	public void UpdateGameData()
	{
		int currentLevel = BackendGameData.Instance.UserGameData.level;

		textLevel.text = $"{BackendGameData.Instance.UserGameData.level}";
		// 임시로 최대 경험치를 100으로 설정
		sliderExperience.value = BackendGameData.Instance.UserGameData.experience /
								 BackendChartData.levelChart[currentLevel].maxExperience;
		textGoldSeed.text = $"{BackendGameData.Instance.UserGameData.goldSeed}";
		textSeed.text = $"{BackendGameData.Instance.UserGameData.seed}";
	}
}



