﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static AIManager aiManager;
    
    public GameObject playerBulletPrefab;
    public GameObject playerPrefab;
    
    public List<GameObject> spawnPoints = new List<GameObject>();

    public GameObject player;
    
    private void Awake()
    {
        instance = this;
        aiManager = GetComponent<AIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject spawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        player = Instantiate(playerPrefab, spawn.transform.position, spawn.transform.rotation);

        player.GetComponent<Ability1>().bulletPref = SetupPlayer();
        aiManager.player = player;
    }

    public GameObject SetupPlayer()
    {
        Vector3 spawnPos = new Vector3(0,-15000,0);
        GameObject obj = Instantiate(playerBulletPrefab, spawnPos, Quaternion.identity);
        
        return obj;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
