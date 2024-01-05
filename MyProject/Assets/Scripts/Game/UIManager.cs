using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform mainCam;
    [SerializeField] private Transform miniCam;
    [SerializeField] private Transform staticUI;
    [SerializeField] private Canvas staticCanvas;
    [SerializeField] private Player player;

    private void Awake()
    {
        mainCam.position = new Vector3(0, 0, -10);
        miniCam.position = new Vector3(0, -13, -10);
    }
    public void OnClick(float des)
    {
        StartCoroutine(MoveCam(des));

        if(des < 0) // ³óÀåÀ¸·Î °¬À» ¶§
        {
            miniCam.position = new Vector3(0, -2.6f, -10);
            player.wantToMove = false;
        }
        else // ÀüÀåÀ¸·Î °¬À» ¶§
        {
            miniCam.position = new Vector3(0, -13, -10);
            player.wantToMove = true;
        }
    }
    private IEnumerator MoveCam(float des)
    {
        while(mainCam.position.y > des)
        {
            staticCanvas.transform.position = staticUI.position;

            mainCam.position += Vector3.down * 0.5f;
            yield return null;
        }

        while (mainCam.position.y < des)
        {
            staticCanvas.transform.position = staticUI.position;

            mainCam.position += Vector3.up * 0.5f;
            yield return null;
        }
    }

}
