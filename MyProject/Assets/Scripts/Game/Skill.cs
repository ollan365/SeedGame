using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{
    [SerializeField] FriendManager FM;
    private Transform skillTransform;
    private BoxCollider2D skillCollider;
    private Rigidbody2D rigid;
    public int skillType;
    public int shopLevel;
    private void Awake()
    {
        skillTransform = GetComponent<Transform>();
        skillCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        skillType = FM.skillType;
        shopLevel = FM.ShopLevel;

        skillTransform.transform.position = new Vector3(0, -6, 0);
        skillTransform.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        skillCollider.enabled = false;

        StartCoroutine(Casting());
    }
    private IEnumerator Casting()
    {
        float value = 0.01f;
        while(value < 0.1f)
        {
            value += 0.001f;
            skillTransform.transform.localScale = new Vector3(value, value, 1);
            yield return null;
        }
        skillCollider.enabled = true;

        rigid.velocity = Vector2.up * 10;
    }
}
