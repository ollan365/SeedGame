using UnityEngine;
using BackEnd;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TopPanelViewer TM;
    public void OnClickSave()
    {
        // 게임 정보 업데이트
        TM.UpdateGameData();
        BackendGameData.Instance.GameDataUpdate();
        BackendGameData.Instance.InventoryUpdate();
        BackendGameData.Instance.ListUpdate();
    }
    public void OnClickLogOut()
    {
        Backend.BMember.Logout((callback) => {
            Utils.LoadScene("Logo");
        });
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
