using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyGameManager : MonoBehaviour
{
    private int seedType;
    public int SeedType
    {
        get { return seedType; }
        set { seedType = value; }
    }

    [SerializeField] private Text message;
    public void Message(string text)
    {
        message.color = new Color(0, 0, 0, 1);
        message.text = text;
        StartCoroutine(fadeText());
    }
    private IEnumerator fadeText()
    {
        float alpha = 1;
        while (alpha != 0)
        {
            alpha = alpha - 0.01f <= 0 ? 0 : alpha - 0.01f;
            message.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
