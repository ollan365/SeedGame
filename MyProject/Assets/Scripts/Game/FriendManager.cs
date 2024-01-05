using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendManager : MonoBehaviour
{
    private int friendIndex;

    [Header("��ų")]
    [SerializeField] private Image Img_Skill;
    private bool isSkillLock;
    private bool isCoolTime;
    private float skillCoolTime;
    public int skillType;
    private int randomNum;
    [SerializeField] private GameObject SkillAcorn;

     [Header("����")]
    public int ShopLevel;
    [SerializeField] private GameObject CatShop;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemPrice;
    [SerializeField] private Text purchaseButtonText;
    [SerializeField] private Button purchaseButton;

    [Header("���� ����Ʈ")]
    [SerializeField] private List<Sprite> lv1_itemSprites;
    [SerializeField] private List<Sprite> lv2_itemSprites;
    [SerializeField] private List<Sprite> lv3_itemSprites;
    [SerializeField] private List<Sprite> lv4_itemSprites;

    [Header("��Ÿ ���")]
    [SerializeField] private GameManager GM;
    [SerializeField] private List<Sprite> friendSprites;
    [SerializeField] private Image friendButtonImg;
    [SerializeField] private GameObject Elephant;
    public bool ElephantLock;
    private void Start()
    {
        friendIndex = 0;

        // ��ų
        skillCoolTime = 3f;
        isSkillLock = true;
        isCoolTime = false;
        skillType = 0;

        // ����
        ShopLevel = 0;

        // �ڳ���
        ElephantLock = true;
    }
    public void OnClickSkill()
    {
        if (BackendGameData.Instance.UserGameData.acorn < 1) // �κ��丮�� ���丮 ������ ��� �Ұ�
            return;

        if(!isSkillLock && !isCoolTime)
        {
            BackendGameData.Instance.UserGameData.acorn -= 1; // ���丮 ����

            SkillAcorn.SetActive(true);
            isCoolTime = true;
            Img_Skill.fillAmount = 0;
            StartCoroutine(CoolTime(skillCoolTime));
        }
    }

    private IEnumerator CoolTime(float coolTime)
    {
        float cool = 0;
        while(cool < coolTime)
        {
            cool += Time.deltaTime;
            cool = cool > coolTime ? coolTime : cool;

            Img_Skill.fillAmount = (cool / coolTime);
            yield return new WaitForFixedUpdate();
        }
        isCoolTime = false;
    }

    public bool unLockFriend()
    {
        switch (friendIndex)
        {
            case 0:
                if (GM.Seed < 200)
                    return true;
                else
                {
                    friendButtonImg.sprite = friendSprites[friendIndex];
                    GM.Seed -= 200;
                    isSkillLock = false;
                    friendIndex++;
                    return true;
                }
            case 1:
                if (GM.Seed < 300)
                    return true;
                else
                {
                    friendButtonImg.sprite = friendSprites[friendIndex];
                    GM.Seed -= 300;
                    ShopLevel++;
                    friendIndex++;
                    return true;
                }
            case 2:
                if (GM.Seed < 500)
                    return true;
                else
                {
                    friendButtonImg.sprite = friendSprites[friendIndex];
                    GM.Seed -= 500;
                    friendIndex++;
                    ElephantLock = false;
                    Elephant.SetActive(true);
                }
                return true;
        }
        return false;
    }
    public bool OpenShop()
    {
        if (ShopLevel == 0 || ShopLevel == 5) // 0�� �ٶ��� �� ������ ����, 5�� �ְ� ���� �������� ����
            return false;


        if (BackendGameData.Instance.UserGameData.cheese < ShopLevel) // �κ��丮�� ġ� ���ڶ� ��� ���� �Ұ���
        {
            purchaseButtonText.text = "�� ��� ��";
            purchaseButton.interactable = false;
        }

        switch (ShopLevel)
        {
            case 1:
                randomNum = Random.Range(0, lv1_itemSprites.Count);
                itemImage.sprite = lv1_itemSprites[randomNum];
                itemPrice.text = $"����: {ShopLevel}��";
                break;
            case 2:
                randomNum = Random.Range(0, lv2_itemSprites.Count);
                itemImage.sprite = lv2_itemSprites[randomNum];
                itemPrice.text = $"����: {ShopLevel}��";
                break;
            case 3:
                randomNum = Random.Range(0, lv3_itemSprites.Count);
                itemImage.sprite = lv3_itemSprites[randomNum];
                itemPrice.text = $"����: {ShopLevel}��";
                break;
            case 4:
                randomNum = Random.Range(0, lv4_itemSprites.Count);
                itemImage.sprite = lv4_itemSprites[randomNum];
                itemPrice.text = $"����: {ShopLevel}��";
                break;
        }
        CatShop.SetActive(true);
        return true;
    }
    public void Purchase()
    {
        BackendGameData.Instance.UserGameData.cheese -= ShopLevel;
        ShopLevel++;
        skillType = randomNum;
        BackendGameData.Instance.UserGameData.ListOfSkill[ShopLevel * 5 + skillType]++;
        Debug.Log($"���� ����: {ShopLevel}");
    }
    public void CloseShop()
    {
        CatShop.SetActive(false);
    }
}
