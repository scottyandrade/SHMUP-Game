﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    public GameObject[] prefabEnemies;//creating a array to store the three enemies
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    private BoundsCheck bndCheck;

    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);//calling the spawn enemy function once every 2 seconds
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]);//will generate a random number between 0 and 2 to access a raondom enemy in the array using the random index number.
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;//creating a max and min value for the possible starting points of the ships
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;//setting the y component of the enemies to be at the top of the boundry
        go.transform.position = pos;

        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);//calling the invoke again
    }
    
}