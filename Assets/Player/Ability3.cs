﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Mirror;

public class Ability3 : AbilityBase
{

    public PlayerModel player;
    private Rigidbody body;
    public GameObject explosionPref;
    public Vector3 boostDir;
    public float explosionRadius;
    public int damage;
    public float cooldown;
    private bool onCooldown;
    private float lifetime;
    
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        lifetime = explosionPref.GetComponent<ParticleSystem>().main.startLifetime.constant;
    }

    [Command]
    void CmdBlastOff()
    {   
        RpcBlastOff();
    }

    [ClientRpc]
    void RpcBlastOff()
    {
        if (onCooldown) return;
        damage = (int)(player.attackDamage * 2.2f);
        
        onCooldown = true;
        StartCoroutine(StartCooldown());
        
        body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z);
        body.AddRelativeForce(boostDir);
        GameObject p = Instantiate(explosionPref, transform.position, Quaternion.identity);

        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider c in cols)
        {
            if (c.GetComponent<PlayerModel>()) continue;

            Health h = c.GetComponent<Health>();
            if (h != null) h.CmdDoDamage(damage);
        }
        
        Destroy(p, lifetime);        
    }

    IEnumerator StartCooldown()
    {
        PlayerUI.instance.QCooldown();
        yield return new WaitForSecondsRealtime(cooldown);
        onCooldown = false;
    }
    
    public override void Enter()
    {
        if(!isLocalPlayer) return; 
        
        CmdBlastOff();
    }
}
