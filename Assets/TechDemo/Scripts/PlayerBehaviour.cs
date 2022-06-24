using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

namespace TechDemo
{
	public class PlayerBehaviour : ElympicsMonoBehaviour, IUpdatable
	{
		private readonly int _fireAnimatorTrigger = Animator.StringToHash("Fire");
		private readonly int _movingAnimatorBool  = Animator.StringToHash("IsMoving");

		[SerializeField] public Animator       characterAnimator = null;
		[SerializeField] private float          speed             = 2;
		[SerializeField] private float          force             = 100;
		[SerializeField] private float          fireDelay         = 0.68f;
		[SerializeField] private float          fireDuration      = 1.2f - 0.68f;
		[SerializeField] private string         ballPrefabName    = "Ball";
		[SerializeField] private Transform      ballAnchor        = null;

		private readonly ElympicsFloat _timerForDelay  = new ElympicsFloat();
		private readonly ElympicsFloat _timerForFiring = new ElympicsFloat();

		private Vector3   _cachedVelocity;
		private Rigidbody _rigidbody;

		private bool IsFiring => _timerForDelay > 0 || _timerForFiring > 0;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			//_timerForDelay.ValueChanged += HandleFireDelayTimerChanged;
		}

		//private void HandleFireDelayTimerChanged(float lastValue, float newValue)
		//{
		//	if (lastValue <= 0 && newValue > 0)
		//		characterAnimator.SetTrigger(_fireAnimatorTrigger);
		//}

		public void Move(float forwardAxis, float rightAxis)
		{
			//if (IsFiring) return;
			Vector3 direction = new Vector3(rightAxis, 0, forwardAxis).normalized;
			Vector3 velocity = direction * speed;
			//transform.LookAt(transform.position + velocity);

			

			
			_rigidbody.velocity = velocity;
		}

		public void Fire()
		{
			//characterAnimator.SetBool(_fireAnimatorTrigger,true);
			//Invoke("StopAttack",1f);
			if (_timerForFiring > 0)
			{
				return;
            }
			else
            {
				_timerForFiring.Value = fireDelay;
				SpawnBall();
            }
			////_timerForDelay.Value = fireDelay;
			////_timerForFiring.Value = fireDuration;
			//_rigidbody.velocity = Vector3.zero;
		}

		public void StopAttack()
        {
			characterAnimator.SetBool(_fireAnimatorTrigger, false);
		}

		public void Jump()
		{
			_rigidbody.AddForce(Vector3.up*20,ForceMode.Impulse);
		}

		private Queue<Action> _destroyActions = new Queue<Action>();

		public void ElympicsUpdate()
		{
			characterAnimator.SetBool(_movingAnimatorBool, _rigidbody.velocity.sqrMagnitude > 0.5f);

			while (_destroyActions.Count > 0)
				_destroyActions.Dequeue().Invoke();

			//if (_timerForDelay > 0) DecreaseDelayTimer();
			if (_timerForFiring > 0) DecreaseFiringTimer();
		}

        private void DecreaseDelayTimer()
        {
			_timerForFiring.Value -= Time.deltaTime;
			//_timerForDelay.Value -= Time.deltaTime;
			//if (_timerForDelay <= 0)
			//{
			//    SpawnBall();
			//    _timerForFiring.Value += _timerForDelay;
			//}
		}

        private void DecreaseFiringTimer()
		{
			_timerForFiring.Value -= Time.deltaTime;
		}

		private void SpawnBall()
		{
			if (!IsPredictableForMe)
				return;
			var predictableFor = PredictableFor;
			var newBall = ElympicsInstantiate(ballPrefabName, predictableFor);
			newBall.transform.position = ballAnchor.transform.position;
			newBall.transform.rotation = ballAnchor.transform.rotation;
			newBall.GetComponent<Rigidbody>().AddForce((newBall.transform.forward/* + newBall.transform.up*/) * force);
		}
	}
}
