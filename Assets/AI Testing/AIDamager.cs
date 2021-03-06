﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDamager : MonoBehaviour
{
    public GameObject owner;
    
    public Vector3 knockupDirection;
    public float damageRadius;
    public float knockupStrength = 15;

    public ParticleSystem particleSystem;
    //Damage Types
    public int slamDamage;

    public int explosionDamage;
    // Start is called before the first frame update
    void Start()
    {
        knockupDirection = new Vector3(0,0.5f,0);
    }
    
    public void Delete()
    {
        Destroy(this.gameObject);
    }

    public LayerMask layer;
    
    public void SlamDamage()
    {

        Collider[] cols = Physics.OverlapSphere(this.gameObject.transform.position, damageRadius, layer);

        foreach (Collider col in cols)
        {
            if (col.GetComponent<PlayerModel>())
            {
                Rigidbody targetRb = col.gameObject.GetComponent<Rigidbody>();

                Vector3 dir = (col.gameObject.transform.position - owner.transform.position) * 3;
                dir.y = 0;
                dir = dir.normalized;
                dir.y = knockupDirection.y;

                ApplyKnockback(col.gameObject, dir * knockupStrength);

                targetRb.velocity = dir * knockupStrength;
                Health health = col.gameObject.GetComponent<Health>();
                health.DoDamage(slamDamage);
            }

        }

        owner.GetComponent<Health>().EventDeath -= Delete;
        Delete();

    }
    
    public void ApplyKnockback(GameObject player, Vector3 dir)
    {
        Rigidbody targetRb = player.GetComponent<Rigidbody>();
        targetRb.velocity =  dir *15;
    }

    public void ExplosionDamage()
    {
        transform.localScale = Vector3.one * damageRadius * 2;
        Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, damageRadius, layer);
        particleSystem.Play();
        foreach (Collider col in cols)
        {
            if (col.GetComponent<PlayerModel>())
            {
                Health health = col.gameObject.GetComponent<Health>();
                health.DoDamage(explosionDamage);
            }

        }

        DeleteExplosion();
    }
    
    public void DeleteExplosion()
    {
        StartCoroutine(WaitASec());
    }
    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("Deleteme");
        Delete();
        
    }




}
