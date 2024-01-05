using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private int seed;
    [SerializeField] private Text seedText;
    public int Seed
    {
        get { return seed; }
        set 
        { 
            seed = value;
            seedText.text = seed.ToString();
        }
    }

    private int level;
    [SerializeField] private Text lvText;
    public int Level
    {
        get { return level; }
        set { level = value; lvText.text = "Lv." + level.ToString(); }
    }

    private int stage;
    [SerializeField] private Text stageText;
    public int Stage
    {
        get { return stage; }
        set 
        { 
            stage = value;
            stageText.text = "Stage " + stage.ToString();
        }
    }

    private bool isGameOver;
    private List<bool> life;

    [SerializeField] private SpawnManager SM;
    [SerializeField] private FriendManager FM;
    private void Awake()
    {
        seed = 1000;
        level = 1;
        Stage = 0;
        life = new List<bool>(6);
        for (int i = 0; i < 6; i++)
            life.Add(true);
        isGameOver = false;
    }
    private void Start()
    {
        SM.ReadSpawnFile();
    }

    public void MinusLife(int point)
    {
        life[point] = false;

        for(int i = 0; i < 6; i++)
        {
            if (life[i]) return;
        }

        GameOver();
    }

    [SerializeField] private Player player;
    public void OnClick(int type) 
    {
        switch (type)
        {
            case 1:
                OnClickSeedButton();
                break;
            case 2:
                OnClickHeartButton();
                break;
            case 3:
                OnClickFriendButton();
                break;
        }
    }
    private void OnClickSeedButton()
    {
        if (Seed < 100)
            return;

        Seed -= 100;
        player.AttackSpeedUp();
    }
    private void OnClickHeartButton()
    {
        if (Seed < 100)
            return;

        Seed -= 100;
        Level++;
    }
    [SerializeField] private Button friendBtn;
    private void OnClickFriendButton()
    {
        if (FM.unLockFriend() == false) // 모든 동료를 다 풀었으면 더 이상 버튼이 클릭되지 않도록 한다
        {
            friendBtn.interactable = false;
        }
    }

    [SerializeField] private DailyRankRegister dailyRank;
    [SerializeField] private ObjectManager OM;
    [SerializeField] private UnityEvent onGameOver;
    public void GameOver()
    {
        // 중복 처리 되지 않도록 bool 변수로 제어
        if (isGameOver == true) return;
        isGameOver = true;

        // 게임 오버 되었을 때 호출할 메소드 실행
        onGameOver.Invoke();

        // 게임 종료 시 몬스터 spawn 중지, 맵의 몬스터 삭제
        SM.gameOver = true;
        player.gameOver = true;
        OM.DeleteObj("all");

        // 게임 입장료
        BackendGameData.Instance.UserGameData.seed -= 1000;

        // stage만큼 gold seed 보상
        BackendGameData.Instance.UserGameData.goldSeed += Stage;

        // best stage인지 확인 -> 맞으면 업데이트
        if (BackendGameData.Instance.UserGameData.bestStage < Stage)
        {
            BackendGameData.Instance.UserGameData.bestStage = Stage;

            dailyRank.Process(BackendGameData.Instance.UserGameData.bestStage);
        }

        // 서버에 업데이트
        BackendGameData.Instance.GameDataUpdate();
        BackendGameData.Instance.InventoryUpdate();
        BackendGameData.Instance.ListUpdate();
    }
}
