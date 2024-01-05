using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class Nickname : LoginBase
{
	[SerializeField] private Image image;          // 색상 변경
	[SerializeField] private InputField inputField;        // 텍스트 정보 추출

	[SerializeField]
	private Button btn;            // 버튼 (상호작용 가능/불가능)

    public void OnClickCreateNicknameButton()
    {
        // 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
        ResetUI(image);

        // 필드 값이 비어있는지 체크
        if (IsFieldDataEmpty(image, inputField.text, "닉네임")) return;

        // 로그인 버튼을 연타하지 못하도록 상호작용 비활성화
        btn.interactable = false;

        // 서버에 로그인을 요청하는 동안 화면에 출력하는 내용 업데이트
        // ex) 로그인 관련 텍스트 출력, 톱니바퀴 아이콘 회전 등
        StartCoroutine(nameof(NicknameProcess));

        // 뒤끝 서버 로그인 시도
        ResponseToNickname(inputField.text);
    }
    private void ResponseToNickname(string text)
    {
        string nickname = text;
        string message = string.Empty;

        BackendReturnObject bro = Backend.BMember.UpdateNickname(nickname);
        StopCoroutine(nameof(NicknameProcess));
        if (bro.IsSuccess())
        {
            BackendChartData.LoadAllChart();

            Utils.LoadScene("Lobby");
        }
        else
        {
            // 닉네임 생성에 실패했을 때는 "닉네임" 버튼 상호작용 활성화
            btn.interactable = true;

            if (bro.GetStatusCode() == "400")
            {
                if (bro.GetMessage().Contains("undefined nickname"))
                {
                    message = "닉네임이 비어있습니다.";

                }
                else if (bro.GetMessage().Contains("bad nickname is too long"))
                {
                    message = "20자 이상은 입력할 수 없습니다.";

                }
                else if (bro.GetMessage().Contains("bad beginning or end"))
                {
                    message = "닉네임이 앞 혹은 뒤에 공백이 존재합니다";
                }
                else
                {
                    message = "알 수 없는 에러입니다.";
                }
            }
            else if (bro.GetStatusCode() == "409")
            {
                message = "중복된 닉네임입니다.";
            }
            SetMessage(message);
        }
    }

    private IEnumerator NicknameProcess()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            SetMessage($"중복 검사 중입니다... {time:F1}");

            yield return null;
        }
    }
}
