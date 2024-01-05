using System.Collections.Generic;
[System.Serializable]
public class UserGameData
{
	public int level;           // Lobby Scene에 보이는 플레이어 레벨
	public float experience;    // Lobby Scene에 보이는 플레이어 경험치
	public int seed;            // 무료 재화
	public int goldSeed;        // 유료 재화
	public int bestStage;       // 최고 스테이지

	public int acorn;
	public int chestnut;
	public int cheese;
	public int fish;
	public int apple;

	public List<int> ListOfFarm;
	public List<int> ListOfMonster;
	public List<int> ListOfSkill;
	public void Reset()
	{
		level = 1;
		experience = 0;
		seed = 1000;
		goldSeed = 0;
		bestStage = 1;

		ListOfFarm = new List<int>();
		ListOfMonster = new List<int>();
		ListOfSkill = new List<int>();

		for (int i = 0; i < 8; i++)
			ListOfFarm.Add(0);

		for (int i = 0; i < 11; i++)
			ListOfMonster.Add(0);

		for (int i = 0; i < 20; i++)
			ListOfSkill.Add(0);
	}
}