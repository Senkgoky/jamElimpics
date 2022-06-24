using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class WarpGate : ElympicsMonoBehaviour, IUpdatable
{
    public string enemyPrefab;
    public GameObject spawnAnchor;
    public float counterToStart = 10;
    public GameObject spawnedEnemy;

    void Start()
    {
        
    }

    public void CreateEnemy()
    {
        if (Elympics.IsServer)
        {
            if (spawnedEnemy == null)
            {
                var predictableFor = PredictableFor;
                spawnedEnemy = ElympicsInstantiate(enemyPrefab, predictableFor);
                spawnedEnemy.transform.position = spawnAnchor.transform.position;
                spawnedEnemy.transform.rotation = spawnAnchor.transform.rotation;
            }
        }
    }

    public void ElympicsUpdate()
    {
        counterToStart -= Time.deltaTime;
        if (counterToStart <= 0)
        {
            CreateEnemy();
        }
    }
}
