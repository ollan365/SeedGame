using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> CustomerButton;
    [SerializeField] private List<Image> menuImageList;
    [SerializeField] private List<Sprite> menuSpriteList;
    [SerializeField] private List<Text> menuTextList;

    private List<int> FruitType;
    private List<int> FruitNum;

    [SerializeField] private List<GameObject> ObjectList;
    [SerializeField] private List<SpriteRenderer> SpriteList;
    [SerializeField] private List<Sprite> CustomerSpriteList;

    [SerializeField] private Inventory IM;
    [SerializeField] private TopPanelViewer TM;
    private void Awake()
    {
        FruitType = new List<int>();
        FruitNum = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            FruitType.Add(0);
            FruitNum.Add(0);
        }
    }
    private void Start()
    {
        randomCustomer(0);
        randomCustomer(1);
        randomCustomer(2);
    }

    public void OnClick(int pos)
    {
        if(CustomerButton[pos].activeSelf)
        {
            Transaction(pos);
        }
    }
    private void Transaction(int pos)
    {
        if( IM.getFruitNum(FruitType[pos * 2]) >= FruitNum[pos*2] && 
            IM.getFruitNum(FruitType[pos * 2 + 1]) >= FruitNum[pos * 2 + 1])
        {
            // 거래 개수만큼 과일 차감
            IM.ChangeFruitNum(FruitType[pos * 2], (-1) * FruitNum[pos * 2]);
            IM.ChangeFruitNum(FruitType[pos * 2 + 1], (-1) * FruitNum[pos * 2 + 1]);

            // 거래 물품과 개수에 비례하여 씨앗 얻기
            int price = (FruitNum[pos * 2] + FruitNum[pos * 2 + 1]) * (FruitNum[pos * 2] + FruitNum[pos * 2 + 1]);
            price *= FruitType[pos * 2 + 1] * FruitType[pos * 2 + 1];
            if (FruitType[pos * 2 + 1] == 5 && FruitNum[pos * 2 + 1] == 1) price *= 10; // special 씨앗을 거래 성공한 경우
            BackendGameData.Instance.UserGameData.seed += price;

            // 게임 정보 업데이트
            TM.UpdateGameData();
            BackendGameData.Instance.InventoryUpdate();
            BackendGameData.Instance.GameDataUpdate();

            CustomerButton[pos].SetActive(false);
            StartCoroutine(DoTransaction(pos));
        }
        else
            checkCustomerMenuNum();
    }
    private IEnumerator DoTransaction(int pos)
    {
        float alpha = 1;
        while (alpha != 0)
        {
            alpha = alpha - 0.01f <= 0 ? 0 : alpha - 0.01f;
            SpriteList[pos].color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        SpriteList[pos].color = new Color(1, 1, 1, 1);

        switch (pos)
        {
            case 0:
                ObjectList[pos].transform.position = new Vector3(-2, 8, 0);
                break;
            case 1:
                ObjectList[pos].transform.position = new Vector3(0, 8, 0);
                break;
            case 2:
                ObjectList[pos].transform.position = new Vector3(2, 8, 0);
                break;
        }

        randomCustomer(pos);
    }

    private void randomCustomer(int pos)
    {
        int customer = Random.Range(0, 3);
        SpriteList[pos].sprite = CustomerSpriteList[customer];
        if(customer == 2)
            ObjectList[pos].transform.localScale = new Vector3(1.5f, 1.5f, 1);
        else
            ObjectList[pos].transform.localScale = new Vector3(1, 1, 1);

        switch (customer)
        {
            case 0:
                FruitType[pos * 2] = 0;
                menuImageList[pos * 2].sprite = menuSpriteList[0];
                FruitType[pos * 2 + 1] = 1;
                menuImageList[pos * 2 + 1].sprite = menuSpriteList[1];

                FruitNum[pos * 2] = Random.Range(1, 10);
                menuTextList[pos * 2].text = FruitNum[pos * 2].ToString();
                FruitNum[pos * 2 + 1] = Random.Range(1, 10);
                menuTextList[pos * 2 + 1].text = FruitNum[pos * 2 + 1].ToString();
                break;

            case 1:
                FruitType[pos * 2] = 2;
                menuImageList[pos * 2].sprite = menuSpriteList[2];
                FruitType[pos * 2 + 1] = 3;
                menuImageList[pos * 2 + 1].sprite = menuSpriteList[3];

                FruitNum[pos * 2] = Random.Range(1, 8);
                menuTextList[pos * 2].text = FruitNum[pos * 2].ToString();
                FruitNum[pos * 2 + 1] = Random.Range(1, 8);
                menuTextList[pos * 2 + 1].text = FruitNum[pos * 2 + 1].ToString();
                break;

            case 2:
                FruitType[pos * 2] = 4;
                menuImageList[pos * 2].sprite = menuSpriteList[4];
                FruitType[pos * 2 + 1] = 5;
                menuImageList[pos * 2 + 1].sprite = menuSpriteList[5];

                FruitNum[pos * 2] = Random.Range(1, 6);
                menuTextList[pos * 2].text = FruitNum[pos * 2].ToString();
                FruitNum[pos * 2 + 1] = Random.Range(1, 11) / 10;
                menuTextList[pos * 2 + 1].text = FruitNum[pos * 2 + 1].ToString();
                break;

        }
        StartCoroutine(MoveCustomer(pos));
    }

    private IEnumerator MoveCustomer(int pos)
    {
        float x = -2 + pos * 2;
        float y = 8;
        while (y > 3)
        {
            y = y - 0.01f;
            ObjectList[pos].transform.position = new Vector3(x, y, 0);
            yield return null;
        }

        checkCustomerMenuNum();
        CustomerButton[pos].SetActive(true);
    }

    public void checkCustomerMenuNum()
    {
        for(int i = 0; i < 6; i++)
        {
            if (IM.getFruitNum(FruitType[i]) >= FruitNum[i])
                menuTextList[i].color = Color.black;
            else
                menuTextList[i].color = Color.red;
        }
    }
}
