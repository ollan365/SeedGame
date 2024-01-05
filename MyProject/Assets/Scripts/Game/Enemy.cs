using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float HP;
    private float originHP;
    [SerializeField] public int monsterIndex;
    [SerializeField] private Sprite[] monsterImages;
    [SerializeField] public int spawnPoint;
    [SerializeField] private ParticleSystem ps;

    // 상태 제어
    private bool isDie;

    public GameManager GM;

    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private BoxCollider2D col;

    [Header("체력바")]
    [SerializeField] private GameObject HP_Bar;
    [SerializeField] private Slider HP_Slider;
    public float height;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Vector3 hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        HP_Bar.transform.position = hpBarPos;
    }
    private void OnEnable()
    {
        sr.sprite = monsterImages[monsterIndex];
        isDie = false;

        switch (monsterIndex)
        {
            case 0:
                moveSpeed = 0.5f;
                HP = 3;
                break;
            case 1:
                moveSpeed = 0.6f;
                HP = 4;
                break;
            case 2:
                moveSpeed = 0.5f;
                HP = 6;
                break;
            case 3:
                moveSpeed = 1f;
                HP = 5;
                break;
            case 4:
                moveSpeed = 1f;
                HP = 3;
                break;
            case 5:
                moveSpeed = 0.25f;
                HP = 10;
                break;
        }
        originHP = HP;
        rigid.velocity = Vector2.down * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Seed")
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(OnHit(GM.Level));
        }
        else if (collision.tag == "Acorn")
        {
            Skill acorn = collision.GetComponent<Skill>();
            OnHitSkill(acorn.skillType, acorn.shopLevel);
        }
        else if (collision.tag == "Break Point")
        {
            GM.MinusLife(spawnPoint);
            StartCoroutine(OnHit(100)); // 자연스럽게 사라지게 만든다
        }
    }
    private IEnumerator OnHit(int damage)
    {
        HP -= damage;
        HP_Slider.value = (HP > 0 ? HP : 0) / originHP;

        if(HP > 0)
        { 
            sr.color = Color.red;
            float alpha = 0;
            while (alpha != 1)
            {
                alpha = alpha + 0.1f >= 1 ? 1 : alpha + 0.01f;
                sr.color = new Color(1, alpha, alpha);
                yield return null;
            }
        }

        if (HP <= 0 && isDie == false)
        {
            BackendGameData.Instance.UserGameData.ListOfMonster[monsterIndex]++;

            isDie = true;
            col.enabled = false;
            ps.Play();

            float alpha = 1;
            while (ps.isPlaying == true)
            {
                alpha = alpha - 0.01f < 0 ? alpha : alpha - 0.01f;
                sr.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            HP_Slider.value = 1;
            col.enabled = true;
            sr.color = new Color(1, 1, 1, 1);
            gameObject.SetActive(false);
        }
    }
    private void OnHitSkill(int type, int shopLevel)
    {
        switch (type) // 스킬에 따라 다르게 데미지 (일단은 같은 데미지)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                HP -= GM.Level * shopLevel;
                break;
        }
        HP_Slider.value = (HP > 0 ? HP : 0) / originHP;

        if (HP > 0) // 아직 살아있는 경우
        {
            switch (type)
            {
                case 0:
                    StartCoroutine(OnHit(0));
                    break;
                case 1:
                    StartCoroutine(OnElec(shopLevel));
                    break;
                case 2:
                    StartCoroutine(OnFire(shopLevel));
                    break;
                case 3:
                    StartCoroutine(OnIce(shopLevel));
                    break;
                case 4:
                    StartCoroutine(OnHit(0));
                    break;
            }
        }
        else if (HP <= 0 && isDie == false) // 몬스터가 죽었을 경우
        {
            StartCoroutine(OnHit(0));
        }
    }

    private IEnumerator OnFire(int shopLevel)
    {
        while (HP >= 0)
        {
            StartCoroutine(OnHit(shopLevel));

            yield return new WaitForSeconds(4f);
        }
    }
    private IEnumerator OnIce(int shopLevel)
    {
        sr.color = Color.blue;
        moveSpeed /= (shopLevel + 1);
        rigid.velocity = Vector2.down * moveSpeed;

        float time = 0;
        while(time < 3 + shopLevel)
        {
            time += Time.deltaTime;
            yield return null;
        }

        sr.color = Color.white;
        moveSpeed *= (shopLevel + 1);
        rigid.velocity = Vector2.down * moveSpeed;
    }
    private IEnumerator OnElec(int shopLevel)
    {
        sr.color = Color.yellow;

        float speed = moveSpeed;
        moveSpeed = 0;
        rigid.velocity = Vector2.down * moveSpeed;

        float time = 0;
        while (time < 1 + shopLevel)
        {
            time += Time.deltaTime;
            yield return null;
        }

        moveSpeed = speed;
        rigid.velocity = Vector2.down * moveSpeed;
    }
}
