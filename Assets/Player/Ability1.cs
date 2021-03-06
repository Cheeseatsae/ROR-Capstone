﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability1 : AbilityBase
{
    public PlayerModel player;
    public GameObject bulletPref;
    [HideInInspector] public float rangeInMetres;

    public float baseCooldown;
    [HideInInspector] public float cooldown;
    private bool onCooldown;

    public bool firing;
    
    public float projectileSpeed;
    public Transform aimTransform;

    public PlayerModel.AnimationAction AnimationEventAbility1;
    
    private void Start()
    {
        firing = false;
        onCooldown = false;
    }
    
    public void Fire(float lifeTime, Vector3 target)
    {
        AnimationEventAbility1?.Invoke();
        player.audio.PlaySound(0);
        GameObject bullet = Instantiate(bulletPref, aimTransform.position, Quaternion.Euler(90,90,0));
        
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = (target - bullet.transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;
        
        Destroy(bullet, lifeTime);
        
        bullet.GetComponent<Projectile>().SetVelocity(bulletRb.velocity);
        bullet.GetComponent<Damager>().SetDamage((int)player.attackDamage);
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSecondsRealtime(cooldown);
        onCooldown = false;
    }
    
    public override void Enter()
    {
        firing = true;
    }

    public override void Exit()
    {
        firing = false;
    }

    private void FixedUpdate()
    {
        if (onCooldown) return;
        if (!firing) return;
        if (player.attackOccupied) return;
        
        PlayerUI.instance.LMouseCooldown();
        onCooldown = true;
        cooldown = baseCooldown / (0.1f * player.attackSpeed);
        
        Fire(player.attackRange, player.target);
        StartCoroutine(StartCooldown());
    }
}
