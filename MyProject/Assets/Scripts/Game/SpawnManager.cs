using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spawn
{
    public float delay;
    public int type;
    public int point;
}

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;

    public bool gameOver;

    private List<Spawn> spawnList;
    private int spawnIndex;
    private bool spawnEnd;
    private float nextSpawnDelay;
    private float curSpawnDelay = 10;

    [SerializeField] private ObjectManager OM;
    [SerializeField] private GameManager GM;

    [Header("���� ��������")]
    [SerializeField] private Text nextStageText;
    [SerializeField] private Image clockImg;
    [SerializeField] private FriendManager FM;
    private void Awake()
    {
        gameOver = false;
        spawnList = new List<Spawn>();
    }
    public void ReadSpawnFile()
    {
        // ���� �ʱ�ȭ
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // ���� �б�
        TextAsset textFile = Resources.Load("Stage " + GM.Stage.ToString()) as TextAsset; //  as: TextAsset �����̸� ��ȯ �ƴϸ� null ��ȯ
        // TextAsset: �ؽ�Ʈ ����-> Resources���� ���� ������ Load()��
        StringReader stringReader = new StringReader(textFile.text); // StringReader: ���� ���� ���ڿ� ������ �б� Ŭ����

        while (stringReader != null)
        {
            string line = stringReader.ReadLine(); // ReadLine: �ؽ�Ʈ �����͸� �� �پ� ��ȯ(�ڵ� �� �ٲ�)
            if (line == null) return;

            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[2]);
            spawnData.type = int.Parse(line.Split(',')[1]);
            spawnData.point = int.Parse(line.Split(',')[0]);
            spawnList.Add(spawnData);
        }

        stringReader.Close();

        nextSpawnDelay = spawnList[0].delay;

        if (GM.Stage == 10)
            StartCoroutine(Stage_Ten());
    }
    private void Update()
    {
        curSpawnDelay += Time.deltaTime;
        if (curSpawnDelay > nextSpawnDelay && !spawnEnd && !gameOver)
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }
        if (gameOver)
            StopAllCoroutines();
    }
    void SpawnEnemy()
    {
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            StartCoroutine(nextStage());
            return;
        }
        nextSpawnDelay = spawnList[spawnIndex].delay;

        GameObject monster = OM.MakeObj("monster", spawnList[spawnIndex].type);
        Enemy enemy = monster.GetComponent<Enemy>();
        enemy.GM = GM;
        enemy.spawnPoint = spawnList[spawnIndex].point;
        monster.transform.position = spawnPoints[spawnList[spawnIndex].point].position;

        spawnIndex++;
    }

    private IEnumerator Stage_Ten()
    {
        while (!spawnEnd)
        {
            // ��� �ð� ���� (minSpawnCycleTime ~ maxSpawnCycleTime)
            float spawnCycleTime = Random.Range(5f, 10f);
            // spawnCycleTime �ð����� ���
            yield return new WaitForSeconds(spawnCycleTime);

            // ��� ���� ��ġ
            int x = Random.Range(0, 6);

            // ��� ������Ʈ ����
            GameObject alertLineClone = OM.MakeObj("alert");
            Vector3 v = new Vector3(spawnPoints[x].position.x, 0, 0);
            alertLineClone.transform.position = v;

            // 1�� ��� ��
            yield return new WaitForSeconds(1);

            int num = Random.Range(4, 7);
            for (int i = 0; i < num; i++)
            {
                GameObject monster = OM.MakeObj("monster", 0);
                Enemy enemy = monster.GetComponent<Enemy>();
                enemy.GM = GM;
                enemy.spawnPoint = x;
                monster.transform.position = spawnPoints[x].position;

                yield return new WaitForSeconds(1);
            }

            // ��� ������Ʈ ����
            alertLineClone.SetActive(false);
        }
    }


    private IEnumerator nextStage()
    {
        yield return new WaitForSeconds(5f);

        float breakTime = 0;
        if (FM.OpenShop()) breakTime = 3f;

        clockImg.fillAmount = 0;

        while (breakTime > 0)
        {
            breakTime -= Time.deltaTime;
            clockImg.fillAmount = (3 - breakTime) / 3;
            yield return new WaitForFixedUpdate();
        }

        FM.CloseShop();

        nextStageText.color = new Color(0, 0, 0, 1);
        GM.Stage++;
        ReadSpawnFile();

        float alpha = 1;
        while(alpha > 0)
        {
            alpha = alpha - 0.01f <= 0 ? 0 : alpha - 0.01f;
            nextStageText.color = new Color(0, 0, 0, alpha);
            yield return new WaitForFixedUpdate();
        }
    }
}