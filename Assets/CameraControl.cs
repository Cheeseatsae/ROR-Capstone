﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float cameraSpeed = 120f;

    public GameObject followObj;
    
    public float clamp = 80;

    public float sensitivity = 150;

    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float rotY;
    public float rotX;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotX = rotation.x;
        rotY = rotation.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {

        //float inputX = Input.GetAxis("JoyStickHori");
       // float inputZ = Input.GetAxis("JoyStickVert");
        
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = /*inputX +*/ mouseX;
        finalInputZ = /*inputZ +*/ mouseY;

        rotY += finalInputX * sensitivity * Time.deltaTime;
        rotX += finalInputZ * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clamp, clamp);

        Quaternion localRot = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRot;

        Quaternion r = followObj.transform.rotation;
        followObj.transform.rotation = Quaternion.Euler(r.x, rotY, r.z);
        
    }

    private void FixedUpdate()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        Transform target = followObj.transform;

        float step = cameraSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
