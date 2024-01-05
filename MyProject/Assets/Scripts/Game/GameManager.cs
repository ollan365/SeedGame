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
        if (FM.unLockFriend() == false) // ��� ���Ḧ �� Ǯ������ �� �̻� ��ư�� Ŭ������ �ʵ��� �Ѵ�
        {
            friendBtn.interactable = false;
        }
    }

    [SerializeField] private DailyRankRegister dailyRank;
    [SerializeField] private ObjectManager OM;
    [SerializeField] private UnityEvent onGameOver;
    public void GameOver()
    {
        // �ߺ� ó�� ���� �ʵ��� bool ������ ����
        if (isGameOver == true) return;
        isGameOver = true;

        // ���� ���� �Ǿ��� �� ȣ���� �޼ҵ� ����
        onGameOver.Invoke();

        // ���� ���� �� ���� spawn ����, ���� ���� ����
        SM.gameOver = true;
        player.gameOver = true;
        OM.DeleteObj("all");

        // ���� �����
        BackendGameData.Instance.UserGameData.seed -= 1000;

        // stage��ŭ gold seed ����
        BackendGameData.Instance.UserGameData.goldSeed += Stage;

        // best stage���� Ȯ�� -> ������ ������Ʈ
        if (BackendGameData.Instance.UserGameData.bestStage < Stage)
        {
            BackendGameData.Instance.UserGameData.bestStage = Stage;

            dailyRank.Process(BackendGameData.Instance.UserGameData.bestStage);
        }

        // ������ ������Ʈ
        BackendGameData.Instance.GameDataUpdate();
        BackendGameData.Instance.InventoryUpdate();
        BackendGameData.Instance.ListUpdate();
    }
}
