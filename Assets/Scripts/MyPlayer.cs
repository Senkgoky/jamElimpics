using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class MyPlayer : ElympicsMonoBehaviour, IInputHandler, IUpdatable
{
    public float speed = 5;
    public void OnInputForClient(IInputWriter inputSerializer)
    {
        if (Elympics.Player != PredictableFor)
            return;

        float movementX = Input.GetAxis("Horizontal");

        float movementY = Input.GetAxis("Vertical");
        bool fire = Input.GetMouseButtonDown(0);

        Debug.Log("MOVE X " + movementX);

        inputSerializer.Write(movementX);
        inputSerializer.Write(movementY);
        inputSerializer.Write(fire);
    }

    public void OnInputForBot(IInputWriter inputSerializer)
    {

    }

    public void ElympicsUpdate()
    {
        var movementX = 0.0f;
        var movementY = 0.0f;
        bool fire = false;

        if (ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
        {
            inputReader.Read(out movementX);
            inputReader.Read(out movementY);
        }

        Move(new Vector2(movementX, movementY));
        if (fire)
        {
            Attack();
        }
    }

    //private void Update()
    //{
    //    float movementX = Input.GetAxis("Horizontal");

    //    float movementY = Input.GetAxis("Vertical");
    //    Move(new Vector2(movementX, movementY));
    //}

    public void Move(Vector2 moveValue)
    {
        transform.position += (transform.forward * moveValue.y + transform.right*moveValue.x) * speed * Elympics.TickDuration;
    }

    public void Attack()
    {
    }
}
