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
        loginButtonGroup.SetActive(false); // �α��� ��ư ��Ȱ��ȭ
    }
    public void LoginWithBackendToken()
    {
        BackendReturnObject bro = Backend.BMember.LoginWithTheBackendToken();
        // ������ �α����� ������ �����ִ� ��� -> �ڵ� �α���
        if (bro.IsSuccess())
        {
            // �г����� ���� ���
            if (string.IsNullOrEmpty(Backend.UserNickName))
            {
                // �г��� ���� ui ȣ��
                nickNamePopup.SetActive(true);
            }
            else
            {
                BackendChartData.LoadAllChart();

                // ���� ȭ������
                Utils.LoadScene("Lobby");
            }
        }
        // ������ �α����� ������ ���� ��� -> �α��� ��ư on
        else
        {
            SetButton();
        }
    }

    // �α��� ��ư �����ϱ�
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

        // �䵥���̼� �α��� ��� �̱���
        buttons[0].gameObject.SetActive(false);
    }
    // �Խ�Ʈ�α��� �Լ� ȣ��. ���� ó���� AuthorizeProcess ����
    private void GuestLogin()
    {
        Backend.BMember.DeleteGuestInfo();
        BackendReturnObject bro = Backend.BMember.GuestLogin();
        if (!bro.IsSuccess())
        {
            textMessage.text = $"�α��ο� �����Ͽ����ϴ�: {bro}";
            return;
        }
        // ���� ������ �������� �� �ش� ������ ���� ���� ����
        BackendGameData.Instance.GameDataInsert();
        AuthorizeProcess(bro);
    }

    // �α��� UI ����
    private void CustomLogin()
    {
        customLoginPopup.SetActive(true);
    }
    private void FederationLogin()
    {

    }

    // �α��� �Լ� �� ó�� �Լ�
    public void AuthorizeProcess(BackendReturnObject callback)
    {
        Debug.Log($"Backend.BMember.AuthorizeProcess : {callback}");

        // ������ �߻��� ��� ����
        // �α��� ��ư Ȱ��ȭ
        if (callback.IsSuccess() == false)
        {
            SetButton();
            return;
        }

        // �г����� ���� ���
        if (string.IsNullOrEmpty(Backend.UserNickName))
        {
            // �г��� ���� ui ȣ��
            nickNamePopup.SetActive(true);
        }
        else
        {
            BackendChartData.LoadAllChart();

            // ������ �ҷ�����
            Utils.LoadScene("Lobby");
        }
    }
}