using UnityEngine;
using Elympics;

namespace TechDemo
{
	[RequireComponent(typeof(PlayerBehaviour))]
	public class PlayerInputController : ElympicsMonoBehaviour, IInitializable, IInputHandler, IUpdatable
	{
		[SerializeField] private Camera cam = null;
		[SerializeField] private PlayerJoystickInputProvider joystickInputProvider = null;
		[SerializeField] private PlayerBotInputProvider botInputProvider = null;

		private bool _cameraFollowing;
		private PlayerBehaviour _playerBehaviour;

		public Vector3 myMousePos;

		// Handling only one player through this input handlers, every player has the same player input controller
		public void OnInputForClient(IInputWriter inputSerializer)
		{
			joystickInputProvider.GetRawInput(out var forwardMovement, out var rightMovement, out var fire, out var jump, out var mouseX, out var mouseY);
			Vector3 pos = myMousePos;//FindPosHit();
			SerializeInput(inputSerializer, forwardMovement, rightMovement, fire, jump, pos.x, pos.y,pos.z);
		}

		public void OnInputForBot(IInputWriter inputSerializer)
		{
			//botInputProvider.GetRawInput(out var forwardMovement, out var rightMovement, out var fire, out var jump, out var mouseX, out var mouseY);
			//Vector3 pos = FindPosHit();
			//SerializeInput(inputSerializer, forwardMovement, rightMovement, fire, jump, pos.x, pos.y,pos.z);
		}

		private void Update()
		{
			myMousePos = Input.mousePosition;
		}

		public Vector3 FindPosHit(Vector3 mousePos)
        {
			Vector3 result = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(mousePos), out hit, 1000))
            {
                _playerBehaviour.characterAnimator.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
                return _playerBehaviour.characterAnimator.transform.localEulerAngles;
            }
            return result;
		}

		private static void SerializeInput(IInputWriter inputWriter, float forwardMovement, float rightMovement, bool fire, bool jump, float rotX, float rotY, float rotZ)
		{
			inputWriter.Write(forwardMovement);
			inputWriter.Write(rightMovement);
			inputWriter.Write(fire);
			inputWriter.Write(jump);
			inputWriter.Write(rotX);
			inputWriter.Write(rotY);
			inputWriter.Write(rotZ);
			//inputWriter.Write(rotX);
			//inputWriter.Write(rotY);
			//inputWriter.Write(rotZ);
		}

		public void ElympicsUpdate()
		{
			if (!ElympicsBehaviour.TryGetInput(_playerBehaviour.PredictableFor, out var inputReader))
				return;

			inputReader.Read(out float forwardMovement);
			inputReader.Read(out float rightMovement);
			inputReader.Read(out bool fire);
			inputReader.Read(out bool jump);
			inputReader.Read(out float rotX);
			inputReader.Read(out float rotY);
			inputReader.Read(out float rotZ);

			if (fire)
			{
				_playerBehaviour.Fire();
			}

			if (jump)
			{
				_playerBehaviour.Jump();
			}

			_playerBehaviour.Move(forwardMovement, rightMovement);
			FindPosHit(new Vector3(rotX,rotY,rotZ));
		}

		public void Initialize()
		{
			_playerBehaviour = GetComponent<PlayerBehaviour>();
			InitializeCameraFollowing();
		}

		private void InitializeCameraFollowing()
		{
			// Initialize camera only to player played by us
			if (CurrentPlayerControlsThis() || IsServerAndThisIsPlayer0())
			{
				_cameraFollowing = true;
				cam.transform.parent = transform;
				cam.transform.localPosition = new Vector3(0.13f, 3.24f, -2.165f);
				cam.transform.localEulerAngles = new Vector3(52, 0, 0);
			}
		}

		private bool CurrentPlayerControlsThis() => Elympics.Player == _playerBehaviour.PredictableFor;
		private bool IsServerAndThisIsPlayer0() => Elympics.Player == ElympicsPlayer.World && _playerBehaviour.PredictableFor == ElympicsPlayer.FromIndex(0);

		//private void Update()
		//{
			
		//	//	//if (!_cameraFollowing)
		//	//	//	return;

		//	//	//var playerTransform = _playerBehaviour.transform;
		//	//	//var camTransform = cam.transform;

		//	//	//camTransform.LookAt(playerTransform);
		//	//}
		//}
	}
}
