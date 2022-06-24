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


		public Vector3 sumPos;

		// Handling only one player through this input handlers, every player has the same player input controller
		public void OnInputForClient(IInputWriter inputSerializer)
		{
			joystickInputProvider.GetRawInput(out var forwardMovement, out var rightMovement, out var fire, out var jump);
			Vector3 res = FindPosHit(sumPos);
			SerializeInput(inputSerializer, forwardMovement, rightMovement, fire, jump, res.x, res.y, res.z);
			sumPos = Vector3.zero;
		}

		public void OnInputForBot(IInputWriter inputSerializer)
		{
		}

		private void Update()
		{
			sumPos = Input.mousePosition;
		}


		private static void SerializeInput(IInputWriter inputWriter, float forwardMovement, float rightMovement, bool fire, bool jump, float posX, float posY, float posZ)
		{
			inputWriter.Write(forwardMovement);
			inputWriter.Write(rightMovement);
			inputWriter.Write(fire);
			inputWriter.Write(jump);
			inputWriter.Write(posX);
			inputWriter.Write(posY);
			inputWriter.Write(posZ);
		}

		public void ElympicsUpdate()
		{
			if (!ElympicsBehaviour.TryGetInput(_playerBehaviour.PredictableFor, out var inputReader))
				return;

			inputReader.Read(out float forwardMovement);
			inputReader.Read(out float rightMovement);
			inputReader.Read(out bool fire);
			inputReader.Read(out bool jump);
			inputReader.Read(out float posX);
			inputReader.Read(out float posY);
			inputReader.Read(out float posZ);

			if (fire)
			{
				_playerBehaviour.Fire();
			}

			if (jump)
			{
				_playerBehaviour.Jump();
			}

			_playerBehaviour.Move(forwardMovement, rightMovement);
			_playerBehaviour.characterAnimator.transform.localEulerAngles = new Vector3(posX,posY,posZ);

		}

		public Vector3 FindPosHit(Vector3 mousePos)
		{
			Vector3 result = Vector3.zero;
			RaycastHit hit;
			if (Physics.Raycast(cam.ScreenPointToRay(mousePos), out hit, 1000))
			{
				_playerBehaviour.characterAnimator.transform.LookAt(new Vector3(hit.point.x, _playerBehaviour.characterAnimator.transform.position.y, hit.point.z));
				return _playerBehaviour.characterAnimator.transform.localEulerAngles;
			}
			return result;
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
	}
}
