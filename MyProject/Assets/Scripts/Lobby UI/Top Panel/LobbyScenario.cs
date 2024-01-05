using UnityEngine;
using UnityEngine.UI;

public class LobbyScenario : MonoBehaviour
{
	[SerializeField]
	private UserInfo user;
	[SerializeField]
	private Inventory inventory;

	private void Awake()
	{
		user.GetUserInfoFromBackend();
	}
    private void Start()
    {
		BackendGameData.Instance.inventory = inventory;
		BackendGameData.Instance.GameDataLoad();
    }
	public void StartGame()
	{
		if (BackendGameData.Instance.UserGameData.seed < 1000)
			return;
		BackendGameData.Instance.UserGameData.acorn = inventory.getFruitNum(0);
		BackendGameData.Instance.UserGameData.chestnut = inventory.getFruitNum(1);
		BackendGameData.Instance.UserGameData.cheese = inventory.getFruitNum(2);
		BackendGameData.Instance.UserGameData.fish = inventory.getFruitNum(3);
		BackendGameData.Instance.UserGameData.apple = inventory.getFruitNum(4);
		Utils.LoadScene("Game");
    }
}