using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        Application.LoadLevel(0);
    }
}
