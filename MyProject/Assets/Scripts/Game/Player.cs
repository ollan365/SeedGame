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
        // 사용자가 터치한 모든 지점을 가져옵니다.
        if (Input.touchCount > 0)
        {
            // 첫 번째 터치 지점을 가져옵니다.
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touchPos.y < -6)  // 버튼 위치
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
                // 터치한 위치의 스크린 좌표를 월드 좌표로 변환
                touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchPos = touchPos - offset;

                // Z 좌표를 원하는 값으로 설정 (예: 0으로 설정)
                touchPos.z = 0f;
                touchPos.y = -5f;
                touchPos.x *= 1.5f;

                if (touchPos.x > 2.5f) touchPos.x = 2.5f;
                if (touchPos.x < -2.5f) touchPos.x = -2.5f;

                // gameObject의 위치를 터치한 위치로 설정
                transform.position = touchPos;
            }
            else if(touch.phase == TouchPhase.Ended && isMoving)
            {
                offset = Vector3.zero;
                isMoving = false;
            }
        }
        // 나중에 모바일 빌드할때 삭제
        else
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (touchPos.y < -6) // 버튼 위치
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
