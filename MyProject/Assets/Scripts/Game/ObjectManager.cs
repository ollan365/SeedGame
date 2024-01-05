using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject seedPrefab;
    public GameObject monsterPrefab;
    public GameObject alertPrefab;

    GameObject[] seed;
    GameObject[] monster;
    GameObject[] alert;

    GameObject[] targetPool;

    private void Awake()
    {
        seed = new GameObject[50];
        monster = new GameObject[40];
        alert = new GameObject[6];
        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < seed.Length; i++)
        {
            seed[i] = Instantiate(seedPrefab);
            seed[i].SetActive(false);
        }
        for (int i = 0; i < monster.Length; i++)
        {
            monster[i] = Instantiate(monsterPrefab);
            monster[i].SetActive(false);
        }
        for (int i = 0; i < alert.Length; i++)
        {
            alert[i] = Instantiate(alertPrefab);
            alert[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "seed":
                targetPool = seed;
                break;
            case "alert":
                targetPool = alert;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }
    public GameObject MakeObj(string type, int index)
    {
        if (type == "monster")
            targetPool = monster;

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                Enemy e = targetPool[i].GetComponent<Enemy>();
                e.monsterIndex = index;
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }
    public void DeleteObj(string type)
    {
        if (type == "seed" || type == "all")
        {
            for (int index = 0; index < seed.Length; index++)
                seed[index].SetActive(false);
        }
        if (type == "monster" || type == "all")
        {
            for (int index = 0; index < monster.Length; index++)
                monster[index].SetActive(false);
        }
        if (type == "alert" || type == "all")
        {
            for (int index = 0; index < alert.Length; index++)
                alert[index].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}
