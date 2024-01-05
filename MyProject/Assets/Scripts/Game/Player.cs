using UnityEngine;

public class Player : MonoBehaviour
{
    public bool gameOver;

    public bool wantToMove;
    private bool isMoving;
    private Vector3 offset;

    private float time;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float seedSpeed;
    [SerializeField] private ObjectManager OM;
    [SerializeField] private GameManager GM;

    [SerializeField] private GameObject lvText;
    [SerializeField] private float height;
    private void Awake()
    {
        gameOver = false;
        wantToMove = true;
        isMoving = false;
    }
    void Update()
    {
        time += Time.deltaTime;

        if (wantToMove && !gameOver)
            Move();
        if(time > attackSpeed && GM.Seed > 0 && !gameOver)
            Attack();

        Vector3 lvPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        lvText.transform.position = lvPos;

    }
    private void Move()
    {
        // ����ڰ� ��ġ�� ��� ������ �����ɴϴ�.
        if (Input.touchCount > 0)
        {
            // ù ��° ��ġ ������ �����ɴϴ�.
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touchPos.y < -6)  // ��ư ��ġ
            {
                isMoving = false;
                return;
            }


            if (touch.phase == TouchPhase.Began)
            {
                Vector3 myPos = transform.position;
                offset = touchPos - myPos;
                isMoving = true;
            }
            else if (touch.phase == TouchPhase.Moved && isMoving)
            {
                // ��ġ�� ��ġ�� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
                touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchPos = touchPos - offset;

                // Z ��ǥ�� ���ϴ� ������ ���� (��: 0���� ����)
                touchPos.z = 0f;
                touchPos.y = -5f;
                touchPos.x *= 1.5f;

                if (touchPos.x > 2.5f) touchPos.x = 2.5f;
                if (touchPos.x < -2.5f) touchPos.x = -2.5f;

                // gameObject�� ��ġ�� ��ġ�� ��ġ�� ����
                transform.position = touchPos;
            }
            else if(touch.phase == TouchPhase.Ended && isMoving)
            {
                offset = Vector3.zero;
                isMoving = false;
            }
        }
        // ���߿� ����� �����Ҷ� ����
        else
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (touchPos.y < -6) // ��ư ��ġ
            {
                isMoving = false;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 myPos = transform.position;
                offset = touchPos - myPos;
                isMoving = true;
            }
            else if (Input.GetMouseButton(0) && isMoving)
            {
                touchPos = touchPos - offset;

                touchPos.z = 0f;
                touchPos.y = -5f;
                touchPos.x *= 1.5f;

                if (touchPos.x > 2.5f) touchPos.x = 2.5f;
                if (touchPos.x < -2.5f) touchPos.x = -2.5f;

                transform.position = touchPos;
            }
            else if (Input.GetMouseButtonUp(0) && isMoving)
            {
                offset = Vector3.zero;
                isMoving = false;
            }
        }
    }

    private void Attack()
    {
        time = 0;
        GM.Seed -= 1;
        GameObject seed = OM.MakeObj("seed");
        seed.transform.position = transform.position;

        Rigidbody2D seedRigid = seed.GetComponent<Rigidbody2D>();
        seedRigid.velocity = Vector2.up * seedSpeed;
    }

    public void AttackSpeedUp()
    {
        attackSpeed /= 2;
        Invoke("SpeedUpEnd", 10);
    }
    private void SpeedUpEnd()
    {
        attackSpeed *= 2;
    }
}
