using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverUIController : MonoBehaviour
{
	[SerializeField]
	private GameManager GM;

	[Header("Game Over")]
	[SerializeField] private GameObject panelGameOver;
	[SerializeField] private Text textStage;
	[SerializeField] private Text textBestStage;

	[SerializeField] private Text gameOverText;

	public void OnGameOver()
	{
		// GameOver Panel UI 활성화
		panelGameOver.SetActive(true);
		textBestStage.text = BackendGameData.Instance.UserGameData.bestStage.ToString();
		// "GAME OVER" 텍스트 크기 축소 애니메이션
		StartCoroutine(SizeProcess(gameOverText, 200, 100, 0.5f));
		// bestStage 텍스트 크기 축소 애니메이션
		StartCoroutine(SizeProcess(textBestStage, 160, 60, 0.5f));
		// 0 -> gameController.Score까지 점수를 카운팅하는 애니메이션
		StartCoroutine(CountingProcess(textStage, 0, GM.Stage, 3));
		// 획득 점수 출력
		textStage.text = GM.Stage.ToString();
	}

	public void BtnClickGoToLobby()
	{
		Utils.LoadScene(SceneNames.Lobby);
	}

	private IEnumerator SizeProcess(Text effectText, float start, float end, float effectTime)
	{
		float current = 0;
		float percent = 0;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / effectTime; 

			effectText.fontSize = (int)Mathf.Lerp(start, end, percent);

			yield return null;
		}
	}

	private IEnumerator CountingProcess(Text effectText, int start, int end, float effectTime)
	{
		float current = 0;
		float percent = 0;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / effectTime;

			effectText.text = Mathf.Lerp(start, end, percent).ToString("F0");

			yield return null;
		}
	}
}

