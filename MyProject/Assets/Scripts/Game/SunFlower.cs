using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SunFlower : MonoBehaviour
{
	private float nextStatusTime;        // 다음 상태로 가기 위해 걸리는 시간
	private int status;
	private float time;

	private Image image;
	[SerializeField] private GameObject Lock;
	[SerializeField] private List<Sprite> FlowerSprites;

	[SerializeField] private GameManager GM;
	[SerializeField] private FriendManager FM;
    private void Awake()
    {
		image = GetComponent<Image>();
		image.color = new Color(1, 1, 1, 0);
		time = 0;
    }
    private void Update()
    {
		time += Time.deltaTime;
    }
    public void OnClick()
    {
        switch (status)
        {
			case 0: // 아직 아무것도 심겨져 있지 않음
				time = 0;
				nextStatusTime = 0f;
				image.color = new Color(1, 1, 1, 1);

				image.sprite = FlowerSprites[status];
				status++;

				if (FM.ElephantLock == false)
					Invoke("OnClick", 1f);

				break;
			case 1: // 물을 한번도 안줌
                if (time > nextStatusTime)
				{
					time = 0;
					nextStatusTime = Random.Range(10f, 15f);

					image.sprite = FlowerSprites[status];
					status++;

					Invoke("ChangeDry", nextStatusTime);
				}
				break;
			case 3: // 물 준 중간 상태
				if (time > nextStatusTime)
				{
					time = 0;
					nextStatusTime = Random.Range(12f, 17f);

					image.sprite = FlowerSprites[status];
					status++;

					Invoke("ChangeBloom", nextStatusTime);
				}
				break;
			case 5: // 수확해야 함
				if (time > nextStatusTime)
				{
					status = 0;
					time = 0;
					GM.Seed += Random.Range(30, 40);

					BackendGameData.Instance.UserGameData.ListOfFarm[5]++;
					image.color = new Color(1, 1, 1, 0);
				}
				break;
        }
    }

	private void ChangeDry()
	{
		image.sprite = FlowerSprites[status];
		status++;

		if (FM.ElephantLock == false)
			Invoke("OnClick", 1f);
	}
	private void ChangeBloom()
	{
		image.sprite = FlowerSprites[status];
		status++;
	}
}
