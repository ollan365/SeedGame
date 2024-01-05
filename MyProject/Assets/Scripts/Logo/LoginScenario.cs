using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class LoginScenario : MonoBehaviour
{
    [SerializeField] private GameObject loginButtonGroup;
    [SerializeField] private GameObject customLoginPopup;
    [SerializeField] private GameObject nickNamePopup;

    [SerializeField]
    private Text textMessage;

    void Start()
    {
        loginButtonGroup.SetActive(false); // 로그인 버튼 비활성화
    }
    public void LoginWithBackendToken()
    {
        BackendReturnObject bro = Backend.BMember.LoginWithTheBackendToken();
        // 이전에 로그인한 정보가 남아있는 경우 -> 자동 로그인
        if (bro.IsSuccess())
        {
            // 닉네임이 없을 경우
            if (string.IsNullOrEmpty(Backend.UserNickName))
            {
                // 닉네임 설정 ui 호출
                nickNamePopup.SetActive(true);
            }
            else
            {
                BackendChartData.LoadAllChart();

                // 게임 화면으로
                Utils.LoadScene("Lobby");
            }
        }
        // 이전에 로그인한 정보가 없는 경우 -> 로그인 버튼 on
        else
        {
            SetButton();
        }
    }

    // 로그인 버튼 연결하기
    private void SetButton()
    {
        if (loginButtonGroup.activeSelf)
        {
            return;
        }
        loginButtonGroup.SetActive(true);

        Button[] buttons = loginButtonGroup.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }

        buttons[0].onClick.AddListener(FederationLogin);
        buttons[1].onClick.AddListener(CustomLogin);
        buttons[2].onClick.AddListener(GuestLogin);

        // 페데레이션 로그인 기능 미구현
        buttons[0].gameObject.SetActive(false);
    }
    // 게스트로그인 함수 호출. 이후 처리는 AuthorizeProcess 참고
    private void GuestLogin()
    {
        Backend.BMember.DeleteGuestInfo();
        BackendReturnObject bro = Backend.BMember.GuestLogin();
        if (!bro.IsSuccess())
        {
            textMessage.text = $"로그인에 실패하였습니다: {bro}";
            return;
        }
        // 계정 생성에 성공했을 때 해당 계정의 게임 정보 생성
        BackendGameData.Instance.GameDataInsert();
        AuthorizeProcess(bro);
    }

    // 로그인 UI 생성
    private void CustomLogin()
    {
        customLoginPopup.SetActive(true);
    }
    private void FederationLogin()
    {

    }

    // 로그인 함수 후 처리 함수
    public void AuthorizeProcess(BackendReturnObject callback)
    {
        Debug.Log($"Backend.BMember.AuthorizeProcess : {callback}");

        // 에러가 발생할 경우 리턴
        // 로그인 버튼 활성화
        if (callback.IsSuccess() == false)
        {
            SetButton();
            return;
        }

        // 닉네임이 없을 경우
        if (string.IsNullOrEmpty(Backend.UserNickName))
        {
            // 닉네임 설정 ui 호출
            nickNamePopup.SetActive(true);
        }
        else
        {
            BackendChartData.LoadAllChart();

            // 데이터 불러오기
            Utils.LoadScene("Lobby");
        }
    }
}