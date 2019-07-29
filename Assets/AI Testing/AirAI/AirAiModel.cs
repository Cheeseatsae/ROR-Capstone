﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class AirAiModel : AIBaseModel
{
    //General
    public GameObject target;
    private float minDistance = Mathf.Infinity;
    public AIManager aiManager;
    private Rigidbody rb;
    
    //Movement
    public float speed;
    public float minGroundDistance;
    public float maxGroundDistance;
    public Vector3 targetDir;
    public Vector3 upDir;
    public Vector3 downDir;
    public float distance;
    public Vector3 direction;
    
    //Avoidance
    public List<GameObject> NearMe = new List<GameObject>();
    public float toClose;
    
    //Abilities
    public GameObject projectilePref;
    public Transform bulletPos1;
    
    public Transform bulletPos2;

    public Transform currentSpawnPos;
    public float minTargetRange;

    public float projSpeed;

    public bool onCd = false;

    public float flakCooldown;

    public Vector3 targetDirection;
    // Start is called before the first frame update
    void Start()
    {
        aiManager = FindObjectOfType<AIManager>();
        rb = GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    void Update()
    {
        if (!isServer) return;
        foreach (GameObject player in CustomNetManager.players)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance < minDistance)
                {
                    target = player;
                    minDistance = distance;
                }
            }
            
        }   
        
        //Movement
        

        
        upDir = Vector3.zero;
        downDir = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit))
        {
            Debug.DrawLine(gameObject.transform.position, hit.point, Color.red);
            //distance = Vector3.Distance(transform.position, hit.point);
            //To close to ground
            if(Vector3.Distance(transform.position, hit.point) <= minGroundDistance)
            {
                Debug.Log("moveup");
                direction = new Vector3(0,1,0);
                //rb.velocity = -upDir * speed;
            }
            else if (Vector3.Distance(transform.position, hit.point) >= maxGroundDistance)
            {
                Debug.Log("movedown");
                direction = new Vector3(0,-1,0);
                //rb.velocity = downDir * speed;
            }
             
        }
        
        if (target != null)
        {
            targetDir = (target.transform.position - transform.position).normalized;
            targetDir.y = targetDir.y + direction.y;
        }
        //Get Target Direction and look at rotation
        rb.velocity = targetDir * speed;
        
        transform.LookAt(target.transform);
        //Avoidance();
        
        //Abilities
        targetDirection = (target.transform.position - transform.position).normalized;
        if (distance < minTargetRange && onCd == false)
        {
            StartCoroutine(FlakAttack());
            onCd = true;
            StartCoroutine(FlakCooldown());
        }






    }
    //Calculate Movement Deviation
    public Vector3 RandomVector(float min, float max)
    {
        float x = Random.Range(min, max);
        float y = Random.Range(min, max);
        float z = Random.Range(min, max);
        return new Vector3(x,y,z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AirAiModel>())
        {
            NearMe.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (NearMe.Contains(other.gameObject))
        {
            NearMe.Remove(other.gameObject);
        }
    }

    public void Avoidance()
    {
        foreach (GameObject ai in NearMe)
        {
            if (ai == null) return;
            float dist = Vector3.Distance(transform.position, ai.transform.position);
            if (dist <= toClose)
            {
                Vector3 dir = (ai.transform.position - transform.position).normalized;
                rb.velocity = -dir * (speed * 2);
            }
        }
    }
    
    [Command]
    public void CmdFlakCannon(int cannon)
    {
        
        if (cannon == 1)
        {
            currentSpawnPos = bulletPos1;
        } else if (cannon == 2)
        {
            currentSpawnPos = bulletPos2;
        }

        GameObject flak = Instantiate(projectilePref, currentSpawnPos.position, Quaternion.identity);
        Rigidbody flakRb = flak.GetComponent<Rigidbody>();
        flakRb.velocity = targetDirection * projSpeed;
        NetworkServer.Spawn(flak);
    }

    public IEnumerator FlakAttack()
    {
        CmdFlakCannon(1);
        yield return new WaitForSeconds(0.5f);
        CmdFlakCannon(2);
        

    }

    public IEnumerator FlakCooldown()
    {
        yield return new WaitForSeconds(flakCooldown);
        onCd = false;
    }
}