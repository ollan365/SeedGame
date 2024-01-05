using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SunFlower : MonoBehaviour
{
	private float nextStatusTime;        // ���� ���·� ���� ���� �ɸ��� �ð�
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
			case 0: // ���� �ƹ��͵� �ɰ��� ���� ����
				time = 0;
				nextStatusTime = 0f;
				image.color = new Color(1, 1, 1, 1);

				image.sprite = FlowerSprites[status];
				status++;

				if (FM.ElephantLock == false)
					Invoke("OnClick", 1f);

				break;
			case 1: // ���� �ѹ��� ����
                if (time > nextStatusTime)
				{
					time = 0;
					nextStatusTime = Random.Range(10f, 15f);

					image.sprite = FlowerSprites[status];
					status++;

					Invoke("ChangeDry", nextStatusTime);
				}
				break;
			case 3: // �� �� �߰� ����
				if (time > nextStatusTime)
				{
					time = 0;
					nextStatusTime = Random.Range(12f, 17f);

					image.sprite = FlowerSprites[status];
					status++;

					Invoke("ChangeBloom", nextStatusTime);
				}
				break;
			case 5: // ��Ȯ�ؾ� ��
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
