using TechDemo;
using UnityEngine;

public class PlayerJoystickInputProvider : MonoBehaviour, IInputProvider
{
	[SerializeField] private Joystick joystick = null;
	[SerializeField] private FireButton fireButton = null;
	[SerializeField] private JumpButton jumpButton = null;

	public void GetRawInput(out float forwardMovement, out float rightMovement, out bool fire, out bool jump, out float mouseX, out float mouseY)
	{
		forwardMovement = Mathf.Clamp(joystick.Vertical + Input.GetAxis("Vertical"), -1, 1);
		rightMovement = Mathf.Clamp(joystick.Horizontal + Input.GetAxis("Horizontal"), -1, 1);
		fire = fireButton.IsPressed || Input.GetMouseButton(0);
		jump = jumpButton.IsPressed || Input.GetKeyDown(KeyCode.Space);
		mouseX = Input.mousePosition.x;
		mouseY = Input.mousePosition.y;
	}
}
