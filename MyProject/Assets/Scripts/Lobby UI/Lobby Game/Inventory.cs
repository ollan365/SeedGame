using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    public List<int> seedList;
    public List<int> fruitList;

    [Header("씨앗 판넬")]
    [SerializeField] private List<Toggle> seedPanelToggle;
    [SerializeField] private List<Text> seedPanelText;

    [SerializeField] private CustomerManager CM;

    [Header("인벤토리")]
    [SerializeField] private List<Text> MySeedList;
    [SerializeField] private List<Text> MyFruitList;

    [Header("상점")]
    private List<int> PriceList;
    private List<int> MenuNumList;
    [SerializeField] private TopPanelViewer TM;

    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            seedList.Add(10);
            fruitList.Add(0);
        }

        PriceList = new List<int>(); MenuNumList = new List<int>();
        PriceList.Add(10); MenuNumList.Add(10);
        PriceList.Add(20); MenuNumList.Add(10);
        PriceList.Add(30); MenuNumList.Add(10);
        PriceList.Add(40); MenuNumList.Add(10);
        PriceList.Add(50); MenuNumList.Add(10);
        PriceList.Add(50); MenuNumList.Add(1);

        BackendGameData.Instance.inventoryLoadEvent.AddListener(LoadData);
    }
    public void reset()
    {
        for(int i = 0; i < 6; i++)
            seedList[i] = 10;

        for(int i = 0; i < 6; i++)
            fruitList[i] = 0;
    }
    public void LoadData()
    {
        for (int i = 0; i < seedPanelToggle.Count; i++)
        {
            if (seedPanelToggle[i].isOn == false)
            {
                seedPanelText[i].color = Color.white;
                seedPanelText[i].text = "열매 " + fruitList[i].ToString() + "개";
            }
            else
            {
                seedPanelText[i].color = Color.black;
                seedPanelText[i].text = "씨앗 " + seedList[i].ToString() + "개";
            }
        }
    }
    public void ChangeFruitNum(int type, int value)
    {
        switch (type)
        {
            case 6:
                BackendGameData.Instance.UserGameData.seed += 500;
                TM.UpdateGameData();
                break;
            case 7:
                BackendGameData.Instance.UserGameData.goldSeed += 1;
                TM.UpdateGameData();
                break;
            default:
                fruitList[type] += value;
                CM.checkCustomerMenuNum();
                LoadData();
                break;
        }
    }
    public void ChangeSeedNum(int type, int value)
    {
        seedList[type] += value;
        LoadData();
    }

    public int getFruitNum(int type)
    {
        return fruitList[type];
    }
    public int getSeedNum(int type)
    {
        return seedList[type];
    }

    public void LoadInventory()
    {
        for (int i = 0; i < MySeedList.Count; i++)
        {
            MySeedList[i].text = seedList[i].ToString() + "개";
            MyFruitList[i].text = fruitList[i].ToString() + "개";
        }
    }

    public void Purchase(int type)
    {
        if(BackendGameData.Instance.UserGameData.seed >= PriceList[type])
        {
            BackendGameData.Instance.UserGameData.seed -= PriceList[type];
            seedList[type] += MenuNumList[type];
            LoadData();


            // 게임 정보 업데이트
            TM.UpdateGameData();
            BackendGameData.Instance.GameDataUpdate();
            BackendGameData.Instance.InventoryUpdate();
        }
    }
}
