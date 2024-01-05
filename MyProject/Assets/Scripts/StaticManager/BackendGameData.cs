using UnityEngine;
using BackEnd;
using UnityEngine.Events;

public class BackendGameData
{
	[System.Serializable]
	public class GameDataLoadEvent : UnityEvent { }
	public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

	[System.Serializable]
	public class InventoryLoadEvent : UnityEvent { }
	public InventoryLoadEvent inventoryLoadEvent = new InventoryLoadEvent();

	private static BackendGameData instance = null;
	public static BackendGameData Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new BackendGameData();
			}

			return instance;
		}
	}

	private UserGameData userGameData = new UserGameData();
	public UserGameData UserGameData => userGameData;

	public Inventory inventory;

	private string gameDataRowInDate = string.Empty;
	private string inventoryDataRowInDate = string.Empty;
	private string listDataRowInDate = string.Empty;

	/// <summary>
	/// �ڳ� �ܼ� ���̺� ���ο� ���� ���� �߰�
	/// </summary>
	public void GameDataInsert()
	{
		// ���� ������ �ʱⰪ���� ����
		userGameData.Reset();

		// ���̺� �߰��� �����ͷ� ����
		Param param = new Param()
		{
			{ "level",      userGameData.level },
			{ "experience", userGameData.experience },
			{ "seed",       userGameData.seed },
			{ "goldSeed",      userGameData.goldSeed },
			{ "bestStage",      userGameData.bestStage }
		};

		// ù ��° �Ű������� �ڳ� �ܼ��� "���� ���� ����" �ǿ� ������ ���̺� �̸�
		BackendReturnObject bro = Backend.GameData.Insert("USER_DATA", param);

		// ���� ���� �߰��� �������� ��
		if (bro.IsSuccess())
        {
			// ���� ������ ������
			gameDataRowInDate = bro.GetInDate();

			Debug.Log($"���� ���� ������ ���Կ� �����߽��ϴ�. : {bro}");
		}
		// �������� ��
		else
		{
			Debug.LogError($"���� ���� ������ ���Կ� �����߽��ϴ�. : {bro}");
		}


		param = new Param()
		{
			{ "acornSeed",      10 },
			{ "chestnutSeed", 10 },
			{ "cheeseSeed",       10 },
			{ "fishSeed",   10 },
			{ "appleSeed",  10 },
			{ "randomSeed",  10 },


			{ "acorn",  0 },
			{ "chestnut",  0 },
			{ "cheese",  0 },
			{ "fish",  0 },
			{ "apple",  0 },
			{ "star",  0 }
		};

		// ù ��° �Ű������� �ڳ� �ܼ��� "�κ��丮" �ǿ� ������ ���̺� �̸�
		bro = Backend.GameData.Insert("Inventory", param);

		// ���� ���� �߰��� �������� ��
		if (bro.IsSuccess())
		{
			// ���� ������ ������
			inventoryDataRowInDate = bro.GetInDate();

			Debug.Log($"���� ���� ������ ���Կ� �����߽��ϴ�. : {bro}");
		}
		// �������� ��
		else
		{
			Debug.LogError($"���� ���� ������ ���Կ� �����߽��ϴ�. : {bro}");
		}

		param = new Param()
		{
			// ��Ȯ
			{ "Farm_0",  0 },
			{ "Farm_1",  0 },
			{ "Farm_2",  0 },
			{ "Farm_3",  0 },
			{ "Farm_4",  0 },
			{ "Farm_5",  0 },
			{ "Farm_6",  0 },
			{ "Farm_7",  0 },

			// ����
			{ "Jelly_0",  0 },
			{ "Jelly_1",  0 },
			{ "Jelly_2",  0 },
			{ "Jelly_3",  0 },
			{ "Jelly_4",  0 },
			{ "Jelly_5",  0 },
			{ "Jelly_6",  0 },
			{ "Jelly_7",  0 },
			{ "Jelly_8",  0 },
			{ "Jelly_9",  0 },
			{ "Jelly_10",  0 },
			{ "Jelly_11",  0 },

			// ��ų
			{ "lv1_0", 0 },
			{ "lv1_1", 0 },
			{ "lv1_2", 0 },
			{ "lv1_3", 0 },
			{ "lv1_4", 0 },

			{ "lv2_0", 0 },
			{ "lv2_1", 0 },
			{ "lv2_2", 0 },
			{ "lv2_3", 0 },
			{ "lv2_4", 0 },

			{ "lv3_0", 0 },
			{ "lv3_1", 0 },
			{ "lv3_2", 0 },
			{ "lv3_3", 0 },
			{ "lv3_4", 0 },

			{ "lv4_0", 0 },
			{ "lv4_1", 0 },
			{ "lv4_2", 0 },
			{ "lv4_3", 0 },
			{ "lv4_4", 0 },
		};

		// ù ��° �Ű������� �ڳ� �ܼ��� "�κ��丮" �ǿ� ������ ���̺� �̸�
		bro = Backend.GameData.Insert("List", param);

		// ���� ���� �߰��� �������� ��
		if (bro.IsSuccess())
		{
			// ���� ������ ������
			listDataRowInDate = bro.GetInDate();

			Debug.Log($"���� ���� ������ ���Կ� �����߽��ϴ�. : {bro}");
		}
		// �������� ��
		else
		{
			Debug.LogError($"���� ���� ������ ���Կ� �����߽��ϴ�. : {bro}");
		}
	}

	/// <summary>
	/// �ڳ� �ܼ� ���̺��� ���� ������ �ҷ��� �� ȣ��
	/// </summary>
	public void GameDataLoad()
	{
		userGameData.Reset();

		var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
		// ���� ���� �ҷ����⿡ �������� ��
		if (bro.IsSuccess())
		{
			Debug.Log($"���� ���� ������ �ҷ����⿡ �����߽��ϴ�. : {bro}");

			// JSON ������ �Ľ� ����
			try
			{
				LitJson.JsonData gameDataJson = bro.FlattenRows();

				// �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
				if (gameDataJson.Count <= 0)
				{
					Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
				}
				else
				{
					// �ҷ��� ���� ������ ������
					gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
					// �ҷ��� ���� ������ userData ������ ����
					userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
					userGameData.experience = float.Parse(gameDataJson[0]["experience"].ToString());
					userGameData.seed = int.Parse(gameDataJson[0]["seed"].ToString());
					userGameData.goldSeed = int.Parse(gameDataJson[0]["goldSeed"].ToString());
					userGameData.bestStage = int.Parse(gameDataJson[0]["bestStage"].ToString());

					onGameDataLoadEvent?.Invoke();
				}
			}
			// JSON ������ �Ľ� ����
			catch (System.Exception e)
			{
				// ���� ������ �ʱⰪ���� ����
				userGameData.Reset();
				inventory.reset();
				// try-catch ���� ���
				Debug.LogError("����! : " + e);
			}
		}
		// �������� ��
		else
		{
			Debug.LogError($"���� ���� ������ �ҷ����⿡ �����߽��ϴ�. : {bro}");
		}


		bro = Backend.GameData.GetMyData("Inventory", new Where());
		// ���� ���� �ҷ����⿡ �������� ��
		if (bro.IsSuccess())
		{
			Debug.Log($"�κ��丮 ������ �ҷ����⿡ �����߽��ϴ�. : {bro}");

			// JSON ������ �Ľ� ����
			try
			{
				LitJson.JsonData gameDataJson = bro.FlattenRows();

				// �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
				if (gameDataJson.Count <= 0)
				{
					Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
				}
				else
				{
					// �ҷ��� ���� ������ ������
					inventoryDataRowInDate = gameDataJson[0]["inDate"].ToString();
					// �ҷ��� ���� ������ userData ������ ����
					inventory.seedList[0] = int.Parse(gameDataJson[0]["acornSeed"].ToString());
					inventory.seedList[1] = int.Parse(gameDataJson[0]["chestnutSeed"].ToString());
					inventory.seedList[2] = int.Parse(gameDataJson[0]["cheeseSeed"].ToString());
					inventory.seedList[3] = int.Parse(gameDataJson[0]["fishSeed"].ToString());
					inventory.seedList[4] = int.Parse(gameDataJson[0]["appleSeed"].ToString());
					inventory.seedList[5] = int.Parse(gameDataJson[0]["randomSeed"].ToString());

					inventory.fruitList[0] = int.Parse(gameDataJson[0]["acorn"].ToString());
					inventory.fruitList[1] = int.Parse(gameDataJson[0]["chestnut"].ToString());
					inventory.fruitList[2] = int.Parse(gameDataJson[0]["cheese"].ToString());
					inventory.fruitList[3] = int.Parse(gameDataJson[0]["fish"].ToString());
					inventory.fruitList[4] = int.Parse(gameDataJson[0]["apple"].ToString());
					inventory.fruitList[5] = int.Parse(gameDataJson[0]["star"].ToString());

					inventoryLoadEvent?.Invoke();
				}
			}
			// JSON ������ �Ľ� ����
			catch (System.Exception e)
			{
				// ���� ������ �ʱⰪ���� ����
				userGameData.Reset();
				inventory.reset();
				// try-catch ���� ���
				Debug.LogError("����!! : " + e);
			}
		}
		// �������� ��
		else
		{
			Debug.LogError($"�κ��丮 ������ �ҷ����⿡ �����߽��ϴ�. : {bro}");
		}

		bro = Backend.GameData.GetMyData("List", new Where());
		// ���� ���� �ҷ����⿡ �������� ��
		if (bro.IsSuccess())
		{
			Debug.Log($"���� ������ �ҷ����⿡ �����߽��ϴ�. : {bro}");

			// JSON ������ �Ľ� ����
			try
			{
				LitJson.JsonData gameDataJson = bro.FlattenRows();

				// �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
				if (gameDataJson.Count <= 0)
				{
					Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
				}
				else
				{
					// �ҷ��� ���� ������ ������
					listDataRowInDate = gameDataJson[0]["inDate"].ToString();
					// �ҷ��� ���� ������ userData ������ ����
					for (int i = 0; i < 8; i++)
					{
						string index = $"Farm_{i}";
						userGameData.ListOfFarm[i] = int.Parse(gameDataJson[0][index].ToString());
					}
					for (int i = 0; i < 11; i++)
					{
						string index = $"Jelly_{i}";
						userGameData.ListOfMonster[i] = int.Parse(gameDataJson[0][index].ToString());
					}
					for (int i = 1; i < 5; i++) {
						for (int j = 0; j < 5; j++)
						{
							string index = $"lv{i}_{j}";
							userGameData.ListOfSkill[i] = int.Parse(gameDataJson[0][index].ToString());
						}
					}
				}
			}
			// JSON ������ �Ľ� ����
			catch (System.Exception e)
			{
				// ���� ������ �ʱⰪ���� ����
				userGameData.Reset();
				inventory.reset();
				// try-catch ���� ���
				Debug.LogError("����!! : " + e);
			}
		}
		// �������� ��
		else
		{
			Debug.LogError($"�κ��丮 ������ �ҷ����⿡ �����߽��ϴ�. : {bro}");
		}
	}

	/// <summary>
	/// �ڳ� �ܼ� ���̺� �ִ� ���� ������ ����
	/// </summary>
	public void GameDataUpdate(UnityAction action = null)
	{
		if (userGameData == null)
		{
			Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�." +
						   "Insert Ȥ�� Load�� ���� �����͸� �������ּ���.");
			return;
		}

		Param param = new Param()
		{
			{ "level",      userGameData.level },
			{ "experience", userGameData.experience },
			{ "seed",       userGameData.seed },
			{ "goldSeed",      userGameData.goldSeed },
			{ "bestStage",      userGameData.bestStage }
		};

		// ���� ������ ������(gameDataRowInDate)�� ������ ���� �޽��� ���
		if (string.IsNullOrEmpty(gameDataRowInDate))
		{
			Debug.LogError($"������ inDate ������ ���� ���� ���� ������ ������ �����߽��ϴ�.");
		}
		// ���� ������ �������� ������ ���̺� ����Ǿ� �ִ� �� �� inDate �÷��� ����
		// �����ϴ� ������ owner_inDate�� ��ġ�ϴ� row�� �˻��Ͽ� �����ϴ� UpdateV2() ȣ��
		else
		{
			Debug.Log($"{gameDataRowInDate}�� ���� ���� ������ ������ ��û�մϴ�.");

			Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");

					action?.Invoke();
				}
				else
				{
					Debug.LogError($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");
				}
			});
		}
	}

	/// <summary>
	/// �ڳ� �ܼ� ���̺� �ִ� �κ��丮 ������ ����
	/// </summary>
	public void InventoryUpdate(UnityAction action = null)
	{
		if (userGameData == null)
		{
			Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�." +
						   "Insert Ȥ�� Load�� ���� �����͸� �������ּ���.");
			return;
		}

		Param param = new Param()
		{
			{ "acornSeed",      inventory.seedList[0] },
			{ "chestnutSeed", inventory.seedList[1] },
			{ "cheeseSeed",       inventory.seedList[2] },
			{ "fishSeed",   inventory.seedList[3] },
			{ "appleSeed",  inventory.seedList[4] },
			{ "randomSeed",  inventory.seedList[5] },


			{ "acorn",  inventory.fruitList[0] },
			{ "chestnut",  inventory.fruitList[1] },
			{ "cheese",  inventory.fruitList[2] },
			{ "fish",  inventory.fruitList[3] },
			{ "apple",  inventory.fruitList[4] },
			{ "star",  inventory.fruitList[5] }
		};

		// ���� ������ ������(gameDataRowInDate)�� ������ ���� �޽��� ���
		if (string.IsNullOrEmpty(inventoryDataRowInDate))
		{
			Debug.LogError($"������ inDate ������ ���� ���� ���� ������ ������ �����߽��ϴ�.");
		}
		// ���� ������ �������� ������ ���̺� ����Ǿ� �ִ� �� �� inDate �÷��� ����
		// �����ϴ� ������ owner_inDate�� ��ġ�ϴ� row�� �˻��Ͽ� �����ϴ� UpdateV2() ȣ��
		else
		{
			Debug.Log($"{inventoryDataRowInDate}�� ���� ���� ������ ������ ��û�մϴ�.");

			Backend.GameData.UpdateV2("Inventory", inventoryDataRowInDate, Backend.UserInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");

					action?.Invoke();
				}
				else
				{
					Debug.LogError($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");
				}
			});
		}
	}

	public void ListUpdate(UnityAction action = null)
	{
		if (userGameData == null)
		{
			Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�." +
						   "Insert Ȥ�� Load�� ���� �����͸� �������ּ���.");
			return;
		}

		Param param = new Param()
		{
			// ��Ȯ
			{ "Farm_0",  userGameData.ListOfFarm[0] },
			{ "Farm_1",  userGameData.ListOfFarm[1] },
			{ "Farm_2",  userGameData.ListOfFarm[2] },
			{ "Farm_3",  userGameData.ListOfFarm[3] },
			{ "Farm_4",  userGameData.ListOfFarm[4] },
			{ "Farm_5",  userGameData.ListOfFarm[5] },
			{ "Farm_6",  userGameData.ListOfFarm[6] },
			{ "Farm_7",  userGameData.ListOfFarm[7] },

			// ����
			{ "Jelly_0",  userGameData.ListOfMonster[0] },
			{ "Jelly_1",  userGameData.ListOfMonster[1] },
			{ "Jelly_2",  userGameData.ListOfMonster[2] },
			{ "Jelly_3",  userGameData.ListOfMonster[3] },
			{ "Jelly_4",  userGameData.ListOfMonster[4] },
			{ "Jelly_5",  userGameData.ListOfMonster[5] },
			{ "Jelly_6",  userGameData.ListOfMonster[6] },
			{ "Jelly_7",  userGameData.ListOfMonster[7] },
			{ "Jelly_8",  userGameData.ListOfMonster[8] },
			{ "Jelly_9",  userGameData.ListOfMonster[9] },
			{ "Jelly_10",  userGameData.ListOfMonster[10] },

			// ��ų
			{ "lv1_0", userGameData.ListOfSkill[0] },
			{ "lv1_1", userGameData.ListOfSkill[1] },
			{ "lv1_2", userGameData.ListOfSkill[2] },
			{ "lv1_3", userGameData.ListOfSkill[3] },
			{ "lv1_4", userGameData.ListOfSkill[4] },

			{ "lv2_0", userGameData.ListOfSkill[5] },
			{ "lv2_1", userGameData.ListOfSkill[6] },
			{ "lv2_2", userGameData.ListOfSkill[7] },
			{ "lv2_3", userGameData.ListOfSkill[8] },
			{ "lv2_4", userGameData.ListOfSkill[9] },

			{ "lv3_0", userGameData.ListOfSkill[10] },
			{ "lv3_1", userGameData.ListOfSkill[11] },
			{ "lv3_2", userGameData.ListOfSkill[12] },
			{ "lv3_3", userGameData.ListOfSkill[13] },
			{ "lv3_4", userGameData.ListOfSkill[14] },

			{ "lv4_0", userGameData.ListOfSkill[15] },
			{ "lv4_1", userGameData.ListOfSkill[16] },
			{ "lv4_2", userGameData.ListOfSkill[17] },
			{ "lv4_3", userGameData.ListOfSkill[18] },
			{ "lv4_4", userGameData.ListOfSkill[19] },
		};

		// ���� ������ ������(gameDataRowInDate)�� ������ ���� �޽��� ���
		if (string.IsNullOrEmpty(listDataRowInDate))
		{
			Debug.LogError($"������ inDate ������ ���� ���� ���� ������ ������ �����߽��ϴ�.");
		}
		// ���� ������ �������� ������ ���̺� ����Ǿ� �ִ� �� �� inDate �÷��� ����
		// �����ϴ� ������ owner_inDate�� ��ġ�ϴ� row�� �˻��Ͽ� �����ϴ� UpdateV2() ȣ��
		else
		{
			Debug.Log($"{listDataRowInDate}�� ���� ���� ������ ������ ��û�մϴ�.");

			Backend.GameData.UpdateV2("List", listDataRowInDate, Backend.UserInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");

					action?.Invoke();
				}
				else
				{
					Debug.LogError($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");
				}
			});
		}
	}
}

