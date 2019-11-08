﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Damager : MonoBehaviour
{
    
    public int damage;
    public bool destroyOnDamage = true;

    public delegate void HitEvent();
    public event HitEvent OnHitEvent;
    
    //effects
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        
        OnHitEvent?.Invoke();
        
        DoDamage(other);
    }
    
    public void SetDamage(int d)
    {
        damage = d;
    }

    public void DoDamage(Collider other)
    {
        if (!other.GetComponent<Health>() || !other.GetComponent<AIBaseModel>())
        {
            Destroy(this.gameObject);
            return;
        }

        Health healthComp = other.GetComponent<Health>();
        healthComp.DoDamage(damage);
        PlayerEvents.CallPlayerDamageEvent(other.gameObject, damage, other.ClosestPointOnBounds(transform.position));
        
        if (!destroyOnDamage) return;
        Destroy(this.gameObject);
    }
    
}
