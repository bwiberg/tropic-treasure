﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

//[ExecuteInEditMode]
public class spawnBox : MonoBehaviour {

    private string difficultyKey = "difficulty";
    private int RandomSeed = -1;
    private int enemyCount;
    private float maxRange;
    private int testCount;
    private float threshold;

    public int difficulty;
    
    // public spawn parameters
    public GameObject enemyPrefab;
    public float radius;
    public float spawnRatePerSec;
    public GameObject chest;

    private void Start()
    {
        enemyCount = 0;
        maxRange = 1000.0f;
        testCount = 100;
        threshold = 1;

        //difficulty = PlayerPrefs.GetInt(difficultyKey);

        if (RandomSeed != -1)
        {
            Random.InitState(RandomSeed);
        }

        InvokeRepeating("spawnEnemy",5.0f,0.1f/spawnRatePerSec);
        if(difficulty != 0)
            InvokeRepeating("incSpawnRate", 15.0f, 10.0f);
    }

    public void spawnEnemy() {
        for (int i = 0; i < testCount; i++)
        {
            float spawnTest = Random.Range(0, maxRange);
            if (spawnTest < threshold)
            {
                float angle = Random.Range(0, 2 * Mathf.PI);
                var polarPos = new Utility.Polar(radius, angle);
                Vector3 startPos = polarPos.Cartesian3D;

                var enemy = GameObject.Instantiate(enemyPrefab, startPos,Quaternion.identity);
                enemy.name = string.Format("enemy_{0}", enemyCount);
                enemy.transform.SetParent(gameObject.transform);

                enemyCount++;
            }
        }
	}

    private void incSpawnRate()
    {
        if (maxRange > threshold * 10)
            switch (difficulty)
            {
                case (1):
                    maxRange -= 10;
                    break;
                case (2):
                    maxRange /= 1.25f;
                    break;
                default:
                    break;
            }
    }
}