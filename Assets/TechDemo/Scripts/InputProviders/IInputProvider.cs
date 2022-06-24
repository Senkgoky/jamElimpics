namespace TechDemo
{
	public interface IInputProvider
	{
		void GetRawInput(out float forwardMovement, out float rightMovement, out bool fire, out bool jump,out float mouseX, out float mouseY);
	}
}
