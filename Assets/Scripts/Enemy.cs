using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using TechDemo;

public class Enemy : ElympicsMonoBehaviour, IUpdatable
{
	private readonly ElympicsInt _ticksAlive = new ElympicsInt();

	public PlayerBehaviour[] players;
	public Transform target;
	public float speed = 20;
	public Rigidbody _rigidbody;

	public float energy = 100;

	private void Start()
	{
		players = GameObject.FindObjectsOfType<PlayerBehaviour>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void ElympicsUpdate()
	{
		if (!IsPredictableForMe)
			return;
		_ticksAlive.Value++;


		for (int i = 0; i < players.Length; i++)
		{
			if (Vector3.SqrMagnitude(players[i].transform.position - transform.position) < 200)
			{
				target = players[i].transform;

			}
		}

		if (target != null)
		{
			transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

			_rigidbody.velocity = (target.position - transform.position).normalized * speed * Time.deltaTime;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("BlueBall") || collision.gameObject.CompareTag("RedBall"))
        {
			energy -= 10;
			ElympicsDestroy(collision.gameObject);

			if(energy<=0)
            {
				ElympicsDestroy(gameObject);

			}
        }
    }
}
