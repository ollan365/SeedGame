using UnityEngine;
using UnityEngine.UI;

public class BreakFarm : MonoBehaviour
{
    [SerializeField] private Button[] btns;
    [SerializeField] private GameObject[] LockList;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            btns[enemy.spawnPoint].interactable = false;
            LockList[enemy.spawnPoint].SetActive(true);

            // spriteµµ ¹Ù²ã¾ß µÊ
        }
    }
}
