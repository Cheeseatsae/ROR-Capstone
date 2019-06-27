﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = System.Random;


public class AIManager : NetworkBehaviour
{
     
    public List<GameObject> Players = new List<GameObject>();

    public int maxEnemySpawnDistance;
    public int minEnemySpawnDistance;

    public GameObject groundAI;
    


    [Command]
    public void CmdSpawnArounfPlayer()
    {
        foreach (GameObject player in Players)
        {
            float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
            
            Vector3 dir = new Vector3(Mathf.Sin(angle),0,Mathf.Cos(angle));

            dir *= UnityEngine.Random.Range(minEnemySpawnDistance, maxEnemySpawnDistance);

            GameObject AI = Instantiate(groundAI, player.transform.position + dir, Quaternion.identity);
            NetworkServer.Spawn(AI);

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CmdSpawnArounfPlayer();
        }
    }
}