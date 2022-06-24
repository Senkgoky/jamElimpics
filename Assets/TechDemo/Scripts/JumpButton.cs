using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public bool IsPressed { get; private set; }

	public void OnPointerDown(PointerEventData eventData)
	{
		IsPressed = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		IsPressed = false;
	}
}