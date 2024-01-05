using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ListManager : MonoBehaviour
{
    [SerializeField] private List<Image> FarmListImg;
    [SerializeField] private List<Sprite> FarmListSprites;
    [SerializeField] private List<Text> FarmListTexts;

    [SerializeField] private List<Image> MosterListImg;
    [SerializeField] private List<Sprite> MosterListSprites;
    [SerializeField] private List<Text> MosterListTexts;

    [SerializeField] private List<Image> SkillListImg;
    [SerializeField] private List<Sprite> SkillListSprites;
    [SerializeField] private List<Text> SkillListTexts;
    public void LoadList()
    {
        for (int i = 0; i < 8; i++)
        {
            if (BackendGameData.Instance.UserGameData.ListOfFarm[i] > 0)
            {
                FarmListImg[i].sprite = FarmListSprites[i];
                FarmListTexts[i].text = BackendGameData.Instance.UserGameData.ListOfFarm[i].ToString();
            }
        }

        for (int i = 0; i < 11; i++)
        {
            if (BackendGameData.Instance.UserGameData.ListOfMonster[i] > 0)
            {
                MosterListImg[i].sprite = MosterListSprites[i];
                MosterListTexts[i].text = BackendGameData.Instance.UserGameData.ListOfMonster[i].ToString();
            }
        }

        for (int i = 0; i < 20; i++)
        {
            if (BackendGameData.Instance.UserGameData.ListOfSkill[i] > 0)
            {
                SkillListImg[i].sprite = SkillListSprites[i];
                SkillListTexts[i].text = BackendGameData.Instance.UserGameData.ListOfSkill[i].ToString();
            }
        }
    }
}
