using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class Nickname : LoginBase
{
	[SerializeField] private Image image;          // ���� ����
	[SerializeField] private InputField inputField;        // �ؽ�Ʈ ���� ����

	[SerializeField]
	private Button btn;            // ��ư (��ȣ�ۿ� ����/�Ұ���)

    public void OnClickCreateNicknameButton()
    {
        // �Ű������� �Է��� InputField UI�� ����� Message ���� �ʱ�ȭ
        ResetUI(image);

        // �ʵ� ���� ����ִ��� üũ
        if (IsFieldDataEmpty(image, inputField.text, "�г���")) return;

        // �α��� ��ư�� ��Ÿ���� ���ϵ��� ��ȣ�ۿ� ��Ȱ��ȭ
        btn.interactable = false;

        // ������ �α����� ��û�ϴ� ���� ȭ�鿡 ����ϴ� ���� ������Ʈ
        // ex) �α��� ���� �ؽ�Ʈ ���, ��Ϲ��� ������ ȸ�� ��
        StartCoroutine(nameof(NicknameProcess));

        // �ڳ� ���� �α��� �õ�
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
            // �г��� ������ �������� ���� "�г���" ��ư ��ȣ�ۿ� Ȱ��ȭ
            btn.interactable = true;

            if (bro.GetStatusCode() == "400")
            {
                if (bro.GetMessage().Contains("undefined nickname"))
                {
                    message = "�г����� ����ֽ��ϴ�.";

                }
                else if (bro.GetMessage().Contains("bad nickname is too long"))
                {
                    message = "20�� �̻��� �Է��� �� �����ϴ�.";

                }
                else if (bro.GetMessage().Contains("bad beginning or end"))
                {
                    message = "�г����� �� Ȥ�� �ڿ� ������ �����մϴ�";
                }
                else
                {
                    message = "�� �� ���� �����Դϴ�.";
                }
            }
            else if (bro.GetStatusCode() == "409")
            {
                message = "�ߺ��� �г����Դϴ�.";
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

            SetMessage($"�ߺ� �˻� ���Դϴ�... {time:F1}");

            yield return null;
        }
    }
}
