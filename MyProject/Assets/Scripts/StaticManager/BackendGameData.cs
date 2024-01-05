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
	/// 뒤끝 콘솔 테이블에 새로운 유저 정보 추가
	/// </summary>
	public void GameDataInsert()
	{
		// 유저 정보를 초기값으로 설정
		userGameData.Reset();

		// 테이블에 추가할 데이터로 가공
		Param param = new Param()
		{
			{ "level",      userGameData.level },
			{ "experience", userGameData.experience },
			{ "seed",       userGameData.seed },
			{ "goldSeed",      userGameData.goldSeed },
			{ "bestStage",      userGameData.bestStage }
		};

		// 첫 번째 매개변수는 뒤끝 콘솔의 "게임 정보 관리" 탭에 생성한 테이블 이름
		BackendReturnObject bro = Backend.GameData.Insert("USER_DATA", param);

		// 게임 정보 추가에 성공했을 때
		if (bro.IsSuccess())
        {
			// 게임 정보의 고유값
			gameDataRowInDate = bro.GetInDate();

			Debug.Log($"게임 정보 데이터 삽입에 성공했습니다. : {bro}");
		}
		// 실패했을 때
		else
		{
			Debug.LogError($"게임 정보 데이터 삽입에 실패했습니다. : {bro}");
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

		// 첫 번째 매개변수는 뒤끝 콘솔의 "인벤토리" 탭에 생성한 테이블 이름
		bro = Backend.GameData.Insert("Inventory", param);

		// 게임 정보 추가에 성공했을 때
		if (bro.IsSuccess())
		{
			// 게임 정보의 고유값
			inventoryDataRowInDate = bro.GetInDate();

			Debug.Log($"게임 정보 데이터 삽입에 성공했습니다. : {bro}");
		}
		// 실패했을 때
		else
		{
			Debug.LogError($"게임 정보 데이터 삽입에 실패했습니다. : {bro}");
		}

		param = new Param()
		{
			// 수확
			{ "Farm_0",  0 },
			{ "Farm_1",  0 },
			{ "Farm_2",  0 },
			{ "Farm_3",  0 },
			{ "Farm_4",  0 },
			{ "Farm_5",  0 },
			{ "Farm_6",  0 },
			{ "Farm_7",  0 },

			// 몬스터
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

			// 스킬
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

		// 첫 번째 매개변수는 뒤끝 콘솔의 "인벤토리" 탭에 생성한 테이블 이름
		bro = Backend.GameData.Insert("List", param);

		// 게임 정보 추가에 성공했을 때
		if (bro.IsSuccess())
		{
			// 게임 정보의 고유값
			listDataRowInDate = bro.GetInDate();

			Debug.Log($"게임 정보 데이터 삽입에 성공했습니다. : {bro}");
		}
		// 실패했을 때
		else
		{
			Debug.LogError($"게임 정보 데이터 삽입에 실패했습니다. : {bro}");
		}
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에서 유저 정보를 불러올 때 호출
	/// </summary>
	public void GameDataLoad()
	{
		userGameData.Reset();

		var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
		// 게임 정보 불러오기에 성공했을 때
		if (bro.IsSuccess())
		{
			Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다. : {bro}");

			// JSON 데이터 파싱 성공
			try
			{
				LitJson.JsonData gameDataJson = bro.FlattenRows();

				// 받아온 데이터의 개수가 0이면 데이터가 없는 것
				if (gameDataJson.Count <= 0)
				{
					Debug.LogWarning("데이터가 존재하지 않습니다.");
				}
				else
				{
					// 불러온 게임 정보의 고유값
					gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
					// 불러온 게임 정보를 userData 변수에 저장
					userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
					userGameData.experience = float.Parse(gameDataJson[0]["experience"].ToString());
					userGameData.seed = int.Parse(gameDataJson[0]["seed"].ToString());
					userGameData.goldSeed = int.Parse(gameDataJson[0]["goldSeed"].ToString());
					userGameData.bestStage = int.Parse(gameDataJson[0]["bestStage"].ToString());

					onGameDataLoadEvent?.Invoke();
				}
			}
			// JSON 데이터 파싱 실패
			catch (System.Exception e)
			{
				// 유저 정보를 초기값으로 설정
				userGameData.Reset();
				inventory.reset();
				// try-catch 에러 출력
				Debug.LogError("에러! : " + e);
			}
		}
		// 실패했을 때
		else
		{
			Debug.LogError($"게임 정보 데이터 불러오기에 실패했습니다. : {bro}");
		}


		bro = Backend.GameData.GetMyData("Inventory", new Where());
		// 게임 정보 불러오기에 성공했을 때
		if (bro.IsSuccess())
		{
			Debug.Log($"인벤토리 데이터 불러오기에 성공했습니다. : {bro}");

			// JSON 데이터 파싱 성공
			try
			{
				LitJson.JsonData gameDataJson = bro.FlattenRows();

				// 받아온 데이터의 개수가 0이면 데이터가 없는 것
				if (gameDataJson.Count <= 0)
				{
					Debug.LogWarning("데이터가 존재하지 않습니다.");
				}
				else
				{
					// 불러온 게임 정보의 고유값
					inventoryDataRowInDate = gameDataJson[0]["inDate"].ToString();
					// 불러온 게임 정보를 userData 변수에 저장
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
			// JSON 데이터 파싱 실패
			catch (System.Exception e)
			{
				// 유저 정보를 초기값으로 설정
				userGameData.Reset();
				inventory.reset();
				// try-catch 에러 출력
				Debug.LogError("에러!! : " + e);
			}
		}
		// 실패했을 때
		else
		{
			Debug.LogError($"인벤토리 데이터 불러오기에 실패했습니다. : {bro}");
		}

		bro = Backend.GameData.GetMyData("List", new Where());
		// 게임 정보 불러오기에 성공했을 때
		if (bro.IsSuccess())
		{
			Debug.Log($"도감 데이터 불러오기에 성공했습니다. : {bro}");

			// JSON 데이터 파싱 성공
			try
			{
				LitJson.JsonData gameDataJson = bro.FlattenRows();

				// 받아온 데이터의 개수가 0이면 데이터가 없는 것
				if (gameDataJson.Count <= 0)
				{
					Debug.LogWarning("데이터가 존재하지 않습니다.");
				}
				else
				{
					// 불러온 게임 정보의 고유값
					listDataRowInDate = gameDataJson[0]["inDate"].ToString();
					// 불러온 게임 정보를 userData 변수에 저장
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
			// JSON 데이터 파싱 실패
			catch (System.Exception e)
			{
				// 유저 정보를 초기값으로 설정
				userGameData.Reset();
				inventory.reset();
				// try-catch 에러 출력
				Debug.LogError("에러!! : " + e);
			}
		}
		// 실패했을 때
		else
		{
			Debug.LogError($"인벤토리 데이터 불러오기에 실패했습니다. : {bro}");
		}
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에 있는 유저 데이터 갱신
	/// </summary>
	public void GameDataUpdate(UnityAction action = null)
	{
		if (userGameData == null)
		{
			Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
						   "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
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

		// 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
		if (string.IsNullOrEmpty(gameDataRowInDate))
		{
			Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
		}
		// 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
		// 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
		else
		{
			Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

			Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {callback}");

					action?.Invoke();
				}
				else
				{
					Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
				}
			});
		}
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에 있는 인벤토리 데이터 갱신
	/// </summary>
	public void InventoryUpdate(UnityAction action = null)
	{
		if (userGameData == null)
		{
			Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
						   "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
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

		// 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
		if (string.IsNullOrEmpty(inventoryDataRowInDate))
		{
			Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
		}
		// 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
		// 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
		else
		{
			Debug.Log($"{inventoryDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

			Backend.GameData.UpdateV2("Inventory", inventoryDataRowInDate, Backend.UserInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {callback}");

					action?.Invoke();
				}
				else
				{
					Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
				}
			});
		}
	}

	public void ListUpdate(UnityAction action = null)
	{
		if (userGameData == null)
		{
			Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
						   "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
			return;
		}

		Param param = new Param()
		{
			// 수확
			{ "Farm_0",  userGameData.ListOfFarm[0] },
			{ "Farm_1",  userGameData.ListOfFarm[1] },
			{ "Farm_2",  userGameData.ListOfFarm[2] },
			{ "Farm_3",  userGameData.ListOfFarm[3] },
			{ "Farm_4",  userGameData.ListOfFarm[4] },
			{ "Farm_5",  userGameData.ListOfFarm[5] },
			{ "Farm_6",  userGameData.ListOfFarm[6] },
			{ "Farm_7",  userGameData.ListOfFarm[7] },

			// 몬스터
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

			// 스킬
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

		// 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
		if (string.IsNullOrEmpty(listDataRowInDate))
		{
			Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
		}
		// 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
		// 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
		else
		{
			Debug.Log($"{listDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

			Backend.GameData.UpdateV2("List", listDataRowInDate, Backend.UserInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {callback}");

					action?.Invoke();
				}
				else
				{
					Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
				}
			});
		}
	}
}

