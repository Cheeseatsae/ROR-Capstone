﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : Interactable
{
    public GameObject pickup;
    public int cost;
    private bool activated;
    
    public override void Interact(int currency)
    {
        if (activated) return;
        OpenBox(currency);
    }

    public void OpenBox(int currency)
    {
        if (currency < cost) return;
        
        VisualEffect();
        activated = true;
        PlayerInteraction.ChangeMoney(-cost);
        GameObject p = Instantiate(pickup, transform.position + Vector3.up, Quaternion.identity);
        p.GetComponent<Pickup>().PickItem();
        
        p.GetComponent<Rigidbody>().AddForce((Vector3.forward + Vector3.up) * 300);
        
        Destroy(this);
    }

    private void VisualEffect()
    {
        GetComponent<Renderer>().material.color = Color.grey;   
    }
}
