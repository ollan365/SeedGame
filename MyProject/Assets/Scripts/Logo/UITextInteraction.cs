using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UITextInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[System.Serializable]
	private class OnClickEvent : UnityEvent { }

	// Text UI를 클릭했을 때 호출하고 싶은 메소드 등록
	[SerializeField]
	private OnClickEvent onClickEvent;

	// 색상이 바뀌고, 터치가 되는 TextMeshProGUI
	private Text text;
	public void Awake()
	{
		text = GetComponent<Text>();
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		text.fontStyle = FontStyle.Bold;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		text.fontStyle = FontStyle.Normal;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		onClickEvent?.Invoke();
	}
}

