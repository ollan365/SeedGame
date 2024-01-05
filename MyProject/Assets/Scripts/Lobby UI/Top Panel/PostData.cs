using System.Collections.Generic;

public class PostData
{
	public string title;                        // 우편 제목
	public string content;                  // 우편 내용
	public string inDate;                       // 우편 inDate
	public string expirationDate;               // 우편 만료 날짜

	public bool isCanReceive = false;       // 우편에 받을 수 있는 아이템이 있는지 여부

	// <아이템 이름, 아이템 개수>
	public Dictionary<string, int> postReward = new Dictionary<string, int>();
}

