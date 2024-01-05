using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class Flowers : MonoBehaviour
{
	private float nextStatusTime;        // 다음 상태로 가기 위해 걸리는 시간
	private int status;
	private float time;

	[SerializeField] private LobbyGameManager GM;
	private int type;
	[SerializeField] private Inventory IM;

	private Image image;
	[SerializeField] private List<Sprite> Seeds;
	[SerializeField] private List<Sprite> Seeds_Wet;
	[SerializeField] private List<Sprite> Middle;
	[SerializeField] private List<Sprite> Middle_Wet;
	[SerializeField] private List<Sprite> Fruit;

	[SerializeField] private GameObject SeedPanel;

	[SerializeField] private GameObject Lock;
	[SerializeField] private int unlockLevel;

	private void Awake()
	{
		image = GetComponent<Image>();
		image.color = new Color(1, 1, 1, 0);
		time = 0;
	}
    private void Start()
    {
		if (BackendGameData.Instance.UserGameData.level >= unlockLevel)
			Lock.SetActive(false);
	}
    private void Update()
	{
		time += Time.deltaTime;
	}
	public void OnClick()
	{
		if (BackendGameData.Instance.UserGameData.level < unlockLevel)
			return;

		switch (status)
		{
			case 0:
				type = GM.SeedType;

				if (IM.getSeedNum(type) <= 0)
				{
					GM.Message("씨앗 부족");
					return;
				}
				IM.ChangeSeedNum(type, -1);

				time = 0;
				nextStatusTime = 0f;
				status++;

				image.color = new Color(1, 1, 1, 1);

                if (type == 5) // 랜덤 씨앗
                {
					type = ReadRandomSeed();
				}
				image.sprite = Seeds[type];

				break;
			case 1: // 물을 한번도 안줌
				if (time > nextStatusTime)
				{
					time = 0;
					nextStatusTime = Random.Range(10f, 15f) + type;
					status++;

					image.sprite = Seeds_Wet[type];
					Invoke("ChangeDry", nextStatusTime);
				}
				break;
			case 2: // 물을 한번 준 상태
				if (time > nextStatusTime)
				{
					time = 0;
					nextStatusTime = Random.Range(12f, 17f) + type;
					status++;

					image.sprite = Middle_Wet[type];
					Invoke("ChangeBloom", nextStatusTime);
				}
				break;
			case 3: // 수확해야 함
				if (time > nextStatusTime)
				{
					status = 0;
					time = 0;

					image.color = new Color(1, 1, 1, 0);
					IM.ChangeFruitNum(type, 1);

					BackendGameData.Instance.UserGameData.ListOfFarm[type]++;
				}
				break;
		}
	}

	private void ChangeDry()
	{
		image.sprite = Middle[type];
	}
	private void ChangeBloom()
	{
		image.sprite = Fruit[type];
	}

	private int ReadRandomSeed()
	{
		TextAsset textFile = Resources.Load("RandomSeed") as TextAsset;
		StringReader stringReader = new StringReader(textFile.text);

		int random = Random.Range(0, 100);
		while (stringReader != null)
		{
			string line = stringReader.ReadLine();
			if (line == null) break;

			if (int.Parse(line.Split(',')[1]) > random)
			{
				return int.Parse(line.Split(',')[0]);
			}
		}
		return 4;
	}
}
