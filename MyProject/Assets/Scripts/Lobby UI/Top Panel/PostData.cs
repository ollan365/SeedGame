using System.Collections.Generic;

public class PostData
{
	public string title;                        // ���� ����
	public string content;                  // ���� ����
	public string inDate;                       // ���� inDate
	public string expirationDate;               // ���� ���� ��¥

	public bool isCanReceive = false;       // ���� ���� �� �ִ� �������� �ִ��� ����

	// <������ �̸�, ������ ����>
	public Dictionary<string, int> postReward = new Dictionary<string, int>();
}

