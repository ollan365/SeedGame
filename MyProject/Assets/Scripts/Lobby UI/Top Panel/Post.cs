using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System;       // TimeSpan

public class Post : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spriteItemIcons;       // ���� ���Ե� ������ �����ܿ� ����� �̹��� �迭
	[SerializeField]
	private Image imageItemIcon;            // ���� ���Ե� ������ ������ ���
	[SerializeField]
	private Text textItemCount;          // ���� ���Ե� �������� ����
	[SerializeField]
	private Text textTitle;              // ���� ����
	[SerializeField]
	private Text textContent;            // ���� ����
	[SerializeField]
	private Text textExpirationDate;     // ���� ������� ���� �ð� ���

	[SerializeField]
	private Button buttonReceive;           // ���� "����" ��ư ó��

	private BackendPostSystem backendPostSystem;
	private PostManager PM;
	private PostData postData;

	public void Setup(BackendPostSystem postSystem, PostManager postBox, PostData postData)
	{
		// ���� "����" ��ư�� ������ �� ó��
		buttonReceive.onClick.AddListener(OnClickPostReceive);

		backendPostSystem = postSystem;
		PM = postBox;
		this.postData = postData;

		// ���� ����� ���� ����
		textTitle.text = postData.title;
		textContent.text = postData.content;

		// ù ��° ������ ������ ���� ���
		foreach (string itemKey in postData.postReward.Keys)
		{
			// ���� ���Ե� ������ �̹��� ���
			if (itemKey.Equals("seed")) imageItemIcon.sprite = spriteItemIcons[0];
			else if (itemKey.Equals("goldSeed")) imageItemIcon.sprite = spriteItemIcons[1];
			else if (itemKey.Equals("acornSeed")) imageItemIcon.sprite = spriteItemIcons[2];
			else if (itemKey.Equals("chestnutSeed")) imageItemIcon.sprite = spriteItemIcons[3];
			else if (itemKey.Equals("cheeseSeed")) imageItemIcon.sprite = spriteItemIcons[4];
			else if (itemKey.Equals("fishSeed")) imageItemIcon.sprite = spriteItemIcons[5];
			else if (itemKey.Equals("appleSeed")) imageItemIcon.sprite = spriteItemIcons[6];
			else if (itemKey.Equals("randomSeed")) imageItemIcon.sprite = spriteItemIcons[7];
			else if (itemKey.Equals("acorn")) imageItemIcon.sprite = spriteItemIcons[8];
			else if (itemKey.Equals("chestnut")) imageItemIcon.sprite = spriteItemIcons[9];
			else if (itemKey.Equals("cheese")) imageItemIcon.sprite = spriteItemIcons[10];
			else if (itemKey.Equals("fish")) imageItemIcon.sprite = spriteItemIcons[11];
			else if (itemKey.Equals("apple")) imageItemIcon.sprite = spriteItemIcons[12];
			else if (itemKey.Equals("star")) imageItemIcon.sprite = spriteItemIcons[13];

			// ���� ���Ե� ������ ���� ���
			textItemCount.text = postData.postReward[itemKey].ToString();

			// �ϳ��� ���� ���Ե� �������� ���� �� �� ���� �ִµ� ���� ���������� ù ��° ������ ������ ���
			break;
		}

		// GetServerTime() - ���� �ð� �ҷ�����
		Backend.Utils.GetServerTime(callback =>
		{
			if (!callback.IsSuccess())
			{
				Debug.LogError($"���� �ð� �ҷ����⿡ �����߽��ϴ�. : {callback}");
				return;
			}

			// JSON ������ �Ľ� ����
			try
			{
				// ���� ���� �ð�
				string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();

				// ���� ������� ���� �ð� = ���� ���� �ð� - ���� ���� �ð�
				TimeSpan timeSpan = DateTime.Parse(postData.expirationDate) - DateTime.Parse(serverTime);

				// timeSpan.TotalHours�� ���� �Ⱓ�� ��(hour)�� ǥ��
				textExpirationDate.text = $"{timeSpan.TotalHours:F0}�ð� �� ����";
			}
			// JSON ������ �Ľ� ����
			catch (Exception e)
			{
				// try-catch ���� ���
				Debug.LogError(e);
			}
		});
	}

	private void OnClickPostReceive()
	{
		// ���� ���� UI ������Ʈ ����
		PM.DestroyPost(gameObject);
		// ���� ����
		backendPostSystem.PostReceive(PostType.Admin, postData.inDate);
	}
}

